using System;
using System.Collections;
using System.Collections.Generic;
using DigitalTwin.Utils.Events;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct MapRGB
{
    public float[,] r;
    public float[,] g;
    public float[,] b;
    public float[,] intensity;

}
public class EfficencyMapVisualizer : MonoBehaviour
{
    public RectTransform pointer;
    
    [HideInInspector]
    public Texture2D texture;

    public TextAsset Red;
    public TextAsset Blue;
    public TextAsset Green;
    public TextAsset Efficeincy;
    public MapRGB rgb;
    public float[,] eff;
    public int MaxRPM=1800;
    public float MaxTorque=60f;
    public float MaxTorqueRatio = 10.0f;
    private int MaxTorqueRecalc;
    public float Torque;
    public float RPM;
    private RectTransform trans;

    public InstanceMaterial motorAxle;

    public UnityEventString EfficiencyOut=new UnityEventString(); 
    // Start is called before the first frame update
    void Start()
    {
        
        trans = GetComponent<RectTransform>();
        MaxTorqueRecalc = (int) (MaxTorque * MaxTorqueRatio);

        draw_map_image();
        eff = readFloatArray(Efficeincy);

    }

    // Update is called once per frame
    void Update()
    {
        float recalc_w = Mathf.Clamp(trans.rect.width * RPM / MaxRPM, 0, trans.rect.width);
        float recalc_h = Mathf.Clamp(trans.rect.height * (Torque * 10) / MaxTorqueRecalc, 0, trans.rect.height);
        pointer.localPosition = new Vector3((int)recalc_w, (int)recalc_h);

        float tempeff = rgb.r[(int)recalc_w,(int)recalc_h] *
                        (1 - rgb.g[(int)recalc_w,(int)recalc_h]) *
                        (1 - rgb.b[(int)recalc_w,(int)recalc_h]);
       
        EfficiencyOut?.Invoke(eff[(int)recalc_w,(int)recalc_h].ToString("F2"));
        motorAxle.MaterialColor = texture.GetPixel((int) RPM, (int) (Torque * MaxTorqueRatio));
    }

    float[,] readFloatArray(TextAsset item)
    {
        Vector2 size;
        string text = item.text;
        string[] lines = text.Split(System.Environment.NewLine[0]);
        
        size.y = lines.Length;
        string[] _count_coord = lines[0].Split(';');
        size.x = _count_coord.Length;
        print(_count_coord.Length);
        float[,] output = new float[(int) size.x, (int) size.y];
        string temp="";
        Vector2 tv = new Vector2();
        for (int i=0; i < MaxTorqueRecalc;i++)
        {
            
            string[] coords = lines[i].Split(';');
            
            for (int j = 0; j < MaxRPM; j++)
            {


            
                // print(i+"  "+ j);
                output[j,MaxTorqueRecalc-1-i] = float.Parse(coords[j]);
                
            }
        }
        print(output[7,200]);
        
        
        return output;
    }
    float[,] readArray(TextAsset item)
    {
        Vector2 size;
        string text = item.text;
        string[] lines = text.Split(System.Environment.NewLine[0]);
        
        size.y = lines.Length;
        string[] _count_coord = lines[0].Split(';');
        size.x = _count_coord.Length;
        print(_count_coord.Length);
        float[,] output = new float[(int) size.x, (int) size.y];
        string temp="";
        Vector2 tv = new Vector2();
        for (int i=0; i < MaxTorqueRecalc;i++)
        {
            
            string[] coords = lines[i].Split(';');
            
            for (int j = 0; j < MaxRPM; j++)
            {


            
                // print(i+"  "+ j);
                output[j,MaxTorqueRecalc-1-i] = (float)int.Parse(coords[j])/255.0f;
                
            }
        }
        print(output[7,200]);
        
        
        return output;
    }

    void draw_map_image()
    {
        print("Hello");
        rgb.r=readArray(Red);
        rgb.g=readArray(Green);
        rgb.b=readArray(Blue);
        
        texture = new Texture2D(rgb.r.GetLength(0),rgb.r.GetLength(1));
        print(new Vector2(rgb.r.GetLength(0),rgb.r.GetLength(1)));
        
        
        for (int i=0; i<rgb.r.GetLength(0);i++)
        {
            
            
            for (int j = rgb.r.GetLength(1)-1; j >=0 ; j--)
            {
        
                texture.SetPixel(i,j,new Color(rgb.r[i,j],rgb.g[i,j],rgb.b[i,j]));
        
            }
        }
        texture.Apply();
        GetComponent<Image>().sprite = Sprite.Create(texture,new Rect(0,0,rgb.r.GetLength(0),rgb.r.GetLength(1)),gameObject.GetComponent<RectTransform>().pivot);



    }
    
    
    
    
    
}
