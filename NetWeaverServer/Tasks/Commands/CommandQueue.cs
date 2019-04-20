using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public class CommandQueue
    {
        public ICommand[] Commands { get; }

        private AutoResetEvent Reply = new AutoResetEvent(false);
        private MqttMaster Channel { get; }

        public CommandQueue(MqttMaster channel, params ICommand[] commands)
        {
            Channel = channel;
            Commands = commands;

            Channel.MessageReceivedEvent += AwaitReply;
        }

        public async Task Run()
        {
            foreach (ICommand cmd in Commands)
            {
                await cmd.Execute();
                Reply.WaitOne();
            }
        }

        private void AwaitReply(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Reply.Set();
        }
    }
}