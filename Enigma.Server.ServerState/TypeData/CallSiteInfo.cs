using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Enigma.Server.Domain;

namespace Enigma.Server.ServerState.TypeData
{
    internal class CallSiteInfo
    {
        private Dictionary<string, Delegate> _methodsByName;
        private Dictionary<string, IOrderedEnumerable<Type>> _sortedMethodArgumentTypes;
        public CallSiteInfo(Type t)
        {
            var publicInstanceMethod = ReflectionHelper.GetPublicInstanceMethods(t);
            // needs rework to do 1 iteration, not two.
            _methodsByName = publicInstanceMethod.ToDictionary(c => c.Name,
                                                               v => v.CreateDelegate(
                                                                   Expression.GetDelegateType(
                                                                       v.GetParameters().Select(c => c.ParameterType).Concat(new [] {v.ReturnType})
                                                                        .ToArray())));
            _sortedMethodArgumentTypes =
                publicInstanceMethod.ToDictionary(c => c.Name,
                                                  v => v.GetParameters().Select(parameter => parameter.ParameterType)
                                                        .OrderBy(type => type.GetHashCode()));
        }

        public void ExecMethod(string methodName, object[] args)
        {
            if (!_methodsByName.ContainsKey(methodName))
            {
                // Place some logger to log that this shouldn't happen.
                return;
            }

            VerifyArgs(methodName, args);
            _methodsByName[methodName].DynamicInvoke(args);
        }

        private void VerifyArgs(string methodName, IEnumerable<object> args)
        {
            var sortedArgs = args.Select(c => c.GetType()).OrderBy(type => type.GetHashCode());
            if (!_sortedMethodArgumentTypes[methodName].SequenceEqual(sortedArgs))
            {
                // throw a custom exception
            }
            
        }
    }
}
