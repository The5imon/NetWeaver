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
            await Task.Run(() => File.Copy(Path, @"\\" + channel.Client.HostName + @"\\Scripts", true));
        }
    }
}