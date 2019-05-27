using System;
using System.IO;
using System.Threading.Tasks;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public class CopyExecute : ICommand
    {
        public static string SCRIPTS = @"Scripts\";
        public static string INSTALL = @"Install\";
        private string Filepath { get; }
        private string Destination { get; }

        public CopyExecute(string filepath, string destination)
        {
            Filepath = filepath;
            Destination = destination;
        }

        public async Task Execute(ClientChannel channel)
        {
            string filename = Path.GetFileName(Filepath);
            await Task.Run(() =>
                File.Copy(Filepath,
                    @"\\" + channel.Client.HostName + @"\\" + Destination + filename, true));
            //TODO: Format commands for better structure
            await new ClientExecute($"{Cmd.Seefile} {filename}").Execute(channel);
        }
    }
}