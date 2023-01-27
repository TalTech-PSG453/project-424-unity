using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngluarVelocityVisualizer : MonoBehaviour
{
    public Rigidbody TrackedObject;
    public int axis;
    public List<GradientPainter> color;
    public Transform Curve;

    public Transform Cone;
    
    
    public float AngularVelocity;
    public float MaximumAngularVelocity;
    
    public Vector3 Scale;
    
    public float MinimumScalePercent;
    public float MaximumScalePercent;

    public bool Invert;
    
    private Vector3 startScale;
    private Vector3 coneOffset;
    private float forcePercent;

    private float scalePercent;
    

    private float scaleRange;

    private Vector3 m_scale;
    // Start is called before the first frame update
    void Start()
    {
        startScale = Curve.localScale;
        coneOffset = Cone.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (TrackedObject != null)
        {
            Vector3 localangularvelocity =
                TrackedObject.transform.InverseTransformDirection(TrackedObject.angularVelocity).normalized
                * TrackedObject.angularVelocity.magnitude;
            AngularVelocity = localangularvelocity[axis];
        }
        AngularVelocity=Mathf.Clamp(AngularVelocity, -MaximumAngularVelocity, MaximumAngularVelocity);
        forcePercent = AngularVelocity / (Mathf.Abs(MaximumAngularVelocity));
        scalePercent = forcePercent * (Invert ? -1f : 1f);
        scaleRange = MaximumScalePercent - MinimumScalePercent;
        scalePercent = MinimumScalePercent*Mathf.Sign(scalePercent)+scalePercent * scaleRange;
        
        m_scale = startScale * scalePercent;
        Curve.localScale = m_scale;
        Cone.localPosition = coneOffset * scalePercent;
        Cone.localScale = Vector3.one*Mathf.Sign(scalePercent);
        foreach (var c in color)
        {
            if(c!=null)
                c.value = Mathf.Abs(forcePercent);
        }
        





    }
}
