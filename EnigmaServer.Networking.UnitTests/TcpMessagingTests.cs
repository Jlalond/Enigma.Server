using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Enigma.Server.Domain;
using Enigma.Server.Networking.ConnectionHandlers;
using EnigmaServer.Networking.UnitTests.HelperClasses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EnigmaServer.Networking.UnitTests
{
    [TestFixture]
    public class TcpMessagingTests
    {
        private readonly ConnectionInitializationListener _connectionInitializationListener;
        private readonly TcpClient _tcpClient;
        private readonly TcpConnectionHandler _tcpConnectionHandler;

        public TcpMessagingTests()
        {
            StartupInfo.PortNum = 54231;
            _connectionInitializationListener = new ConnectionInitializationListener();
            _tcpClient = new TcpClient();
            var callBackContainer = new CallBackContainer<Socket>();

            _connectionInitializationListener.NewSocketEvent += callBackContainer.CallBackMethod;

            _tcpClient.Connect(Dns.GetHostName(), StartupInfo.PortNum);

            callBackContainer.WaitForCallBack(1000);

            _tcpConnectionHandler = new TcpConnectionHandler(callBackContainer.CallBackObject);

        }

        [Test]
        public void WriteMessageOverTcpPort()
        {
            var obj = new TestMessage
            {
                Test = 42
            };

            var jsonObj = JsonConvert.SerializeObject(obj);
            var jsonAsBytes = Encoding.UTF8.GetBytes(jsonObj);
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(jsonAsBytes.Length));
            bytes.AddRange(jsonAsBytes);

            _tcpClient.Client.Send(bytes.ToArray());

            Thread.Sleep(1000);

            Assert.AreEqual(jsonObj, _tcpConnectionHandler.Messages.Pop());
        }

        [Test]
        public void WriteMultipleMessageOverTcpPort()
        {
            var obj = new TestMessage
            {
                Test = 42
            };

            var jsonObj = JsonConvert.SerializeObject(obj);
            var jsonAsBytes = Encoding.UTF8.GetBytes(jsonObj);
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(jsonAsBytes.Length));
            bytes.AddRange(jsonAsBytes);

            var bytesAsArray = bytes.ToArray();
            for (int i = 0; i < 6; i++)
            {
                _tcpClient.Client.Send(bytesAsArray);
            }

            Thread.Sleep(1000);

            Assert.AreEqual(6, _tcpConnectionHandler.Messages.Count);
            foreach (var message in _tcpConnectionHandler.Messages)
            {
                Assert.AreEqual(jsonObj, message);
            }
        }

        ~TcpMessagingTests()
        {
            _tcpClient.Client.Close();
        }
    }
}
