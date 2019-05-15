using System.Threading.Tasks;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Jobs
{

    public class Cmd
    {
        public const string Exescript = "exescript";
        public const string Openshare = "openshare";
        public const string Closeshare = "closeshare";
        public const string Seefile = "seefile";
        public const string Copy = "copy";
    }

    public class ClientJob : Job
    {

        public ClientJob(ClientChannel channel, JobProgress progress, string args)
            : base(channel, progress, args)
        {
            Progress.SetCommandCount(1);
        }

        public  override async Task Work()
        {
            await Channel.PublishAsync(Args);
            Channel.Reply.WaitOne();
            Progress.NextCommandDone();
        }
    }
}