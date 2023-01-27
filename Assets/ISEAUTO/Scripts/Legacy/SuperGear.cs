using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;


public enum JointDirection
{
    PASS_THROUGH=1,
    STRAIGHT=0,
    REVERSE=-1
}

public enum GearApplication{
    Guided,
    Unguided,
    ScaleBased
}



public abstract class SuperGear : MonoBehaviour
{
    public int ToothCount;
    public float DiameterMillimiters;
    public float ThicknessMillimiters;
    public List<SuperGear> JointGears;
    public List<JointDirection> Direction;
    public float AngularVelocity;
    public Axis axis;
    public GearApplication app;
    public float torque;
    
    public float ratio=1f;
    //public float ratioPrint=1f;
    
    public bool printer;
    protected Vector3 rotate;
    protected Rigidbody r;
    protected Transform t;
    
    
    // Start is called before the first frame update
    protected void Start()
    {
        rotate = new Vector3();
        
        r = gameObject.GetComponent<Rigidbody>();
        t = gameObject.GetComponent<Transform>();
        r.maxAngularVelocity = Mathf.Infinity;

        switch (app)
        {
            case GearApplication.Guided:
                DiameterMillimiters = t.localScale.x * 100;
                ThicknessMillimiters = t.localScale.z * 100;
                break;
            case GearApplication.Unguided:
                break;
            case GearApplication.ScaleBased:
                
                Vector3 scale;
                scale.x = DiameterMillimiters / 100;
                scale.y = DiameterMillimiters / 100;
                scale.z = ThicknessMillimiters / 100;
                t.localScale = scale;
                break;
            
        }
        
    }

  
}
