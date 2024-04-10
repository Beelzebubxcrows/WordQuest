using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;


namespace Utility.Animation.TextAnimations
{
    public class TextAnimator : MonoBehaviour
    {
        [SerializeField] private string textToWrite;
        [SerializeField] private float animationSpeed;
        [SerializeField] private TMP_Text textComponent;
        
        public IEnumerator PlayLoadingTextAnimation()
        {
            var soundPlayer = InstanceManager.GetInstanceAsSingle<SoundPlayer>();
            var stringBuilder = new StringBuilder();
            var index = 0;
            while (index<textToWrite.Length)
            {
                yield return new WaitForSeconds(animationSpeed);
                stringBuilder.Append(textToWrite[index]);
                index++;
                textComponent.text = stringBuilder.ToString();
                soundPlayer.PlayTypingSound();
            }
        }
    }
}