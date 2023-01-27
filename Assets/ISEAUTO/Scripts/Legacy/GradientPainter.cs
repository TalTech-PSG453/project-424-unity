using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InstanceMaterial))]
public class GradientPainter : MonoBehaviour
{
    [Range(0f,1f)]
    public float value;
    public Gradient Gradient;
    [Range(0f,1f)]
    public float Alpha;
    public bool Trasparent;
    private InstanceMaterial material;
    
    private void Awake()
    {
        material = GetComponent<InstanceMaterial>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.MaterialColor = Gradient.Evaluate(value);
        material.Transparent = Trasparent;
        material.MaterialColor.a = Alpha;
    }
}
