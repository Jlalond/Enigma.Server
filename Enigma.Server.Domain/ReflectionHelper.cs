using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Server.Domain
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> GetAllChildTypes(Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new ConcurrentBag<Type>();
            Parallel.ForEach(assemblies, (assembly) =>
            {
                var descendentTypes = assembly.GetTypes().Where(c => c.IsSubclassOf(type));
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

        public static string GetTypeName(Type type)
        {
            if (type.FullName != null)
            {
                return type.FullName;
            }

            return "";
        }
    }
}
