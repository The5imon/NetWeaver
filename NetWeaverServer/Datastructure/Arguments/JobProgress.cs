using System;
using System.Collections.Generic;

namespace NetWeaverServer.Datastructure.Arguments
{
    public class JobProgress
    {
        public Client Client { get; private set; }
        public event EventHandler ProgressChanged;
        public double Percentage { get; private set; } = 0;
        public bool Done { get; private set; } = false;
        private int CommandCount { get; set; }

        public JobProgress(Client client)
        {
            Client = client;
        }

        public void SetCommandCount(int commands)
        {
            CommandCount += commands;
        }

        public void NextCommandDone()
        {
            Percentage += 1d / CommandCount;
            if (Percentage >= 1)
            {
                Done = true;
            }
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return $"{Client.HostName} is at: {Percentage*100} %";
        }
    }
}