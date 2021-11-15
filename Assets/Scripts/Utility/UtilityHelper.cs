using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WFPIR.Utility
{
    public class UtilityHelper : MonoBehaviour
    {
        public int GetNewRandomNumber(int minimumRandomNumber, int maximumRandomNumber, int previousRandomNumber)
        {
            int randomIdleAnimationIndex = Random.Range(minimumRandomNumber, maximumRandomNumber);

            while (randomIdleAnimationIndex == previousRandomNumber)
            {
                randomIdleAnimationIndex = Random.Range(minimumRandomNumber, maximumRandomNumber);
            }

            return randomIdleAnimationIndex;
        }
    }
}
