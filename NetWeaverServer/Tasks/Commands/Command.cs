using System;
using System.Threading;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    //TODO: Use Commands to actually do things --> Use Commands on Client to actually do things!!!
    public class Command
    {
        private MqttMaster Channel { get; }

        private AutoResetEvent Reply = new AutoResetEvent(false);

        public Command(EventHandler<ProgressDetails> message)
        {

        }
    }
}