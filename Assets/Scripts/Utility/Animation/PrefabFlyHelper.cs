using System;
using System.Collections;
using UnityEngine;

namespace Utility.Animation
{
    public class PrefabFlyHelper : MonoBehaviour
    {
        public IEnumerator FlyPrefabToPosition(Transform endPosition, float time, Transform prefabToFly, Action onReached, Action onEverySecondElapsed)
        {
            // Store initial position and time elapsed
            var startPosition = prefabToFly.position;
            var elapsedTime = 0f;

            // Control point calculation (midpoint between start and end positions)
            var controlPosition = (startPosition + endPosition.position) / 2f;
            controlPosition.y += 3f; // Adjust Y position of the control point (customize as needed)

            while (elapsedTime < time)
            {
                // Calculate the current time ratio (0 to 1)
                float t = elapsedTime / time;

                // Calculate position using Bézier curve formula
                Vector3 newPosition = CalculateBezierPoint(t, startPosition, controlPosition, endPosition.position);

                // Update the position of the prefab
                prefabToFly.position = newPosition;

                // Increment time
                elapsedTime += Time.deltaTime;

                // Wait for the end of the frame
                yield return new WaitForEndOfFrame();
                
                onEverySecondElapsed?.Invoke();
            }

            // Ensure final position is exact and not based on approximation
            prefabToFly.position = endPosition.position;
            onReached?.Invoke();
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // Bézier curve formula: B(t) = (1-t)^2 * p0 + 2 * (1-t) * t * p1 + t^2 * p2
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * p0; // (1-t)^2 * p0
            point += 2f * u * t * p1; // 2 * (1-t) * t * p1
            point += tt * p2; // t^2 * p2

            return point;
        }
    
    }
}