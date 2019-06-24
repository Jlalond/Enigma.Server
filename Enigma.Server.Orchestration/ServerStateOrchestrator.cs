using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
        private const int MilliSecondsToWait = 33;
        private DateTime _previousDateTime;

        public ServerStateOrchestrator(ISerializer serializer, INetworkStateDatabase networkStateDatabase)
        {
            _serializer = serializer;
            _networkStateDatabase = networkStateDatabase;
            _establishedConnections = new List<EstablishedConnection>();
            _connectionListener.NewSocketEvent += (sender, socket) => EstablishConnection(socket);
            _previousDateTime = DateTime.UtcNow;
            new Thread(ListenerLoop).Start();
        }

        private void EstablishConnection(Socket socket)
        {
            _establishedConnections.Add(new EstablishedConnection(socket));
        }

        private void ListenerLoop()
        {
            ListenForCreationAndDeletionEvents();
            ListenForUdpUpdates();
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

        private void ListenForUdpUpdates()
        {
            foreach (var connection in _establishedConnections)
            {
                var popValue = connection.UdpConnectHandler.Messages.Pop();
                var messageWrapper = _serializer.Deserialize<MessageWrapper>(popValue);
                _networkStateDatabase.Put(messageWrapper.Object.AssociatedNetworkIdentity, messageWrapper.Object);
            }
        }

        private void BroadCastLoop()
        {
            var autoEvent = new AutoResetEvent(false);
            var timer = new Timer(new TimerCallback(BroadcastAll), autoEvent, MilliSecondsToWait, MilliSecondsToWait);
        }

        private void BroadcastAll(object value)
        {
            foreach (var connection in _establishedConnections)
            {
                connection.SendStatelessBroadCast(_networkStateDatabase.GetAllObjectsToBroadcast());
            }
        }
    }
}
