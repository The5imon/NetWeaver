using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.MQTT
{
    public class ClientChannel
    {
        public Client Client { get; }
        private MqttMaster Channel { get; }

        public AutoResetEvent Reply = new AutoResetEvent(false);

        public ClientChannel(Client client, MqttMaster channel)
        {
            Client = client;
            Channel = channel;
            Channel.MessageReceivedEvent += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs args)
        {
            if (args.ApplicationMessage.Topic.Equals($"/reply/{Client.HostName}")
                && args.ApplicationMessage.ConvertPayloadToString().Equals("ACK"))
            {
                Reply.Set();
            }
        }

        public async Task PublishAsync(string message)
        {
            await Channel.PublishAsync($"/cmd/{Client.HostName}", message);
        }
    }
}