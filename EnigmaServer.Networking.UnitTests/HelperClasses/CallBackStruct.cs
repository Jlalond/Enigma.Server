using System.Collections.Generic;
using System.Threading;

namespace EnigmaServer.Networking.UnitTests.HelperClasses
{
    internal class CallBackContainer<T>
    {
        public bool ReceivedCallBack { get; private set; }

        public T CallBackObject { get; private set; }

        private static readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);

        public void CallBackMethod(object sender, T args)
        {
            ReceivedCallBack = true;
            CallBackObject = args;
            ResetEvent.Reset();
        }

        public void WaitForCallBack(int millisecondsToWait)
        {
            ResetEvent.WaitOne(millisecondsToWait);
        }

    }
}
