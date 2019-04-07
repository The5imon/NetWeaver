﻿using System;
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
                    s.ConstructUsing(client => new MqttSlave("192.168.0.171", 6666));
                    s.WhenStarted(client => Task.Run(client.StartAsync));
                    s.WhenStopped(client => Task.Run(client.StopAsync));
                });

                x.RunAsLocalSystem();
                x.SetServiceName("MCON Client Service");
                x.SetDisplayName("MCON Client Service");
                x.SetDescription("Ze service from ze team MCON.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;    
        }
    }
}