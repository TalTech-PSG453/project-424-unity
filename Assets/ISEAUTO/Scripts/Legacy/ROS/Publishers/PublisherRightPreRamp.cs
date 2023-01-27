using System;
using EE.TalTech.IVAR.DigitalizationProject.ROS;
using EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs;
using UnityEngine;

namespace DefaultNamespace
{
    public class PublisherRightPreRamp : Publisher<Float32>
    {
        public float value;
        private void Start()
        {
            Advertise();
        }

        public void Loop(float v)
        {
            var msg = new Float32
            {
                data = v,
            };
            Publish(msg);
        }
    }
}