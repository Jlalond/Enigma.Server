using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Enigma.Server.Networking.ConnectionHandlers
{
    public class UdpConnectionHandler : IConnectionHandler 
    {
        private readonly UdpClient _udpClient;
        public Stack<string> Messages { get; }

        public UdpConnectionHandler(Socket socket)
        {
            _udpClient = new UdpClient(socket.RemoteEndPoint.AddressFamily);
            _udpClient.Client.Bind(socket.LocalEndPoint);
            Messages = new Stack<string>();
            new Thread(ListenUdp).Start();
        }

        public async void ListenUdp()
        {
            while (true)
            {
                var dataGram = await _udpClient.ReceiveAsync();
                Messages.Push(Encoding.UTF8.GetString(dataGram.Buffer));
            }
        }

        public void SendMessage(byte[] array)
        {
            _udpClient.Send(array, array.Length);
        }
    }
}
