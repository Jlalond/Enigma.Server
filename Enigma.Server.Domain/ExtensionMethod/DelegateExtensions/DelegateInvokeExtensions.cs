using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Enigma.Server.Domain.ExtensionMethod.DelegateExtensions
{
    public static class DelegateInvokeExtensions
    {
        public static void FastDynamicInvoke(this Delegate delegateToInvoke, object[] parameters)
        {
            delegateToInvoke.DynamicInvoke(parameters);
            // TODO: Refactor to do type checking (Via a dictionary?) And then exec as a dynamic
        }
    }
}
