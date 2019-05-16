using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Enigma.Server.Networking.ConnectionHandlers
{
    public class TcpConnectionHandler : IConnectionHandler
    {
        private readonly Socket _socket;
        public TcpConnectionHandler(Socket socket)
        {
            _socket = socket;
            new Thread(ListenOverSocket).Start();
        }

        private void ListenOverSocket()
        {
            while (true)
            {
                // some code to read the size of the chunk, then sync read it until it's finished.
            }
        }
    }
}
