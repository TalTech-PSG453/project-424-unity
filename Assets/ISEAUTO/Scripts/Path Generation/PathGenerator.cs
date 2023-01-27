using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;

namespace ISEAUTO.Scripts.Path_Generation
{
    public class PathGenerator : SerializedMonoBehaviour
    {

        public TextAsset pathData;

        [NonSerialized]
        public CSVReaderFloat reader;
        public List<float[]> pathDataRaw=new List<float[]>();
        public List<Vector2> path2D = new List<Vector2>();
        public Transform m_parent;
        public GameObject QuadPrefab;
        public float width;
        public int precision;

        public List<GameObject> Path = new List<GameObject>();
        private void Start()
        {
            
            
            
            
        }

        public GameObject CreateTile(string name,float width, float length, Vector2 center,float angle,Transform parent)
        {
            GameObject g;
            if(QuadPrefab==null)
                g = GameObject.CreatePrimitive(PrimitiveType.Quad);
            else
            {
                g = Instantiate(QuadPrefab);
            }
            g.transform.position = new Vector3(0,center.y,center.x);
            g.transform.localScale = new Vector3(width,length,1);
            g.transform.rotation = Quaternion.Euler(new Vector3(angle,0f,0f));
            if(parent!=null)
                g.transform.SetParent(parent);
            else
            {
                g.transform.SetParent(this.transform);
            }

            g.name = name;
            return g;
        }

        public GameObject CreateTile(string name, float width, Vector2 p1, Vector2 p2,Transform parent)
        {

            // GameObject[] p = {GameObject.CreatePrimitive(PrimitiveType.Sphere),GameObject.CreatePrimitive(PrimitiveType.Sphere), };
            // p[0].transform.position=new Vector3(0,p1.y,p1.x);
            // p[1].transform.position=new Vector3(0,p2.y,p2.x);
            
            
            Vector2 center = (p1 + p2) / 2;

            float length = Vector2.Distance(p2, p1);

            float angle = Mathf.Rad2Deg*Mathf.Atan2( p2.x - p1.x,p2.y - p1.y);
            
            return CreateTile(name,width,length,center,angle,m_parent);
            








        }

        public List<GameObject> CreateTilePath(List<Vector2> pathData, Transform parent,float width=3,int entrystep = 1)
        {
            List<GameObject> gl = new List<GameObject>();
            Vector2 offset = pathData[0];
            Transform par=parent;
            for (int i = 0; i < pathData.Count-entrystep; i+=entrystep)
            {
                

                GameObject g = CreateTile(i + "", width, pathData[i]-offset, pathData[i + entrystep]-offset, par);
                par.gameObject.GetComponent<Transform>();
                gl.Add(g);
                


            }

            return gl;

        }

        List<Vector2> List2Array(List<float[]> list)
        {
            var ret = new List<Vector2>();
            foreach (var item in list)
            {
                ret.Add(new Vector2(item[0],item[1]));
            }






            return ret;
        }


        public void GetPath()
        {
            reader = new CSVReaderFloat(pathData);
            pathDataRaw = reader.GetArrayList();
            path2D = List2Array(reader.GetColumns(new int[] {2, 1}, pathDataRaw));
        }

        public void ClearPath(List<GameObject> list)
        {
            foreach (var go in list)
            {
                DestroyImmediate(go);
            }
            list.Clear();
        }



    }
    [CustomEditor(typeof(PathGenerator))]
    class PathGeneratorEditor : OdinEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            PathGenerator p = (PathGenerator) target;
            if(GUILayout.Button("Read"))
            {
                p.GetPath();
                

            }if(GUILayout.Button("Test Tile"))
            {
                p.Path=p.CreateTilePath(p.path2D,p.m_parent,p.width,p.precision);

            }
            if(GUILayout.Button("Clear Path"))
            {
                p.ClearPath(p.Path);

            }
        }
    }
    
}