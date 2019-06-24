using System;
using System.Collections.Generic;

namespace Enigma.Server.ServerState
{
    public interface INetworkStateDatabase
    {
        void Put(Guid guid, object obj);
        void DeleteEntityGroup(Guid guid);
        void DeleteEntity(Guid guid, object obj);
        IEnumerable<object> GetAssociatedEntities(Guid guid);
        IEnumerable<object> GetAllObjectsToBroadcast();
    }
}
