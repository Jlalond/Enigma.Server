using System;
using System.Collections.Generic;
using System.Linq;
using Enigma.Core.Networking.Identity;
using Enigma.Core.Networking.Utilities;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState.TypeData
{
    public static class TypeDataDictionary
    {
        private static readonly Dictionary<Type, TypeMethodCallingInfo> TypesAndTheirPublicMethods;
        private static readonly Dictionary<string, Type> TypesByTheirTypeNames;

        static TypeDataDictionary()
        {
            var childTypes = ReflectionHelper.GetAllChildTypes(typeof(INetworkEntity));
            TypesAndTheirPublicMethods =
                childTypes.ToDictionary(type => type, v => new TypeMethodCallingInfo(v));
            TypesByTheirTypeNames = ReflectionHelper.GetAllTypesInAppDomain()
                                           .ToDictionary(TypeNamer.GetTypeName, v => v);
        }

        public static TypeMethodCallingInfo GetTypeMethodCallingInfoForType(Type t)
        {
            return TypesAndTheirPublicMethods.ContainsKey(t)
                ? TypesAndTheirPublicMethods[t]
                : throw new ArgumentException($"Type {t} is an unexpected type for method invocation");
        }

        public static Type GetTypeForTypeName(string typeName)
        {
            return TypesByTheirTypeNames.ContainsKey(typeName) ? TypesByTheirTypeNames[typeName] : null;
        }
    }
}