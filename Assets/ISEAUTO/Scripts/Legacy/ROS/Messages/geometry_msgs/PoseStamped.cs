using EE.TalTech.IVAR.DigitalizationProject.ROS.std_msgs;

namespace EE.TalTech.IVAR.DigitalizationProject.ROS.geometry_msgs {

    [System.Serializable]
    public class PoseStamped : Message {
        public override string GetMessageType() => "geometry_msgs/PoseStamped";
        public Header header;
        public Pose pose;
    }
}