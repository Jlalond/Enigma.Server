using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enigma.Server.Domain.ConcurrnetBagExtensions;

namespace Enigma.Server.ServerState.Models
{
    internal class ServerEntityIdentity
    {
        private Dictionary<Type, ServerEntity> _entitiesByType;
        private readonly ConcurrentBag<ServerEntity> _touched;
        public Guid Guid { get; }

        public ServerEntityIdentity(Guid guid, object initialState)
        {
            Guid = guid;
            Update(initialState);
            _touched = new ConcurrentBag<ServerEntity>();
        }

        public void Update(object value)
        {
            if (_entitiesByType.ContainsKey(value.GetType()))
            {
                _entitiesByType[value.GetType()].PushState(value);
            }
            else
            {
                _entitiesByType.Add(value.GetType(), new ServerEntity(value));
            }
            _touched.Add(_entitiesByType[value.GetType()]);
        }

        public void FlushAll()
        {
            _touched.ClearAndDoAction((serverEntity) => serverEntity.Flush());
        }
    }
}
