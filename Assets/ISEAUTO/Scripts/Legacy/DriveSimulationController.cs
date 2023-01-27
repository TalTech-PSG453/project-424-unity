using System;
using System.Collections;
using System.Collections.Generic;
using EE.TalTech.IVAR.DigitalizationProject.ROS;
using UnityEngine;
using UnityEngine.Events;

public class DriveSimulationController : MonoBehaviour
{
    #region Set up variables

    public bool RunningROS = false;
    public float maximumVelocity;

    public UnityEvent ToggleForceVisuals=new UnityEvent();

    public UnityEvent ToggleTransparencyVisuals=new UnityEvent();
    public UnityEvent ToggleDisableBody=new UnityEvent();

    public UnityEvent ToggleCloseCamera=new UnityEvent();
    public bool DifferentialDrive;
    #endregion



    #region Scene References

    [Header("Scene References")]
    public ForceVectorVisualizer LeftTorqueVisualizer;
    public ForceVectorVisualizer RightTorqueVisualizer;
    public ForceVectorVisualizer DriveMotorTorqueVisualizer;
    
    public Float32Subscriber LeftTorqueRosSubscriber;
    public Float32Subscriber RightTorqueRosSubscriber;
    public Float32Subscriber DriveMotorTorqueRosSubscriber;
    
    public AngularVelocitySetter LeftWheel;
    public AngularVelocitySetter RightWheel;
    public AngularVelocitySetter DriveMotor;
    #endregion

    public bool testBenchMode;

    public AdvancedMotorVisualizer leftLoad;
    public AdvancedMotorVisualizer rightLoad;
    /*public void onMoveLeft(InputAction.CallbackContext context)
    {
        var move = context.ReadValue<float>();
        LeftWheel.AngularVelocity = move*maximumVelocity;
        print(move);
    }*/

    void Start()
    {
        if (!RunningROS)
        {
            LeftTorqueVisualizer.MaximumTorque = maximumVelocity*1f;
            RightTorqueVisualizer.MaximumTorque = maximumVelocity*1f;
        }
    }

    private Vector2 drive;

    void Update()
    {
//        print(Input.GetAxis("Right"));
        if (DifferentialDrive)
        {
            drive = new Vector2(Input.GetAxis("RightX"), Input.GetAxis("Left"));



            LeftWheel.AngularVelocity = (drive.y - drive.x) * maximumVelocity * 0.75f;
            RightWheel.AngularVelocity = (drive.y + drive.x) * maximumVelocity * 0.75f;
        }
        else
        {
            LeftWheel.AngularVelocity = Input.GetAxis("Left") * maximumVelocity;
            RightWheel.AngularVelocity = Input.GetAxis("Right") * maximumVelocity;
        }

        if (Input.GetButtonDown("Y"))
            ToggleForceVisuals?.Invoke();
        if (Input.GetButtonDown("X"))
            ToggleTransparencyVisuals?.Invoke();
        if (Input.GetButtonDown("A"))
            ToggleForceVisuals?.Invoke();
        if (Input.GetButtonDown("B"))
            ToggleTransparencyVisuals?.Invoke();
        if (!RunningROS)
        {
            LeftTorqueVisualizer.Torque = LeftWheel.AngularVelocity;
            RightTorqueVisualizer.Torque = RightWheel.AngularVelocity;
        }
        else
        {


            LeftTorqueVisualizer.Torque = LeftTorqueRosSubscriber.value;
            RightTorqueVisualizer.Torque = RightTorqueRosSubscriber.value;
        }

        if (testBenchMode)
        {
            leftLoad.anguarVelocity = -LeftWheel.AngularVelocity;
        

            rightLoad.anguarVelocity = -RightWheel.AngularVelocity;

        }

}
    void setListAngularVel(List<AngularVelocitySetter> list, float velocity)
    {
        foreach (var item in list)
        {
            item.AngularVelocity = velocity;
        }
    }
}
