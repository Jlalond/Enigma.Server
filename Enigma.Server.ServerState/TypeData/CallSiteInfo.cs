using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigma.Server.ServerState.TypeData
{
    public class CallSiteInfo
    {
        public string MethodName { get; }
        public Type DelegateType { get; }

        public CallSiteInfo(MethodInfo method)
        {
            MethodName = method.Name;
            DelegateType =
                Expression.GetDelegateType(method.GetParameters().Select(c => c.GetType()).Append(method.ReturnType)
                                                 .ToArray());
        }
    }
}
