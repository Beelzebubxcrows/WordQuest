using System;

namespace Utility
{
    public class RandomCharacterSelector : IDisposable
    {
        private readonly int[] _charWeights = new int[]
            { 15,8,8,8,15,8,8,8,15,8,8,8,8,8,15,8,5,8,8,8,15,5,7,2,2,1 };
        
        
        public char SelectRandomCharacter(Random random)
        {
            // Calculate total sum of weights
            int totalWeight = 0;
            foreach (int weight in _charWeights)
            {
                totalWeight += weight;
            }

            // Generate a random number between 0 and totalWeight - 1
            int randomValue = random.Next(0, totalWeight);

            // Find the corresponding character based on the random value
            int cumulativeWeight = 0;
            for (int i = 0; i < _charWeights.Length; i++)
            {
                cumulativeWeight += _charWeights[i];
                if (randomValue < cumulativeWeight)
                {
                    // Convert index to ASCII character code ('a' = 97)
                    return (char)(i + 'A');
                }
            }

            return '.';
        }
        
        public void Dispose()
        {
        }
    }
}