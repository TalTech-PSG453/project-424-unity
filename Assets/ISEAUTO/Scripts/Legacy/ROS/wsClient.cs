using System;  
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

public class wsClient : MonoBehaviour
{
    public enum advertiseTopicForROS {
        Yes,
        No
    }

    public advertiseTopicForROS advertiseTorqueTopic;
    public advertiseTopicForROS advertiseAngularVelocityTopic;

    public class wsMessageObj{
       public String op;
       public String topic;
    }

    public class wsAdvertisementMessageObj{
        public String op;
        public String topic;
        public String type;
    }

    public class wsPublishTorqueMessageObj{
        public string op;
        public string topic;
        public TorqueMsg msg;
            
    }
    [Serializable]
    public class TorqueMsg {
        public Header header;
        public float left_torque;
        public float right_torque;
    }
    public class wsPublishAngularVelocityMessageObj{
        public string op;
        public string topic;
        public AngularVelocityMsg msg;
            
    }
    [Serializable]
    public class AngularVelocityMsg {
        public Header header;
        public float right_velocity;
        public float left_velocity;
    }    

    

    [Serializable]
    public class Header {
        public Stamp stamp;
        public string frame_id; 
    }

    [Serializable]
    public class Stamp{

    }

    public string ServerIP;
    public string ServerPort = "9090";
    public string SubscriptionTopic = "/tb_lm/supply_input";
    public string torqueAdvertisedMessageType = "digital_twin_msgs/Torque";
    public string AngularVelocityAdvertisedMessageType = "digital_twin_msgs/AngularVelocity";
    public string torqueTopic = "/tb_lm/feedback_torque";
    public string angularVelocityTopic = "/tb_lm/feedback_angular_velocity";

    WebSocket ws;
    
    private void openWsConnection(string ip, string port){
        ws = new WebSocket("ws://" +ip+ ":" +port);
        ws.Connect();
    }

    private void unsubscribeFromTopic(string topic){
        wsMessageObj unsubscribtionMessage = new wsMessageObj();
        unsubscribtionMessage.op = "unsubscribe";
        unsubscribtionMessage.topic = topic;
        sendJsonMessage(unsubscribtionMessage);
    }

    private void advertiseTopic(string topic, string messageType){
        wsAdvertisementMessageObj advertisedTopicMessage = new wsAdvertisementMessageObj();
        advertisedTopicMessage.op = "advertise";
        advertisedTopicMessage.topic = topic;
       advertisedTopicMessage.type = messageType;
        sendJsonMessage(advertisedTopicMessage);

    }

    private void unadvertiseTopic(string topic){
        wsMessageObj unAdvertisedTopicMessage = new wsMessageObj();
        unAdvertisedTopicMessage.op = "unadvertise";
        unAdvertisedTopicMessage.topic = topic;
        sendJsonMessage(unAdvertisedTopicMessage);

    }

    private void publishTorqueMsg(float torqueL, float torqueR){
        wsPublishTorqueMessageObj messageToPublish = new wsPublishTorqueMessageObj(){
            op = "publish",
            topic = torqueTopic,
            msg = new TorqueMsg(){
                header = new Header(),
                left_torque = torqueL,
                right_torque = torqueR
            }
        }; 

        sendJsonMessage(messageToPublish);
    }

    private void publishAngularVelocityMsg(float angVelL, float angVelR){
        wsPublishAngularVelocityMessageObj messageToPublish = new wsPublishAngularVelocityMessageObj(){
            op = "publish",
            topic = angularVelocityTopic,
            msg = new AngularVelocityMsg(){
                header = new Header(),
                left_velocity = angVelL,
                right_velocity = angVelR
            }
        };

        sendJsonMessage(messageToPublish);
    }

    private void closeWsConnection(){
        ws.Close();
    }

    private void OnDisable(){
        unsubscribeFromTopic(SubscriptionTopic);
        
        if(advertiseTorqueTopic == advertiseTopicForROS.Yes){
            unadvertiseTopic(torqueTopic);
        }
        
        if(advertiseAngularVelocityTopic == advertiseTopicForROS.Yes){
            unadvertiseTopic(angularVelocityTopic);
        }

        closeWsConnection();
    }

    private void subscribeToTopic(string topic){
        wsMessageObj subscriptionMessage = new wsMessageObj();
        subscriptionMessage.op = "subscribe";
        subscriptionMessage.topic = topic;
        sendJsonMessage(subscriptionMessage);
    }

    private void sendJsonMessage(object message){
        ws.Send(JsonUtility.ToJson(message));
    }

    public static ReceivedInfo deserializeVoltageAndCurrentJSON(string dataString){
        return JsonUtility.FromJson<ReceivedInfo>(dataString);
    }    


    [Serializable]
    public class Voltages{
        public float voltage1;
        public float voltage2;
        public float voltage3;
    }

    [Serializable]
    public class Currents{
        public float current1;
        public float current2;
        public float current3;
    }
    
    [Serializable]
    public class VoltageAndCurrentMsg{
        public Header header;
        public Voltages voltages;
        public Currents currents;
    }

    [Serializable]
    public class ReceivedInfo{
        public string op;
        public string topic;
        public VoltageAndCurrentMsg msg;
    }


    private void Start()
    {
        openWsConnection(ServerIP, ServerPort);

        if(advertiseTorqueTopic == advertiseTopicForROS.Yes){
            advertiseTopic(torqueTopic, torqueAdvertisedMessageType);
        }
        
        if(advertiseAngularVelocityTopic == advertiseTopicForROS.Yes){
            advertiseTopic(angularVelocityTopic, AngularVelocityAdvertisedMessageType);
        }
        
    }

    private void Update(){
        ws.OnMessage += (sender, e) =>{
            Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
            ReceivedInfo receivedInfo = new ReceivedInfo();
            receivedInfo = deserializeVoltageAndCurrentJSON(e.Data);
            Debug.Log(receivedInfo.msg.voltages.voltage1);
        };

        if(advertiseTorqueTopic == advertiseTopicForROS.Yes){
            publishTorqueMsg(222.022f, 666.122f); 
        }
        
        if(advertiseAngularVelocityTopic == advertiseTopicForROS.Yes){
            publishAngularVelocityMsg(123.032f, 432.123f);
        }

        
        if(ws == null){
            return;
        }

        
        if (Input.GetKeyDown(KeyCode.Space)){
        subscribeToTopic(SubscriptionTopic); // Subscribes to /tb_lm/supply_input topic
        }      
    }
}
