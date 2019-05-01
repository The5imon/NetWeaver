using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NetWeaverClient.MQTT;
using MQTTnet;
using MQTTnet.Client;
using PcapDotNet.Core;

namespace NetWeaverClient.MQTT
{
    internal class MqttSlave
    {
        private readonly int _port;
        private readonly string _ipaddress;
        private readonly IMqttClient _client;
        private readonly ClientInformation intInfo = new ClientInformation();
        public MqttSlave(string ipaddress, int port)
        {
            this._ipaddress = ipaddress;
            this._port = port;
            _client = new MqttFactory().CreateMqttClient();
        }

        public async Task StartAsync()
        {
            //DeviceDiscovery extInfo = new DeviceDiscovery();

            await ConnectAsync();
            await SubscribeAsync("/cmd/" + intInfo.Name);
            await PublishAsync("/conn", intInfo.Info);

            //await PublishAsync("/conn", ExtInfo._adapter);

            _client.ApplicationMessageReceived += OnMessageReceived;

            while (true)
            {
                string c = Console.ReadLine();
                await _client.PublishAsync("/reply/" + intInfo.Name, c);
            }
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ClientId + ": " + e.ApplicationMessage.ConvertPayloadToString());
        }

        public async Task StopAsync()
        {
            await _client.DisconnectAsync();
        }

        private async Task ConnectAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/disconn").WithPayload(intInfo.Name)
                .WithExactlyOnceQoS();


            var options = new MqttClientOptionsBuilder()
                .WithClientId(intInfo.Name).WithWillMessage(message.Build())
                .WithCredentials("netweaver", "woswofürdaspasswort")
                .WithCleanSession().WithTcpServer(_ipaddress, _port);


            await _client.ConnectAsync(options.Build());
        }

        private async Task PublishAsync(string topic, string payload)
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
    }
}
