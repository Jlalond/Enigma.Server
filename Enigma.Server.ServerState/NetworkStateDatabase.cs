using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enigma.Server.ServerState.Models;

namespace Enigma.Server.ServerState
{
    public class NetworkStateDatabase : INetworkStateDatabase
    {
        private Dictionary<Guid, ServerIdentityGroup> _serverEntitiesByGuid { get; set; }
        private ConcurrentBag<ServerEntity> _touchedEntities;

        public void Put(Guid guid, object obj)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                _serverEntitiesByGuid[guid].Update(obj);
            }
            else
            {
                _serverEntitiesByGuid.Add(guid, new ServerIdentityGroup(guid, obj));
            }
        }

        public void DeleteEntityGroup(Guid guid)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                _serverEntitiesByGuid[guid] = null;
                // Also refactor this.
            }
        }

        public void DeleteEntity(Guid guid, object obj)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                _serverEntitiesByGuid[guid].Delete(obj);
            }
        }

        public IEnumerable<object> GetAssociatedEntities(Guid guid)
        {
            return _serverEntitiesByGuid[guid].GetCurrentEntityGroupSnapshot();
        }

        public T GetEntityWithType<T>(Guid guid) where T : class
        {
            return _serverEntitiesByGuid[guid].GetEntityOfType<T>();
        }
    }
}
