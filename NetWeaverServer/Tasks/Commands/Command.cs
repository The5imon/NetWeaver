using System;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Commands
{
    //TODO: Use Commands to actually do things --> Use Commands on Client to actually do things!!!
    public interface ICommand
    {
        Task Execute();
    }
}