using EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs;

namespace EE.TalTech.IVAR.DigitalizationProject.ROS
{
    public class Float32Subscriber:Subscriber<Float32>
    {
        public float value;
        public override void MessageReceived(Float32 message)
        {
            value = message.data;
        }
    }
}