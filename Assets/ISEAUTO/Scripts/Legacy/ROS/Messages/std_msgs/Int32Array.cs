namespace EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs
{
    [System.Serializable]
    public class Int32Array : Message
    {
        public override string GetMessageType() => "std_msgs/Int32[]";

        public int[] data;
    }
}