using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueVisualizer : MonoBehaviour
{
    //public Rigidbody TrackedObject;
    public int axis;
    public List<GradientPainter> color;
    public List<Transform> Curve;

    public Transform Cone;
    
    
    public float Torque;
    public float MaximumTorque;
    
    public Vector3 Scale;
    
    public float MinimumScalePercent;
    public float MaximumScalePercent;

    public bool Invert;
    
    private List<Vector3> startScale;
    private Vector3 coneOffset;
    private float forcePercent;

    private float scalePercent;
    

    private float scaleRange;

    private List<Vector3> m_scale;
    // Start is called before the first frame update
    void Start()
    {
        startScale = new List<Vector3>(Curve.Count);
        m_scale = new List<Vector3>(Curve.Count);
        
        
        for (int i = 0; i<Curve.Count;i++)
        {
            startScale.Add(Curve[i].localScale);
            m_scale.Add(Curve[i].localScale);
        }
        coneOffset = Cone.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // if (TrackedObject != null)
        // {
        //     Vector3 localangularvelocity =
        //         TrackedObject.transform.InverseTransformDirection(TrackedObject.angularVelocity).normalized
        //         * TrackedObject.angularVelocity.magnitude;
        //     Torque = localangularvelocity[axis];
        // }
        Torque=Mathf.Clamp(Torque, -MaximumTorque, MaximumTorque);
        forcePercent = Torque / (Mathf.Abs(MaximumTorque));
        scalePercent = forcePercent * (Invert ? -1f : 1f);
        scaleRange = MaximumScalePercent - MinimumScalePercent;
        scalePercent = MinimumScalePercent*Mathf.Sign(scalePercent)+scalePercent * scaleRange;
        
//        print(startScale.Count);
        for (int i = 0; i<Curve.Count;i++)
        {
            
            m_scale[i] = 
                startScale[i] * scalePercent;
            Curve[i].localScale = m_scale[i];
        }
        
        
        
        
        Cone.localPosition = coneOffset * scalePercent;
        Cone.localScale = Vector3.one*Mathf.Sign(scalePercent);
        foreach (var c in color)
        {
            if(c!=null)
                c.value = Mathf.Abs(forcePercent);
        }
        





    }
}
