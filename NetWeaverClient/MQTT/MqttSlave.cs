using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

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
                    exitCode = Commands.RunPowershellScript(file); break;
                default:
                    break;
            }
            HandleExitCode(exitCode);
        }
        
        private void HandleExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 0:
                    Task.Run(() => PublishAsync("/reply/" + clientInformation.Name, "ACK")); break;
                case -1:
                    Task.Run(() => PublishAsync("/reply/" + clientInformation.Name, "NACK")); break;
                default:
                    break;
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
            while (!_client.IsConnected)
            {
                await ConnectAsync();
                Thread.Sleep(5000);
            }
            
            Console.WriteLine("Connected: " + _client.IsConnected);
            await SubscribeAsync("/cmd/"+clientInformation.Name);
            await PublishAsync("/conn", clientInformation.Info);

            _client.ApplicationMessageReceived += OnMessageReceived;

            Console.Read();
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
