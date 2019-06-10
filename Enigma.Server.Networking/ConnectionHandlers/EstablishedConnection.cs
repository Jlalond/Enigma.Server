using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Enigma.Server.Networking.ConnectionHandlers
{
    public class EstablishedConnection
    {
        public readonly TcpConnectionHandler TcpConnectionHandler;
        public readonly UdpConnectionHandler UdpConnectHandler;

        public EstablishedConnection(Socket socket)
        {
            TcpConnectionHandler = new TcpConnectionHandler(socket);
            UdpConnectHandler = new UdpConnectionHandler(socket);
        }
    }
}
