using Core.Screens;


namespace Popups
{
    public class SettingsPopup : Popup
    {
        
        public void OnClickClose()
        {
            StartCoroutine(HandlePopupClose());
        }
    }
}