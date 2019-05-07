using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Server.Domain
{
    public class NetworkDataPoint
    {
        private object _node;
        public object Node
        {
            get { return _node; }
            set
            {
                // This should really never be null.
                if (value != null)
                {
                    _nodeType = value.GetType();
                }

                _node = value;
            }
        }

        private Type _nodeType;

        public bool IsOfType<T>()
        {
            return _nodeType == typeof(T) || _nodeType.IsSubclassOf(typeof(T));
        }

        public T GetAsT<T>()
        {
            return (T) _node;
        }
    }
}
