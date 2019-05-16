using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Enigma.Server.Networking.ConnectionHandlers
{
    public class EstablishedConnection
    {
        private readonly TcpConnectionHandler _tcpConnectionHandler;
        private readonly UdpConnectHandler _udpConnectHandler;
        private readonly SftpConnectionHandler _sftpConnectionHandler;

        public EstablishedConnection(Socket socket)
        {
            _tcpConnectionHandler = new TcpConnectionHandler(socket);
        }
    }
}
