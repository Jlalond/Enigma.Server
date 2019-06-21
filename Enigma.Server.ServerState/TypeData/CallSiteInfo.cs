using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigma.Server.ServerState.TypeData
{
    public class CallSiteInfo
    {
        public string MethodName { get; }
        private Type _delegateType { get; }
        private readonly MethodInfo _methodInfo;

        public CallSiteInfo(MethodInfo method)
        {
            _methodInfo = method;
            MethodName = method.Name;
            _delegateType =
                Expression.GetDelegateType(method.GetParameters().Select(c => c.GetType()).Append(method.ReturnType)
                                                 .ToArray());
        }

        public Delegate BuildDelegateForInstance(object obj)
        {
            return _methodInfo.CreateDelegate(_delegateType, obj);
        }
    }
}
