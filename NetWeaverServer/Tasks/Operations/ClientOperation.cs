using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        public ClientOperation(MqttMaster communication, DBInterface dbInterface, EventInterface eventInterface)
        {
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
                Debug.WriteLine("New Client connected");
                string[] args = e.ApplicationMessage.ConvertPayloadToString().Split('&');
                //Database entry + trigger update event
                DBInterface.insertClients(new List<Client>
                {
                    new Client(args[1], args[0], args[2], true, DateTime.Today.ToString("dd-MM-yyyy"))
                });
                //GUI.clients.Add(new Client(args[1], args[0], args[2]));
                EventInterface.GetUpdatedContentEvent().Invoke(this, EventArgs.Empty);
                Debug.WriteLine("Updated");
            }
        }

        private void OnClientDisconnect(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.Topic.Equals(disconnectionTopic))
            {
                //Database entry + trigger update event
                DBInterface.setOffline(e.ApplicationMessage.ConvertPayloadToString());
                //GUI.clients.RemoveAll(x => x.HostName.Equals(e.ApplicationMessage.ConvertPayloadToString()));
                EventInterface.GetUpdatedContentEvent().Invoke(this, EventArgs.Empty);
            }
        }
    }
}