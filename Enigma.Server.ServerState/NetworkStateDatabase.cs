using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Enigma.Core.Networking.Identity.BroadcastFrequency;
using Enigma.Server.Domain;
using Enigma.Server.ServerState.Models;

namespace Enigma.Server.ServerState
{
    public class NetworkStateDatabase : INetworkStateDatabase
    {
        private ISet<ServerEntity> _alwaysUpdateLoop;
        private Dictionary<Guid, ServerIdentityGroup> _serverEntitiesByGuid;
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

            if (ReflectionHelper.GetBroadCastFrequencySettingForType(obj.GetType()) ==
                BroadCastFrequencySetting.AlwaysBroadcast &&
                _alwaysUpdateLoop.Contains(_serverEntitiesByGuid[guid].GetEntityOfType(obj.GetType())) == false)
            {
                _alwaysUpdateLoop.Add(_serverEntitiesByGuid[guid].GetEntityOfType(obj.GetType()));
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


        public IEnumerable<object> GetAllObjectsToBroadcast()
        {
            return _alwaysUpdateLoop.Select(c => c.Current).Concat(_touchedEntities.Select(c => c.Current)).Distinct();
        }
    }
}