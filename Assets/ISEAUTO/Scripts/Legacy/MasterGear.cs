using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;





public class MasterGear : SuperGear
{
   

    void Update()
    {
        
        rotate =Vector3.zero;

        switch (axis)
        {
            case Axis.X:
                
                AngularVelocity= transform.InverseTransformDirection(r.angularVelocity).x;;
                break;
            case Axis.Y:
                
                AngularVelocity= transform.InverseTransformDirection(r.angularVelocity).y;
                break;
            case Axis.Z:
                
                AngularVelocity= transform.InverseTransformDirection(r.angularVelocity).z;
                break;
        }
            
        
        
        if(JointGears.Count==0)
            return;
        int i = 0;
        
        foreach (var g in JointGears)
        {
            ratio = (float) ToothCount / (float) g.ToothCount;
            switch (Direction[i])
            {
                case JointDirection.STRAIGHT:
                    
                    g.AngularVelocity = AngularVelocity * (ratio);
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque / ratio;
                    }
                    break;
                case JointDirection.REVERSE:
                    g.AngularVelocity = -AngularVelocity * (ratio);
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque / ratio;
                    }
                    break;
                case JointDirection.PASS_THROUGH:
                    g.AngularVelocity = AngularVelocity;
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque ;
                    }
                    break;
            }

            i++;



        } 
        
        
        
        
    }
}
