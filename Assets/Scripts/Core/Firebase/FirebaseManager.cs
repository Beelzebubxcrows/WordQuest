using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Core.Firebase
{
    public class FirebaseManager : MonoBehaviour, IDisposable
    {
        private const string PUZZLE_STARTED_EVENT = "puzzle_started_{0}";
        private const string PUZZlE_LOST_EVENT = "puzzle_lost_{0}";
        private const string PUZZLE_WON_EVENT = "puzzle_won_{0}";
        
        private bool _isInitialized;
        private List<string> _cachedEvents;
        
        public async void Initialise()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    // Crashlytics will use the DefaultInstance, as well;
                    // this ensures that Crashlytics is initialized.
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    
                    _isInitialized = true;
                    FireCachedEvents();
                    Debug.Log("Firebase : Initialised");
                }
            });
        }
        
        private void FireCachedEvents()
        {
            if (_cachedEvents == null) {
                return;
            }

            foreach (var cachedEvent in _cachedEvents)
            {
                Debug.Log($"Firebase : Logging {cachedEvent}");
                FirebaseAnalytics.LogEvent(cachedEvent);
            }
            
            _cachedEvents.Clear();
        }

        private void LogEvent(string eventName)
        {
            if (_cachedEvents == null) {
                _cachedEvents = new List<string>();
            }
            
            if (!_isInitialized) {
                Debug.Log($"Firebase : Caching {eventName}");
                _cachedEvents.Add(eventName);
            }else {
                Debug.Log($"Firebase : Logging {eventName}");
                FirebaseAnalytics.LogEvent(eventName);
            }
        }


        public void LogPuzzleStarted(int levelNumber)
        {
            LogEvent(string.Format(PUZZLE_STARTED_EVENT,levelNumber));
        }
        
        public void LogPuzzleLost(int levelNumber)
        {
            LogEvent(string.Format(PUZZlE_LOST_EVENT,levelNumber));
        }
        
        public void LogPuzzleWon(int levelNumber)
        {
            LogEvent(string.Format(PUZZLE_WON_EVENT,levelNumber));
        }

        public void Dispose()
        {
        }
    }
}