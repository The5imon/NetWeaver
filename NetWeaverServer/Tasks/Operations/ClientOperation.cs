using System;
using System.Collections.Generic;
using System.Linq;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Operations
{
    public class ClientOperation
    {
        private static string connectionTopic = "/conn";
        private static string disconnectionTopic = "/disconn";
        private MqttMaster Channel { get; }
        private DBInterface DBInterface { get; }
        private EventInterface EventInterface { get; }

        private GUI GUI { get; }
        public ClientOperation(MqttMaster communication, GUI gui, DBInterface dbInterface, EventInterface eventInterface)
        {
            GUI = gui;
            Channel = communication;
            DBInterface = dbInterface;
            EventInterface = eventInterface;

            Channel.MessageReceivedEvent += OnClientConnect;
            Channel.MessageReceivedEvent += OnClientDisconnect;
        }

        private void OnClientConnect(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.Topic.Equals(connectionTopic))
            {
                Console.WriteLine("New Client connected");
                string[] args = e.ApplicationMessage.ConvertPayloadToString().Split('&');
                //Database entry + trigger update event
                DBInterface.insertClients(new List<Client>{new Client(args[1], args[0], args[2])});
                //GUI.clients.Add(new Client(args[1], args[0], args[2]));
                EventInterface.GetUpdatedContentEvent().Invoke(this, EventArgs.Empty);
            }
        }

        private void OnClientDisconnect(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.Topic.Equals(disconnectionTopic))
            {
                //Database entry + trigger update event
                DBInterface.deleteClientByHostname(e.ApplicationMessage.ConvertPayloadToString());
                //GUI.clients.RemoveAll(x => x.HostName.Equals(e.ApplicationMessage.ConvertPayloadToString()));
                EventInterface.GetUpdatedContentEvent().Invoke(this, EventArgs.Empty);
            }
        }
    }
}