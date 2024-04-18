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

       
        public void Initialise(int score, Transform scoreTransform, Action onReached)
        {
            scoreText.text = score.ToString();
            StartCoroutine(prefabFlyHelper.FlyPrefabToPosition(scoreTransform, 0.6f, transform, onReached, OnEverySecond));
        }

        public void SetColor(Color color)
        {
            background.color = color;
        }

        private void OnEverySecond()
        {
        }
    }
}