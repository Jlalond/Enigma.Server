using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Enigma.Server.Networking.ConnectionHandlers;
using EnigmaServer.Networking.UnitTests.HelperClasses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EnigmaServer.Networking.UnitTests
{
    public class UdpMessagingTests
    {
        private readonly SocketHelper _socketHelper;

        public UdpMessagingTests()
        {
            _socketHelper = SocketHelper.GetSocketHelper(SocketHelper.DefaultTestPort);
        }

        [Test]
        public void SendDataGram()
        {
            var obj = new TestMessage
            {
                Test = 1446
            };

            var udpListener = new UdpConnectionHandler(_socketHelper.Socket);
            var objBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            var udpClient = new UdpClient();
            udpClient.Connect(Dns.GetHostName(), SocketHelper.DefaultTestPort);
            udpClient.Send(objBytes, objBytes.Length);

            Thread.Sleep(1000);

            Assert.AreEqual(udpListener.Messages.First(), JsonConvert.SerializeObject(obj));

        }
    }
}
