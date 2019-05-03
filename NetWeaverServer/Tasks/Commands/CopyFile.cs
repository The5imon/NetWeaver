using System;
using System.IO;
using System.Threading.Tasks;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public class CopyFile : ICommand
    {
        private string Filepath { get; }

        public CopyFile(string filepath)
        {
            Filepath = filepath;
        }

        public async Task Execute(ClientChannel channel)
        {
            await Task.Run(() =>
                File.Copy(Filepath,
                    @"\\" + channel.Client.HostName + @"\\Scripts\" + Path.GetFileName(Filepath), true));
            await new ClientExecute(Cmd.Seefile).Execute(channel);
        }
    }
}