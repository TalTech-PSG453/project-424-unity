using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class InstanceMaterial : MonoBehaviour
{
    private Material clear;

    private Material opaque;
    private MeshRenderer mr;
    public bool Transparent;
    public Color MaterialColor;
    
    private void Awake()
    {

        mr = GetComponent <MeshRenderer>();
        opaque=Instantiate(Resources.Load("Materials/DefaultOpaque") as Material);
        clear=Instantiate(Resources.Load("Materials/DefaultTransparent") as Material);
        SetMaterial(mr, Transparent ? clear : opaque);
        UpdateColor();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    private bool transp_old;
    private Color old_Color;
    void Update()
    {
        if (old_Color != MaterialColor)
        {
            UpdateColor();
            old_Color = MaterialColor;
        }
        if (transp_old != Transparent)
        {
            SetMaterial(mr, Transparent ? clear : opaque);
        }

        transp_old = Transparent;
    }
    void SetMaterial(MeshRenderer mesh,Material material)
    {
        mesh.material = material;

    }

    void UpdateColor()
    {
        opaque.color = MaterialColor;
        clear.color = MaterialColor;
    }
}
