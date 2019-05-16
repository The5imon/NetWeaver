using System;
using System.IO;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Jobs
{
    public class CopyFileJob : Job
    {
        public CopyFileJob(ClientChannel channel, JobProgress progress, string file)
            : base(channel, progress, file)
        {
            Progress.SetCommandCount(1);
        }

        public override async Task Work()
        {
            string filename = Path.GetFileName(Args);
            await Task.Run(() =>
                File.Copy(Args,
                    @"\\" + Client.HostName + @"\\" + filename, true));
            //TODO: Format commands for better structure
            await Channel.PublishAsync($"{Cmd.Seefile} {filename}");
            Channel.Reply.WaitOne();
            Progress.NextCommandDone();
        }
    }
}