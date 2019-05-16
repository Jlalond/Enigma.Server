using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Enigma.Server.Networking.ConnectionHandlers
{
    public class UdpConnectHandler : IConnectionHandler 
    {
        private readonly UdpClient _udpClient;

        public UdpConnectHandler(Socket socket)
        {
            _udpClient = new UdpClient(socket.RemoteEndPoint.AddressFamily);
        }

        public void ListenUdp()
        {
            //while (true)
            //{
            //    _udpClient.
            //}
        }
    }
}
