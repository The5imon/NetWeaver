using System;
using System.Threading.Tasks;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public class ClientExecute : ICommand
    //TODO: Enum for all the ClientCommands
    {
        public string Command { get; }

        public ClientExecute(string command)
        {
            Command = command;
        }
        
        public async Task Execute(ClientChannel channel)
        {
            await channel.PublishAsync(Command);
        }

        public override string ToString()
        {
            return Command;
        }
    }
}