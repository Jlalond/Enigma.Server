using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Server.Orchestration
{
    public struct ServerConfiguration
    {
        public int StartingPortNumbers { get; set; }
        public int NumberOfSequentialPortsToTry { get; set; }
    }
}
