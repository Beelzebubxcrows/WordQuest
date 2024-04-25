using Core;
using Core.Screens;
using UnityEngine;
using Utility;


namespace Popups
{
    public class SettingsPopup : Popup
    {
        
        public void OnClickClose()
        {
            StartCoroutine(HandlePopupClose());
        }

        
        
        
        public void OnClickQuit()
        {
            var screenManager = InstanceManager.GetInstanceAsSingle<ScreenManager>();
            screenManager.DoAfterTransition(()=>Application.Quit(),3);
        }
    }
}