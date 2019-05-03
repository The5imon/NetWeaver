using System;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    public interface ICommand
    {
        Task Execute(ClientChannel channel);
    }

    public class Cmd
    {
        public const string Exescript = "exescript";
        public const string Openshare = "openshare";
        public const string Closeshare = "closeshare";
        public const string Seefile = "seefile";
    }
}