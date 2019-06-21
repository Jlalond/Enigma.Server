using System.Collections;
using Enigma.Server.ServerState.Settings;

namespace Enigma.Server.ServerState.Data_Structures
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

        public object Current()
        {
            return _stack.Peek();
        }
    }
}
