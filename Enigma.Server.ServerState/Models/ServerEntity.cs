using System;
using System.Collections.Generic;
using System.Linq;
using Enigma.Server.Domain.ExtensionMethod.DelegateExtensions;
using Enigma.Server.ServerState.Data_Structures;
using Enigma.Server.ServerState.TypeData;

namespace Enigma.Server.ServerState.Models
{
    public class ServerEntity
    {
        private Dictionary<string, Delegate> _methodsByName;
        private readonly EntityStateStack _entityStateStack;
        private object _currentTickValue;
        public Type Type;
        public object Current => _entityStateStack.Current();

        public ServerEntity(object initialValue)
        {
            _entityStateStack = new EntityStateStack();
            PushState(initialValue);
            Type = initialValue.GetType();
            _methodsByName = TypeDataDictionary.GetTypeMethodCallingInfoForType(Type)
                                               .MethodsAndTheirCallSiteInfos
                                               .ToDictionary(c => c.Key, v => v.Value.BuildDelegateForInstance(this));
        }

        /// <summary>
        /// Push a new object's state to the most current
        /// </summary>
        /// <param name="obj"></param>
        public void PushState(object obj)
        {
            _currentTickValue = obj;
        }


        /// <summary>
        /// Method to be called at the end of a frame
        /// </summary>
        public void Flush()
        {
            _entityStateStack.Push(_currentTickValue);
        }

        public void ExecMethod(string methodName, object[] parameters)
        {
            if (!_methodsByName.ContainsKey(methodName))
            {
                // TODO: Build a good descriptive exception
            }

            _methodsByName[methodName].FastDynamicInvoke(parameters);
        }
    }
}
