using System;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Operations
{
    public class ClientOperation
    {
        private static string connectionTopic = "/conn";
        private MqttMaster Channel { get; }

        private GUI GUI { get; }
        public ClientOperation(MqttMaster communication, GUI gui)
        {
            GUI = gui;
            Channel = communication;
            Channel.MessageReceivedEvent += OnClientConnect;
        }

        private void OnClientConnect(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.Topic.Equals(connectionTopic))
            {
                Console.WriteLine("New Client connected");
                string[] args = e.ApplicationMessage.ConvertPayloadToString().Split('&');
                //Database entry + trigger update event
                GUI.clients.Add(new Client(args[1], args[0], args[2]));
            }
        }
    }
}