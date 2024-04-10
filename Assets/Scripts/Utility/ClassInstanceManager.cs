using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class InstanceManager
    {
        private static readonly Dictionary<Type, object> Singles = new Dictionary<Type, object>();

        public static T BindInstanceAsSingle<T>(T instance) where T : IDisposable
        {
            var key = typeof(T);
            if (Singles.ContainsKey(key))
            {
                Debug.LogError($"{key.FullName} is already bound, skipping this instance.");
                return (T)Singles[key];
            }
            Singles.Add(key, instance);
            return instance;
        }

        public static bool TryGetInstanceAsSingle<T>(out T instance) where T : IDisposable
        {
            var key = typeof(T);
            if (Singles.ContainsKey(key)) {
                instance = (T)Singles[typeof(T)];
                return true;
            }

            instance = default;
            return false;
        }
      
        public static T GetInstanceAsSingle<T>() where T : IDisposable
        {
            if (Singles.ContainsKey(typeof(T)))
                return (T)Singles[typeof(T)];
            Debug.LogError($"The type {typeof(T).Name} has not been bound yet, use DependencyProvider.BindInstanceAsSingle to do so.");
            return default;
        }

        public static void UnbindInstanceAsSingle<T>() where T:IDisposable
        {
            var type = typeof(T);
            if (Singles.ContainsKey(type))
            {
                ((T)Singles[type]).Dispose();
                Singles.Remove(type);
            }
            else
                Debug.LogError($"The type {type} is not bound as single!");
        }

    }
}