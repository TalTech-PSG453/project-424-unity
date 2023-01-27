using System.ComponentModel;
using EE.TalTech.IVAR.DigitalizationProject.ROS;
using EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs;
using UnityEngine;

namespace Andriy.ROS.Publishers
{
    public class Float32Publisher:Publisher<Float32>
    {


        private float time = 0;

        private void Start() {
            //Advertise();
        }
        [SerializeField,ReadOnly(true)]
        private float value;
        public void Publish()
        {
            
            
            Float32 data = new Float32
            {
                data = value
            };
            Publish(data);
        }

        public void Publish(float f)
        {
            value = f;
            Publish();
        }
        
    }
}