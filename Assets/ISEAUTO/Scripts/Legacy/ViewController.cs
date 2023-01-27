using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[Serializable]
public struct MaterialState
{
    public Material material;
    public bool Transparent;
    [Range(0,1)]
    public float Alpha;
    [ReadOnly(true)]
    public bool oldState;
}
public class ViewController : MonoBehaviour
{
    public List<MaterialState> Materials;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i<Materials.Count;i++)
        { 
            if (Materials[i].Transparent != Materials[i].oldState)
            {       
                if (Materials[i].Transparent)
                    Materials[i].material.ToFadeMode();
                else
                {
                    print("opaque");
                    Materials[i].material.ToOpaqueMode();
                }

                Color c = Materials[i].material.color;
                c.a = Materials[i].Alpha;
                Materials[i].material.color = c;
            }

            

        }
    }
}
public static class MaterialExtensions
{
    public static void ToOpaqueMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Opaque");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
   
    public static void ToFadeMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
    }
}