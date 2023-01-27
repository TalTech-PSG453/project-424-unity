using System;
using EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs;
using Int32 = EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs.Int32;

namespace EE.TalTech.IVAR.DigitalizationProject.ROS.sensor_msgs
{
    [Serializable]
    public class Joy : Message
    {
        public override string GetMessageType() => "sensor_msgs/Joy";
        public Header header;
        public float[] axes;
        public int[] buttons;

    }
}