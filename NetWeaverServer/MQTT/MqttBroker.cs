using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Protocol;

namespace NetWeaverServer.MQTT
{
    public class MqttBroker
    {
        private readonly int _port;
        private readonly IMqttServer _server;

        public MqttBroker(int port)
        {
            this._port = port;
            _server = new MqttFactory().CreateMqttServer();
            _server.ClientDisconnected += OnClientDisconnect;
        }

        private async void OnClientDisconnect(object sender, MqttClientDisconnectedEventArgs e)
        {
            await _server.PublishAsync(e.ClientId);
        }

        public async Task StartAsync()
        {
            var options = new MqttServerOptionsBuilder().WithDefaultEndpointPort(_port)
                .WithConnectionValidator(c =>
            {
                if (c.Username != "netweaver" || c.Password != "woswofürdaspasswort")
                {
                    c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                    return;
                }

                c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
            });

            await _server.StartAsync(options.Build());
        }



    }
}
