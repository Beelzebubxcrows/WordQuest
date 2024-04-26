using System.Collections.Generic;
using Core.Screens;
using TMPro;
using UnityEngine;

namespace Popups
{
    public class HintPopup : Popup
    {
        [SerializeField] private List<string> hints;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private List<GameObject> heroPrefabs;

        private int _index;

        public override void Initialise()
        {
            _index = 0;
            UpdateUI();
            base.Initialise();
        }

        private void UpdateUI()
        {
            var hintsCount = _index%hints.Count;
            hintText.text =  hints[hintsCount];
            
            for (var index = 0; index<heroPrefabs.Count;index++) {
                if (index is 0 or 1) {
                    heroPrefabs[index].SetActive(true);
                }
                else {
                    heroPrefabs[index].SetActive(index==hintsCount);
                }
            }
        }

        public void OnClickNext()
        {
            _index++;
            UpdateUI();
        }
        
        public void OnClickPrevious()
        {
            _index--;
            UpdateUI();
        }

        public void OnClickClose()
        {
            HandlePopupClose();
        }


    }
}