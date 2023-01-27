using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;





public class VisualGear : SuperGear
{

    // Update is called once per frame
    void Update()
    {
        
        rotate =Vector3.zero;

        switch (axis)
        {
            case Axis.X:
                
                rotate.x = AngularVelocity;
                t.Rotate(rotate, Space.Self);
                break;
            case Axis.Y:
                
                rotate.y = AngularVelocity;
                t.Rotate(rotate, Space.Self);
                break;
            case Axis.Z:
                
                rotate.z = AngularVelocity*Mathf.Rad2Deg;
                t.Rotate(rotate*Time.deltaTime, Space.Self);
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
                case JointDirection.PASS_THROUGH:
                    
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
                case JointDirection.STRAIGHT:
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

