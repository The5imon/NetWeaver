using System;
using System.IO;
using System.Threading.Tasks;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public class CopyFile : ICommand
    {
        private string Path { get; }

        public CopyFile(string path)
        {
            Path = path;
        }

        public async Task Execute(ClientChannel channel)
        {
            //TODO: Only file name pathn
            await Task.Run(() => File.Copy(Path, @"\\" + channel.Client.HostName + @"\\Scripts\" + Path, true));
            await new ClientExecute("seefile").Execute(channel);
        }
    }
}