using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Enigma.Server.Domain;
using Enigma.Core.Networking.Identity;

namespace Enigma.Server.ServerState.TypeData
{
    public static class TypeDataDictionary
    {
        internal static Dictionary<Type, CallSiteInfo> TypesAndTheirPublicMethods;

        static TypeDataDictionary()
        {
            var childTypes = ReflectionHelper.GetAllChildTypes(typeof(NetworkEntity));
            TypesAndTheirPublicMethods =
                childTypes.ToDictionary(type => type, v => new CallSiteInfo(v));
        }
    }
}
