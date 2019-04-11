using System;
using System.Collections.Generic;

namespace NetWeaverServer.Datastructure.Arguments
{
    public class JobProgress
    {
        public Client Client { get; set; }
        
        public double Percentage { get; set; } = 0;

        public bool Done { get; set; } = false;
    }
}