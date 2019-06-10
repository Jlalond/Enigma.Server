using System.Net.Sockets;

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
