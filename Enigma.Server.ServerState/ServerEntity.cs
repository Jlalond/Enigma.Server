using System;
using System.Collections.Generic;
using System.Text;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState
{
    public class ServerEntity
    {
        public Guid NetworkGuid { get; set; }
        public bool StateHasChanged { get; set; }
        public int ModifyingThreadId { get; set; }
        public Type Type { get; }

        private readonly EntityStateStack _entityStateStack;
        private object _currentTickValue;

        public ServerEntity(object initialValue)
        {
            _entityStateStack = new EntityStateStack();
            PushState(initialValue);
            Type = initialValue.GetType();
        }

        /// <summary>
        /// Push a new object's state to the most current
        /// </summary>
        /// <param name="obj"></param>
        public void PushState(object obj)
        {
            _currentTickValue = obj;
        }


        /// <summary>
        /// Method to be called at the end of a frame
        /// </summary>
        public void Flush()
        {
            _entityStateStack.Push(_currentTickValue);
        }
    }
}
