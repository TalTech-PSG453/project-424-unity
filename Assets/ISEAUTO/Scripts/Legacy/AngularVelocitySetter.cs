using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AngularVelocitySetter : MonoBehaviour
{
    private Rigidbody rotor;
    public int Axis;
    public float AngularVelocity;
    public bool Inverted;
    private void Start()
    {
        rotor = GetComponent<Rigidbody>();
        rotor.maxAngularVelocity=0;
    }

    private Vector3 force;
    private float old_vel;
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == Axis)
                force[i] = (Inverted?1f:-1f)*Mathf.Sign(AngularVelocity)*100000;
            else
            {
                force[i] = 0;
            }

            
            
            if(old_vel!=AngularVelocity)
                
                    rotor.maxAngularVelocity = AngularVelocity;
            old_vel = AngularVelocity;
            rotor.AddRelativeTorque(force);
        }
    }
    
}
