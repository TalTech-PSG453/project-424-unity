using System;
using System.Collections.Generic;
using EdyCommonTools;
using ISEAUTO.Scripts.Path_Generation;
using Sirenix.OdinInspector;
using UnityEngine;
using VehiclePhysics;

namespace ISEAUTO.Scripts.Controller
{
    [Serializable]
    public struct PIDParameters
    {
        public float P;
        public float I;
        public float D;
        
        public float maxParametric;
        public float maxIntegral;
        public float maxDerivative;
    }
    public class VehicleVelocityController : MonoBehaviour
    {
        public Vector3 startPosition;
        public TextAsset pathData;
        public List<float[]> Position_Velocity = new List<float[]>();

        public float throutleOut;
        public float realVelocity;
        public float setpoint;
        public PidController pidController = new PidController();
        public PIDParameters pidParameters;
        public CSVReaderFloat reader;


        public Perrinn424CarController carController;

        private void Start()
        {
            pidController.SetParameters(pidParameters.P,pidParameters.I,pidParameters.D);
            pidController.maxOutput = 1f;
            pidController.minOutput = 0f;
            startPosition = transform.position;
            reader = new CSVReaderFloat(pathData);
            Position_Velocity=reader.GetColumns(new int[]{0,3},reader.GetArrayList());
            //setpoint = Mathf.Max(0.5f,Position_Velocity[0][1]);



        }

        public int counter=0;
        public float currentDistance = 0;
        
        public float defaultTime = 0;
        public bool start;
        public bool runing;
        private void Update()
        {
            // if (start)
            // {
            //     defaultTime = Time.realtimeSinceStartup;
            //     start = false;
            //     runing = true;
            // }
            //
            // if (runing)
            // {
            //     currentDistance = Time.realtimeSinceStartup - defaultTime;
            //
            //     /*Vector3.Distance(transform.position, startPosition)*/
            //     
            //
            //     if (currentDistance > Position_Velocity[counter][0])
            //     {
            //         counter++;
            //         setpoint = Mathf.Max(0.5f, Position_Velocity[counter][1]);
            //
            //
            //     }
            // }



            pidController.setpoint = setpoint/3.6f;
            pidController.input = carController.speed;
            pidController.Compute();
            carController.autoThrotle = pidController.output;
        }
    }
    
    
}