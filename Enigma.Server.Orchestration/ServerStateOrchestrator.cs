using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Enigma.Core.Networking.Messaging;
using Enigma.Server.Domain;
using Enigma.Server.Networking.ConnectionHandlers;
using Enigma.Server.ServerState;

namespace Enigma.Server.Orchestration
{
    public class ServerStateOrchestrator
    {
        private readonly ConnectionInitializationListener _connectionListener;
        private readonly IList<EstablishedConnection> _establishedConnections;
        private readonly ISerializer _serializer;
        private INetworkStateDatabase _networkStateDatabase;

        public ServerStateOrchestrator(ISerializer serializer, INetworkStateDatabase networkStateDatabase)
        {
            _serializer = serializer;
            _networkStateDatabase = networkStateDatabase;
            _establishedConnections = new List<EstablishedConnection>();
            _connectionListener.NewSocketEvent += (sender, socket) => EstablishConnection(socket);
            new Thread(ListenerLoop).Start();
        }

        private void EstablishConnection(Socket socket)
        {
            _establishedConnections.Add(new EstablishedConnection(socket));
        }

        private void ListenerLoop()
        {
            ListenForCreationAndDeletionEvents();
        }

        private void ListenForCreationAndDeletionEvents()
        {
            foreach (var connection in _establishedConnections)
            {
                var popValue = connection.TcpConnectionHandler.Messages.Pop();
                var messageWrapper = _serializer.Deserialize<MessageWrapper>(popValue);
                _networkStateDatabase.Put(messageWrapper.Object.AssociatedNetworkIdentity, messageWrapper.Object);
            }
        }
    }
}
