/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Commands;

namespace NetWeaverServer.Tasks.Jobs
{
    public class DistributeFileJob : Job
    {
        private ClientChannel Peer { get; }

        public DistributeFileJob(ClientChannel channel, JobProgress progress, ClientChannel peer, string file)
            : base(channel, progress, file)
        {
            Peer = peer;

            Progress.SetCommandCount(4);
            Commands.AddRange(new ICommand[]
            {
                new ClientExecute(Cmd.Openshare), //To Peer
                new ClientExecute(Cmd.Copy + $" {peer.Client.HostName}\\\\Install\\" + Path.GetFileName(file)), //To Client
                new ClientExecute(Cmd.Seefile + $" {Path.GetFileName(file)}"), // To Peer
                new ClientExecute(Cmd.Closeshare), //To Peer
            });
        }

        public override async Task Work()
        {
            await new ClientExecute(Cmd.Openshare).Execute(Peer);
            Peer.Reply.WaitOne();
            Progress.NextCommandDone();

            await new ClientExecute(Cmd.Copy + $" {Peer.Client.HostName}\\\\Install\\" + Path.GetFileName(Args))
                .Execute(Channel);
            Channel.Reply.WaitOne();
            Progress.NextCommandDone();

            await new ClientExecute(Cmd.Seefile + $" {Path.GetFileName(Args)}").Execute(Peer);
            Peer.Reply.WaitOne();
            Progress.NextCommandDone();

            await new ClientExecute(Cmd.Closeshare).Execute(Peer);
            Peer.Reply.WaitOne();
            Progress.NextCommandDone();


        }
    }
}*/