using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ForceVectorVisualizer : MonoBehaviour
{
    public float Torque;
    public float MaximumTorque;
    
    public Vector3 Scale=>transform.localScale;
    
    public float MinimumScalePercent;
    public float MaximumScalePercent;
    public bool OnlyLength;

    public bool Invert;
    public Gradient ArrowColor;
    public bool Transparent;
    private MeshRenderer[] m_meshRenderer;
    private Material opaque;

    public void transparencyToggle()
    {
        Transparent = !Transparent;
    }
    private Material Seethrough;
    private Vector3 startScale;
    void Start()
    {
        startScale = transform.localScale;
        
        m_meshRenderer = GetComponentsInChildren<MeshRenderer>();
        
        opaque=Instantiate(Resources.Load("Materials/ForceVector") as Material);
        Seethrough=Instantiate(Resources.Load("Materials/ForceVectorTransparent") as Material);
        SetMaterial(m_meshRenderer, Transparent ? Seethrough : opaque);
    }

    void SetMaterial(MeshRenderer[] meshes,Material material)
    {
        foreach (var mesh in meshes)
        {
            mesh.material = material;
        }
    }
    private float forcePercent;

    private float scalePercent;

    private float scaleRange;

    private Vector3 m_scale;
    private Color m_color;
    private bool transp_old;
    
    // Update is called once per frame
    void Update()
    {
        Torque=Mathf.Clamp(Torque, -MaximumTorque, MaximumTorque);
        forcePercent = Torque / (Mathf.Abs(MaximumTorque));
        scalePercent = forcePercent * (Invert ? -1f : 1f);
        scaleRange = MaximumScalePercent - MinimumScalePercent;
        scalePercent = MinimumScalePercent*Mathf.Sign(scalePercent)+scalePercent * scaleRange;

        if (OnlyLength)
        {
            m_scale.x = startScale.x;
            m_scale.y = startScale.y * scalePercent;
            m_scale.z = startScale.z;
        }
        else
        {
            m_scale = startScale * scalePercent;
        }

        m_color = ArrowColor.Evaluate(Mathf.Abs(forcePercent));
        transform.localScale = m_scale;
        foreach (var mesh in m_meshRenderer)
        {
            m_color.a = 0.5f;
            mesh.material.color = m_color;
            
        }

        if (transp_old != Transparent)
        {
            SetMaterial(m_meshRenderer, Transparent ? Seethrough : opaque);
        }

        transp_old = Transparent;

    }
}
