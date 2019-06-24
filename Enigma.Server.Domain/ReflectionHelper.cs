using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Enigma.Core.Networking.Identity.BroadcastFrequency;

namespace Enigma.Server.Domain
{
    public static class ReflectionHelper
    {
        private static IReadOnlyDictionary<Type, BroadCastFrequencySetting> _typeAndBroadCastFrequencySettings;
        static ReflectionHelper()
        {
            var typesAndBroadcastSettings = new Dictionary<Type, BroadCastFrequencySetting>();
            var allTypesInDomain = GetAllTypesInAppDomain();
            foreach (var type in allTypesInDomain)
            {
                if (typesAndBroadcastSettings.ContainsKey(type))
                {
                    continue;
                }

                var broadCastFrequencyAttributes =
                    type.GetCustomAttributes(typeof(BroadCastFrequencyAttribute)).ToList();
                if (broadCastFrequencyAttributes.Any())
                {
                    var attribute = broadCastFrequencyAttributes.FirstOrDefault() as BroadCastFrequencyAttribute;
                    typesAndBroadcastSettings.Add(type, attribute?.BroadCastFrequencySetting ?? BroadCastFrequencySetting.OnUpdate);
                }
            }

            _typeAndBroadCastFrequencySettings = typesAndBroadcastSettings;
        }
        public static IEnumerable<Type> GetAllChildTypes(Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new ConcurrentBag<Type>();
            Parallel.ForEach(assemblies, (assembly) =>
            {
                var descendentTypes = assembly.GetTypes().Where(c => c.IsSubclassOf(type) || (type).IsAssignableFrom(c));
                foreach (var childType in descendentTypes)
                {
                    types.Add(childType);
                }
            });

            return types;
        }

        public static IEnumerable<MethodInfo> GetPublicInstanceMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<Type> GetAllTypesInAppDomain()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new ConcurrentBag<Type>();
            Parallel.ForEach(assemblies, (assembly) =>
            {
                var searchedTypes = assembly.GetTypes();
                foreach (var searchedType in searchedTypes)
                {
                    types.Add(searchedType);
                }
            });

            return types;
        }

        public static BroadCastFrequencySetting GetBroadCastFrequencySettingForType(Type t)
        {
            return _typeAndBroadCastFrequencySettings[t];
        }
    }
}
