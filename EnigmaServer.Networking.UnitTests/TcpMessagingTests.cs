using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        private ConnectionInitializationListener _connectionInitializationListener;

        public TcpMessagingTests()
        {
            StartupInfo.PortNum = 54232;
        }

        [SetUp]
        public void Init()
        {
            _connectionInitializationListener = new ConnectionInitializationListener();
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

            var tcpClient = new TcpClient();
            var callBackContainer = new CallBackContainer<Socket>();

            _connectionInitializationListener.NewSocketEvent += callBackContainer.CallBackMethod;

            tcpClient.Connect(Dns.GetHostName(), StartupInfo.PortNum);
            
            callBackContainer.WaitForCallBack(1000);

            var tcpConnectionHandler = new TcpConnectionHandler(callBackContainer.CallBackObject);

            tcpClient.Client.Send(bytes.ToArray());

            Assert.AreEqual(jsonObj, tcpConnectionHandler.Messages.Pop());
        }

        private class TestMessage
        {
            public int Test { get; set; }
        }
    }

}
