using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Server.Domain
{
    public interface ISerializer
    {
        T Deserialize<T>(string val);
        string Serialize(object obj);
    }
}
