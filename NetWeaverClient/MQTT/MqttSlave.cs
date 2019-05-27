using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using PcapDotNet.Base;

namespace NetWeaverClient.MQTT
{
    internal class MqttSlave
    {
        private readonly int _port;
        private readonly string _ipaddress;
        private readonly IMqttClient _client;
        private readonly ClientInformation clientInformation = new ClientInformation();
        public MqttSlave(string ipaddress, int port)
        {
            this._ipaddress = ipaddress;
            this._port = port;
            _client = new MqttFactory().CreateMqttClient();
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            int exitCode = 0;
            string exitMsg = string.Empty;
            string file = e.ApplicationMessage.ConvertPayloadToString().Split( )[1];

            switch (e.ApplicationMessage.ConvertPayloadToString())
            {
                case "openshare":
                    exitCode = Commands.OpenNetShare(); break;
                case "seefile":
                    exitCode = Commands.SeeFile(file); break;
                case "closeshare":
                    exitCode = Commands.CloseNetShare(); break;
                case "execscript":
                    exitMsg = Commands.RunPowershellScript(file);
                    exitCode = 2; break;
            }
            HandleExitCode(exitCode, exitMsg);
        }
        
        private void HandleExitCode(int exitCode, string exitMsg)
        {
            if (!exitMsg.IsNullOrEmpty())
            {
                Task.Run(() => PublishAsync("/log/" + clientInformation.Name, exitMsg));
            }
            else
            {
                switch (exitCode)
                {
                    case 0:
                        Task.Run(() => PublishAsync("/reply/" + clientInformation.Name, "ACK"));
                        break;
                    case -1:
                        Task.Run(() => PublishAsync("/reply/" + clientInformation.Name, "NACK"));
                        break;
                }
            }
        }

        private async Task ConnectAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/disconn").WithPayload(clientInformation.Name)
                .WithExactlyOnceQoS();


            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientInformation.Name).WithWillMessage(message.Build())
                .WithCredentials("netweaver", "woswofürdaspasswort")
                .WithCleanSession().WithTcpServer(_ipaddress, _port);

            await _client.ConnectAsync(options.Build());
        }
        
        public async Task StartAsync()
        {
            Commands.RunPowershellScript(@"C:\Users\Gregor Brunner\OneDrive\Desktop\test.ps1");
            while (!_client.IsConnected)
            {
                await ConnectAsync();
                Thread.Sleep(5000);
            }
            
            Console.WriteLine("Connected: " + _client.IsConnected);
            await SubscribeAsync("/cmd/" + clientInformation.Name);
            await SubscribeAsync("/log/" + clientInformation.Name);
            
            await PublishAsync("/conn", clientInformation.Info);
            _client.ApplicationMessageReceived += OnMessageReceived;
        }
        
        public async Task StopAsync()
        {
            await _client.DisconnectAsync();
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
