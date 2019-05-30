using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Enigma.Server.Domain;
using Enigma.Server.Networking.ConnectionHandlers;

namespace EnigmaServer.Networking.UnitTests.HelperClasses
{
    public class SocketHelper
    {
        public Socket Socket { get; private set; }

        private static readonly Dictionary<int, SocketHelper> AlreadyDefinedHelpers = new Dictionary<int, SocketHelper>();
        public const int DefaultTestPort = 54323;
        public static SocketHelper GetSocketHelper(int portNumber)
        {
            StartupInfo.PortNum = portNumber;
            if (AlreadyDefinedHelpers.ContainsKey(portNumber))
            {
                return AlreadyDefinedHelpers[portNumber];
            }
            var callBackContainer = new CallBackContainer<Socket>();
            var connectionInitializer = new ConnectionInitializationListener();

            connectionInitializer.NewSocketEvent += (sender, socket) => callBackContainer.CallBackMethod(sender, socket);

            new TcpClient(Dns.GetHostName(), StartupInfo.PortNum);

            callBackContainer.WaitForCallBack(10000);

            AlreadyDefinedHelpers.Add(portNumber, new SocketHelper
            {
                Socket = callBackContainer.CallBackObject
            });

            return AlreadyDefinedHelpers[portNumber];
        }
    }
}
