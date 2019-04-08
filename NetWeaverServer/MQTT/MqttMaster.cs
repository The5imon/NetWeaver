using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace NetWeaverServer.MQTT
{
    public class MqttMaster
    {
        private readonly int _port;
        private readonly string _ip;
        private readonly IMqttClient _client;
        public event EventHandler<MqttApplicationMessageReceivedEventArgs> MessageReceivedEvent;

        public MqttMaster(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
            _client = new MqttFactory().CreateMqttClient();
        }

        public async Task StartAsync()
        {
            await ConnectAsync();
            await SubscribeAsync("/#");
            
            _client.ApplicationMessageReceived += OnMessageReceived;            
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ApplicationMessage.Topic + ": " + e.ApplicationMessage.ConvertPayloadToString());
            // ClientId ist die vom Master...?
            MessageReceivedEvent?.Invoke(this, e);
        }

        public async Task StopAsync()
        {
            await _client.DisconnectAsync();
        }
 
        private async Task ConnectAsync()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("MASTER")
                .WithCredentials("netweaver", "woswofürdaspasswort")
                .WithCleanSession().WithTcpServer(_ip, _port); 
            
            await _client.ConnectAsync(options.Build());
        }

        public async Task PublishAsync(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic).WithPayload(payload)
                .WithExactlyOnceQoS();
            
            await _client.PublishAsync(message.Build());
        }

        private async Task SubscribeAsync(string topic)
        {
            await _client.SubscribeAsync(topic);
        }
        
        public async Task UnsubscribeAsync(string topic)
        {
            await _client.UnsubscribeAsync(topic);
        }
    }
}