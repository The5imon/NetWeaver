using System;
using System.Threading.Tasks;
using NetWeaverClient.MQTT;
using Topshelf;

namespace NetWeaverClient
{
    class Program
    {
        private static void Main()
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<MqttSlave>(s =>
                {
                    s.ConstructUsing(client => new MqttSlave("127.0.0.1", 6666));
                    s.WhenStarted(client => Task.Run(client.StartAsync));
                    s.WhenStopped(client => Task.Run(client.StopAsync));
                });

                x.RunAsLocalSystem();
                x.SetServiceName("NetWeaver");
                x.SetDisplayName("NetWeaver");
                x.SetDescription("Clientseitige Anwendung des Netweavers.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
