using System;
using System.Collections;
using System.Collections.Generic;
using Andriy.ROS.Publishers;
using DigitalTwin.Utils.Events;
using EE.TalTech.IVAR.DigitalizationProject.ROS;
using UnityEngine;
using UnityEngine.Events;

public class AdvancedMotorVisualizer : MonoBehaviour
{
    public GameObject Rotor;
    public List<GameObject> Windings;
    [Range(0f,1f)]
    public List<float> windingStates;
    [Range(0f,1f)]
    public float defaultValue;
    [Range(0f,188.4956f)]
    public float anguarVelocity;
    [Range(0f,60f)]
    public float torque;

    public TorqueVisualizer tv;
    public EfficencyMapVisualizer emv;
    public Float32Publisher pub;
    public Float32Subscriber sub;
    public bool flux;
    public UnityEventString TorqueValue=new UnityEventString();
    public UnityEventString RPMValue=new UnityEventString();
    IEnumerator fluctuator()
    {


        float torque = tv.Torque;
        while (isActiveAndEnabled)
        {
            
            float rate = ((Mathf.PI)/(2*Mathf.Abs(anguarVelocity)))/Windings.Count;
            
            foreach (var grad in windingGradients)
            {
                if (anguarVelocity == 0)
                {
                    rate = 0.02f;
                    grad.Trasparent = true;
                
                    yield return null;
                    continue;
                }
                grad.Trasparent = true;
                tv.Torque = torque * grad.value;
                yield return new WaitForSeconds(rate);
                grad.Trasparent = false;
                ;
            }

//            print (rate);









        }
        
        
        
        
        
        
        
        
        yield break;
    }
    
    
    // [SerializeField]
    private List<GradientPainter> windingGradients;
    private AngularVelocitySetter rotator;
    private void Awake()
    {
        

    }

    void Start()
    {
        
        windingGradients = new List<GradientPainter>(3);
        // for (int i = 0; i < Windings.Count; i++)
        // {
        //     windingGradients[i] = Windings[i].GetComponent<GradientPainter>();
        // }
        foreach (var winding in Windings)
        {
            windingStates.Add(defaultValue);
            windingGradients.Add(winding.GetComponent<GradientPainter>());
        }
        rotator = Rotor.GetComponent<AngularVelocitySetter>();
        if(flux)
            StartCoroutine(fluctuator());
    }

    public void updateTorque()
    {
        torque = sub.value;
    }
    // Update is called once per frame
    void Update()
    {
        
        rotator.AngularVelocity=anguarVelocity;
        for (int i = 0; i < Windings.Count; i++)
        {
            windingGradients[i].value = windingStates[i];
        }

        //if (!flux)
            tv.Torque = torque;
        
        
        if (emv != null)
        {
            TorqueValue?.Invoke(tv.Torque.ToString("F2"));
            RPMValue?.Invoke((Mathf.Abs(anguarVelocity) * 60 / (2 * Mathf.PI)).ToString("F2"));
            emv.Torque = Mathf.Abs(tv.Torque);
            emv.RPM = Mathf.Abs(anguarVelocity) * 60 / (2 * Mathf.PI);
        }
        pub.Publish(anguarVelocity * 60 / (2 * Mathf.PI));
    }
}
