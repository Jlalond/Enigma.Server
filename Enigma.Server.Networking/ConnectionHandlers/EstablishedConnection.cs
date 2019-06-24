using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Threading.Tasks;
using Enigma.Core.Networking.Messaging;

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

        public void SendStatelessMessage(IEnumerable<object> objects)
        {
            foreach (var value in objects)
            {
                var message = MessageBuilder.GetMessage(value, MessageStrategy.NoHeader);
                UdpConnectHandler.SendMessage(message);
            }
        }
    }
}
