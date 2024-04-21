using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Animation;

namespace Gameboard
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private PrefabFlyHelper prefabFlyHelper;
        
       
        public void Initialise(string score,Sprite sprite, Transform scoreTransform, Action<GameObject> onReached, float timeDuration)
        {
            background.sprite = sprite;
            scoreText.text = score;
            StartCoroutine(prefabFlyHelper.FlyPrefabToPosition(scoreTransform, timeDuration, transform, onReached, null));
        }
    }
}