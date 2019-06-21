using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Enigma.Server.Domain.ConcurrnetBagExtensions;

namespace Enigma.Server.ServerState.Models
{
    internal class ServerIdentityGroup
    {
        private Dictionary<Type, ServerEntity> _entitiesByType;
        private readonly ConcurrentBag<ServerEntity> _touched;
        public Guid Guid { get; }

        public ServerIdentityGroup(Guid guid, object initialState)
        {
            Guid = guid;
            Update(initialState);
            _touched = new ConcurrentBag<ServerEntity>();
            _entitiesByType = new Dictionary<Type, ServerEntity>();
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

        public void Delete(object value)
        {
            if (_entitiesByType.ContainsKey(value.GetType()))
            {
                _entitiesByType[value.GetType()] = null;
                // We really need to refactor this, as we don't want to let a client machine delete an object globally unless they have full authority.
            }
        }

        public void FlushAll()
        {
            _touched.ClearAndDoAction((serverEntity) => serverEntity.Flush());
        }

        public void ExecuteMethod(Type type, string methodName, object[] parameters)
        {
            Debug.Assert(_entitiesByType.ContainsKey(type));
            _entitiesByType[type].ExecMethod(methodName, parameters);
        }

        public IEnumerable<object> GetCurrentEntityGroupSnapshot() =>
            _entitiesByType.Values.Select(c => c.Current);

        public T GetEntityOfType<T>()
        {
            if (_entitiesByType.ContainsKey(typeof(T)))
            {
                return (T)_entitiesByType[typeof(T)].Current;
            }
            // TODO: throw good exception.
            throw new Exception();
        }
    }
}
