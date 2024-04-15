using System.Collections;
using UnityEngine;

namespace Utility.Animation
{
    public class PunchScale : MonoBehaviour
    {
        [SerializeField] private Vector3 punchScaleAmount;
        [SerializeField] private float punchDuration;
        
        private Vector3 _originalScale;

        
        private void Start()
        {
            _originalScale = transform.localScale;
        }

        public IEnumerator PlayPunchScale()
        {
           
                var elapsedTime = 0f;

                while (elapsedTime < punchDuration)
                {
                    var t = elapsedTime / punchDuration;
                    var factor = Mathf.Sin(t * Mathf.PI);

                    // Calculate scaled size using sinusoidal interpolation
                    var targetScale = _originalScale + punchScaleAmount * factor;

                    // Apply the scaled size to the GameObject
                    transform.localScale = targetScale;

                    // Increment time
                    elapsedTime += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }
        }
    }
}