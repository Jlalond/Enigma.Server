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
        public Stack<string> Messages { get; }
        public TcpConnectionHandler(Socket socket)
        {
            _socket = socket;
            new Thread(ListenOverSocket).Start();
            Messages = new Stack<string>();
        }

        private void ListenOverSocket()
        {
            while (true)
            {
                var messageSize = GetMessageSize();
                var messageArray = new byte[messageSize];
                _socket.Receive(messageArray);
                Messages.Push(Encoding.UTF8.GetString(messageArray));
            }
        }

        private int GetMessageSize()
        {
            var bytes = new byte[4];
            var size = _socket.Receive(bytes);
            return BitConverter.ToInt32(bytes);
        }
    }
}
