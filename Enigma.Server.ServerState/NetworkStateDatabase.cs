using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enigma.Server.ServerState.Models;

namespace Enigma.Server.ServerState
{
    public class NetworkStateDatabase : INetworkStateDatabase
    {
        private Dictionary<Guid, ServerEntityIdentity> _serverEntitiesByGuid { get; set; }
        private ConcurrentBag<ServerEntity> _touchedEntities;

        public void Put(Guid guid, object obj)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                _serverEntitiesByGuid[guid].Update(obj);
            }
            else
            {
                _serverEntitiesByGuid.Add(guid, new ServerEntityIdentity(guid, obj));
            }
        }

        public void Delete(Guid guid)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                _serverEntitiesByGuid[guid] = null;
            }
        }

        public void Delete(Guid guid, object obj)
        {
            if (_serverEntitiesByGuid.ContainsKey(guid))
            {
                //
            }
        }

        public IEnumerable<object> Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(Guid guid) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
