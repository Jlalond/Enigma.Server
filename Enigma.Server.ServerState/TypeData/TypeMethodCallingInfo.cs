using System;
using System.Collections.Generic;
using System.Linq;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState.TypeData
{
    public class TypeMethodCallingInfo
    {
        public IReadOnlyDictionary<string, CallSiteInfo> MethodsAndTheirCallSiteInfos { get; }

        public TypeMethodCallingInfo(Type type)
        {
            MethodsAndTheirCallSiteInfos = ReflectionHelper
                                           .GetPublicInstanceMethods(type).Select(c => new CallSiteInfo(c))
                                           .ToDictionary(callSite => callSite.MethodName, value => value);
        }
    }
}
