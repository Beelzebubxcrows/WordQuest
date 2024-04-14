using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public class EventBus : IDisposable
    {
        private readonly Hashtable _eventListeners = new Hashtable();
        public void Register<T>(Action<T> instance)
        {
            Type key = typeof(T);
            if (!_eventListeners.ContainsKey(key))
            {
                _eventListeners.Add(key, instance);
            }
            else
            {
                var eventListener = (Action<T>) _eventListeners[key];
                if (_eventListeners[key] == null)
                    eventListener = instance;
                else
                    eventListener+=instance;
                _eventListeners[key] = eventListener;
            }
        }
        public void Unregister<T>(Action<T> instance)
        {
            var key = typeof(T);
            if (!_eventListeners.ContainsKey(key))
            {
                Debug.LogError($"Not subscribed to {key.Name} yet! But you are trying to Unregister listener");
            }
            else
            {
                var eventListener = (Action<T>) _eventListeners[key];
                if(eventListener==null)
                    Debug.LogError($"No listeners for the event {key.Name} exists but you are trying to Unregister!");
                else
                    eventListener-=instance;
                _eventListeners[key] = eventListener;
            }
        }
        public void Fire<T>(T eventArgument)
        {
            var key = typeof(T);
            // if(!_eventListeners.ContainsKey(key))
            //     Debug.LogWarning($"No listeners exist for {key.Name}");
            var eventListeners = (Action<T>) _eventListeners[key];
            // if (eventListeners == null)
            //     Debug.LogWarning($"No listeners exist for {key.Name}");
            eventListeners?.Invoke(eventArgument);
        }

        public void Dispose()
        {
            _eventListeners.Clear();
        }
    }
}