using UnityEngine;
using VehiclePhysics;
using System;
using Perrinn424.AerodynamicsSystem;


public class Perrinn424Aerodynamics : VehicleBehaviour
{
	private Atmosphere atmosphere = new Atmosphere();

    [SerializeField]
    private AltitudeConverter altitudeConverter;

	[Serializable]
	public class AeroSettings
	{
		public Transform applicationPoint;

		// Aerodynamic model coefficients
		public float constant                    = 1.0f;
		public float frontRideHeightCoefficient  = 1.0f;
		public float frontRideHeight2Coefficient = 1.0f;
		public float rearRideHeightCoefficient   = 1.0f;
		public float rearRideHeight2Coefficient  = 1.0f;
		public float absoluteYawCoefficient      = 1.0f;
		public float absoluteSteerCoefficient    = 1.0f;
		public float absoluteRollCoefficient     = 1.0f;
		public float dRS_Coefficient             = 1.0f;
		public float frontFlapCoefficient        = 1.0f;
	}

	public float deltaISA                  = 0.0f;
	public float dRSActivationDelay        = 0.0f;
	public float dRSActivationTime         = 0.0f;
	public float frontFlapStaticAngle      = 0.0f;
	public float frontFlapFlexDeltaAngle   = 0.0f;
	public float frontFlapFlexMaxDownforce = 0.0f;

	public AeroSettings front = new AeroSettings();
	public AeroSettings rear  = new AeroSettings();
	public AeroSettings drag  = new AeroSettings();

	[HideInInspector] public float flapAngle   = 1.0f;
	[HideInInspector] public bool  DRSclosing  = false;
	[HideInInspector] public float DRS      = 0;
	[HideInInspector] public float SCzFront = 0;
	[HideInInspector] public float SCzRear  = 0;
	[HideInInspector] public float SCx      = 0;
	[HideInInspector] public float downforceFront  = 0.0f;
	[HideInInspector] public float downforceRear   = 0.0f;
	[HideInInspector] public float aeroBal         = 0.0f;
	[HideInInspector] public float dragForce       = 0.0f;
	[HideInInspector] public float yawAngle        = 0.0f;
	[HideInInspector] public float steerAngle      = 0.0f;
	[HideInInspector] public float rollAngle       = 0.0f;
	[HideInInspector] public float fronRollAngle   = 0.0f;
	[HideInInspector] public float rearRollAngle   = 0.0f;
	[HideInInspector] public float frontRideHeight = 0.0f;
	[HideInInspector] public float rearRideHeight  = 0.0f;
	[HideInInspector] public float rho = 0.0f;
	float DRStime = 0;
	

	// Function Name: CalcAeroCoeff
	// This function calculates a given aerodynamic coefficient based on:
	//
	//	 [IN]	aeroSetting [AeroSettings]
	//	 [IN]	fRH_mm [mm]
	//	 [IN]	rRH_mm [mm]
	//	 [IN]	yawAngle_deg [deg]
	//	 [IN]	steerAngle_deg [deg]
	//	 [IN]	rollAngle_deg [deg]
	//	 [IN]	DRS [-]
	//	 [IN]	flapAngle_deg [deg]
	//
	//	 [OUT]	SCn [m2]
	float CalcAeroCoeff(AeroSettings aeroSetting, float fRH_mm, float rRH_mm, float yawAngle_deg, float steerAngle_deg, float rollAngle_deg, float DRSpos, float flapAngle_deg)
	{
		// Assigning return variable
		float SCn;

		// Checking limits before calculating forces
		fRH_mm = Math.Min(Math.Max(fRH_mm, 0), 100);
		rRH_mm = Math.Min(Math.Max(rRH_mm, 0), 100);
		DRSpos = Math.Min(Math.Max(DRSpos, 0), 1);
		flapAngle_deg = Math.Min(Math.Max(flapAngle_deg, -5), 5);
		yawAngle_deg = Math.Min(Math.Max(Math.Abs(yawAngle_deg), 0), 10);
		steerAngle_deg = Math.Min(Math.Max(Math.Abs(steerAngle_deg), 0), 20);
		rollAngle_deg = Math.Min(Math.Max(Math.Abs(rollAngle_deg), 0), 3);

		// Calculate total force
		SCn = aeroSetting.constant +
			  aeroSetting.frontRideHeightCoefficient * fRH_mm +
			  aeroSetting.frontRideHeight2Coefficient * fRH_mm * fRH_mm +
			  aeroSetting.rearRideHeightCoefficient * rRH_mm +
			  aeroSetting.rearRideHeight2Coefficient * rRH_mm * rRH_mm +
			  aeroSetting.absoluteYawCoefficient * yawAngle_deg +
			  aeroSetting.absoluteSteerCoefficient * steerAngle_deg +
			  aeroSetting.absoluteRollCoefficient * rollAngle_deg +
			  aeroSetting.dRS_Coefficient * DRSpos +
			  aeroSetting.frontFlapCoefficient * flapAngle_deg;
		return SCn;
	}


	//  Function Name: CalcDRSPosition
	//  This function calculates the DRS position
	//
	//	 [IN]	throttlePos: 0 to 1 [-]
	//	 [IN]	brakePos:    0 to 1 [-]
	//	 [IN]	DRSpos:      0 to 1 [-]
	//
	//	 [OUT]	DRSpos:      0 to 1 [-]
		float CalcDRSPosition(float throttlePos, float brakePos, float DRSpos)
	{
		if (throttlePos == 1 && brakePos == 0 && !DRSclosing)
		{
			DRStime -= Time.deltaTime;
			if (DRStime <= 0.0f)
				DRSpos += Time.deltaTime * (1 / dRSActivationTime);
		}
		else
		{
			DRSclosing = true;
			if (DRSpos == 0)
			{
				DRSclosing = false;
			}
			DRSpos -= Time.deltaTime * (1 / dRSActivationTime);
			DRStime = dRSActivationDelay;
		}
		DRSpos = Math.Min(Math.Max(DRSpos, 0), 1);
		return DRSpos;
	}


	public override void FixedUpdateVehicle()
	{
		Rigidbody rb = vehicle.cachedRigidbody;

		// Getting driver's input
		int[] input            = vehicle.data.Get(Channel.Input);
		float throttlePosition = input[InputData.Throttle] / 10000.0f;
		float brakePosition    = input[InputData.Brake] / 10000.0f;

        float dynamicPressure = CalculateDynamicPressure();
		rho = (float)atmosphere.Density;

		// Setting vehicle parameters for the aero model
		yawAngle        = vehicle.speedAngle;
		steerAngle      = (vehicle.wheelState[0].steerAngle + vehicle.wheelState[1].steerAngle) / 2;
		fronRollAngle   = vehicle.data.Get(Channel.Custom, Perrinn424Data.FrontRollAngle) / 1000.0f;
		rearRollAngle   = vehicle.data.Get(Channel.Custom, Perrinn424Data.RearRollAngle) / 1000.0f;
		rollAngle       = (fronRollAngle + rearRollAngle) / 2;
		frontRideHeight = vehicle.data.Get(Channel.Custom, Perrinn424Data.FrontRideHeight);
		rearRideHeight  = vehicle.data.Get(Channel.Custom, Perrinn424Data.RearRideHeight);

		// Calculating DRS position and feeding to the car data bus
		DRS = CalcDRSPosition(throttlePosition, brakePosition, DRS);
		vehicle.data.Set(Channel.Custom, Perrinn424Data.DrsPosition, Mathf.RoundToInt(DRS * 1000));

		// Calculating front flap deflection due to aeroelasticity
		flapAngle = frontFlapStaticAngle + downforceFront * frontFlapFlexDeltaAngle / frontFlapFlexMaxDownforce;
		flapAngle = Math.Min(Math.Max(flapAngle, -5), 5);

		// Calculating aero forces
		if (front.applicationPoint != null)
		{
			SCzFront = CalcAeroCoeff(front, frontRideHeight, rearRideHeight, yawAngle, steerAngle, rollAngle, DRS, flapAngle);
			Vector3 VEC_SCzFront = -SCzFront * dynamicPressure * front.applicationPoint.up;
			rb.AddForceAtPosition(VEC_SCzFront, front.applicationPoint.position);
		}

		if (rear.applicationPoint != null)
		{
			SCzRear = CalcAeroCoeff(rear, frontRideHeight, rearRideHeight, yawAngle, steerAngle, rollAngle, DRS, flapAngle);
			Vector3 VEC_SCzRear = -SCzRear * dynamicPressure * rear.applicationPoint.up;
			rb.AddForceAtPosition(VEC_SCzRear, rear.applicationPoint.position);
		}

		if (drag.applicationPoint != null)
		{
			SCx = CalcAeroCoeff(drag, frontRideHeight, rearRideHeight, yawAngle, steerAngle, rollAngle, DRS, flapAngle);
			Vector3 VEC_SCx = -SCx * dynamicPressure * drag.applicationPoint.forward;
			rb.AddForceAtPosition(VEC_SCx, drag.applicationPoint.position);
		}

		downforceFront = SCzFront * dynamicPressure;
		downforceRear  = SCzRear  * dynamicPressure;
		dragForce      = SCx      * dynamicPressure;
		aeroBal	= downforceFront / (downforceFront + downforceRear) * 100;
	}

    private float CalculateDynamicPressure()
    {
        Rigidbody rb = vehicle.cachedRigidbody;
        float vSquared = rb.velocity.sqrMagnitude;
        float y = rb.worldCenterOfMass.y;
        float altitude = altitudeConverter.ToAltitude(y);
        altitude = 1.0f; // backwards compatibility with saved replay
        atmosphere.UpdateAtmosphere(altitude, deltaISA);
        float dynamicPressure = (float)(atmosphere.Density * vSquared / 2.0);
        return dynamicPressure;
    }
}
