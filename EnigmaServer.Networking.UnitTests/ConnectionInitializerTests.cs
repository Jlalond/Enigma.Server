using System.Net;
using System.Net.Sockets;
using Enigma.Server.Domain;
using Enigma.Server.Networking.ConnectionHandlers;
using EnigmaServer.Networking.UnitTests.HelperClasses;
using NUnit.Framework;

namespace EnigmaServer.Networking.UnitTests
{
    [TestFixture]
    public class ConnectionInitializerTests
    {
        private ConnectionInitializationListener _connectInitializer;

        public ConnectionInitializerTests()
        {
            StartupInfo.PortNum = 54232;
        }

        [SetUp]
        public void Init()
        {
            _connectInitializer = new ConnectionInitializationListener();
        }

        [Test]
        public void InitializeConnection_ReceiveMessage()
        {
            var callBackContainer = new CallBackContainer<Socket>();

            _connectInitializer.NewSocketEvent += (sender, socket) => callBackContainer.CallBackMethod(sender, socket);

            var tcpConnector = new TcpClient(Dns.GetHostName(), StartupInfo.PortNum);

            callBackContainer.WaitForCallBack(1000);

            Assert.True(callBackContainer.ReceivedCallBack);
            Assert.NotNull(callBackContainer.CallBackObject);
        }
    }
}