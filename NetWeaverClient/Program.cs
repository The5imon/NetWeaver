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
                    s.ConstructUsing(client => new MqttSlave("10.2.61.21", 6666));
                    s.WhenStarted(client => Task.Run(client.StartAsync));
                    s.WhenStopped(client => Task.Run(client.StopAsync));
                });

                x.RunAsLocalSystem();
                x.SetServiceName("NetWeaver Client Service");
                x.SetDisplayName("NetWeaver Client Service");
                x.SetDescription("Ze service from ze team NetWeaver.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
