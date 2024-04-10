using System;
using UnityEngine;


namespace Splash
{
    public class SplashScene : MonoBehaviour, IDisposable
    {
        

        public void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
        }

     

       

        public void OnDestroy()
        {
            Dispose();   
        }

        public void Dispose()
        { 
        }
    }
}