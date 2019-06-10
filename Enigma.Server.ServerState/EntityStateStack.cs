using System.Collections;

namespace Enigma.Server.ServerState
{
    internal class EntityStateStack
    {
        private Stack _stack;

        internal EntityStateStack()
        {
            _stack = new Stack(ServerStateSettings.HistoryToKeep);
        }

        public void Push(object obj)
        {
            _stack.Push(obj);
        }
    }
}
