using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Enigma.Server.Networking.ConnectionHandlers;

namespace Enigma.Server.Orchestration
{
    public class ServerStateOrchestrator
    {
        private readonly ConnectionInitializationListener _connectionListener;

        public void Initialize(ServerConfiguration config)
        {
            _connectionListener.NewSocketEvent += (sender, socket) => EstablishConnection(socket);
        }

        public void EstablishConnection(Socket socket)
        {
            // do some shit
        }
    }
}
