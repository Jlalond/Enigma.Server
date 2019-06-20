﻿using System;
using System.Collections.Generic;
using System.Linq;
using Enigma.Core.Networking.Identity;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState.TypeData
{
    public static class TypeDataDictionary
    {
        private static readonly Dictionary<Type, TypeMethodCallingInfo> TypesAndTheirPublicMethods;

        static TypeDataDictionary()
        {
            var childTypes = ReflectionHelper.GetAllChildTypes(typeof(NetworkEntity));
            TypesAndTheirPublicMethods =
                childTypes.ToDictionary(type => type, v => new TypeMethodCallingInfo(v));
        }

        public static TypeMethodCallingInfo GetTypeMethodCallingInfoForType(Type t)
        {
            return TypesAndTheirPublicMethods.ContainsKey(t)
                ? TypesAndTheirPublicMethods[t]
                : throw new ArgumentException($"Type {t} is an unexpected type for method invocation");
        }
    }
}