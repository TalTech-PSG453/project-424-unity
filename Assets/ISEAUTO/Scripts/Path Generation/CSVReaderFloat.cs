using System;
using System.Collections.Generic;
using System.Linq;
using EdyCommonTools;
using UnityEngine;
using static EdyCommonTools.CsvFileReader;

namespace ISEAUTO.Scripts.Path_Generation
{
    public class CSVReaderFloat 
    {
        private TextAsset m_file;
        public CSVReaderFloat(string filename)
        {
            m_file=Resources.Load(filename) as TextAsset;
        }
        public CSVReaderFloat(TextAsset file)
        {
            m_file = file;
        }


        public float[,] GetArray()
        {
            List<string> rows = new List<string>();
            rows = m_file.text.Split('\n').ToList();
            int i, j;
            i = 0;
            bool intialized=false;
            float[,] ret=new float[1,1];
            foreach (var row in rows)
            {
                List<string> columns = row.Split(',').ToList();
                j = 0;
                foreach (var value in columns)
                {
                    if (!intialized)
                    {
                        ret = new float[rows.Count, columns.Count];
                        intialized = true;
                    }
                    
                    Debug.Log(value+"\t"+i+"\t"+j);

                    ret[i,j] = float.Parse(value);
                    
                    
                    j++;
                }
                
                i++;
            }

            









            return ret;
        }
            
        public List<List<float>>  GetList()
        {
            List<string> rows = new List<string>();
            rows = m_file.text.Split('\n').ToList();
            List<List<float>> ret=new List<List<float>>() ;
            foreach (var row in rows)
            {
                List<string> columns = row.Split(',').ToList();
                
                List<float> l = new List<float>();
                foreach (var value in columns)
                {


                    l.Add(float.Parse(value));
                    
                    
                    
                }
                ret.Add(l);
                
                
            }

            









            return ret;
        }

        
        public List<float[]>  GetArrayList()
        {
            List<string> rows = new List<string>();
            rows = m_file.text.Split('\n').ToList();
            List<float[]> ret=new List<float[]>() ;
            foreach (var row in rows)
            {
                List<string> columns = row.Split(',').ToList();
                
                float[] l = new float[columns.Count];
                int i = 0;
                foreach (var value in columns)
                {


                    l[i]=(float.Parse(value));

                    i++;

                }
                ret.Add(l);
                
                
            }

            









            return ret;
        }


        public List<float[]> GetColumns(int[] column_idx, List<float[]> list)
        {
            var ret = new List<float[]>();


            foreach (var item in list)
            {
                float[] r = new float[column_idx.Length];
                for (int i = 0; i < column_idx.Length; i++)
                {
                    r[i] = item[column_idx[i]];
                }
                
                ret.Add(r);


            }

            return ret;

        }

    }
}