using System;
using System.Collections.Generic;

namespace Enigma.Server.ServerState
{
    public interface INetworkStateDatabase
    {
        void Put(Guid guid, object obj);
        void Delete(Guid guid);
        void Delete(Guid guid, object obj);
        IEnumerable<object> Get(Guid guid);
        T Get<T>(Guid guid) where T : class;
    }
}
