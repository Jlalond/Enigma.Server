using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState.TypeData
{
    public class TypeMethodCallingInfo
    {
        public readonly Dictionary<string, CallSiteInfo> MethodsAndTheirCallSiteInfos;

        public TypeMethodCallingInfo(Type type)
        {
            MethodsAndTheirCallSiteInfos = ReflectionHelper
                                           .GetPublicInstanceMethods(type).Select(c => new CallSiteInfo(c))
                                           .ToDictionary(callSite => callSite.MethodName, value => value);
        }
    }
}
