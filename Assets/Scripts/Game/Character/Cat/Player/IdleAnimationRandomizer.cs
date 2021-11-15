using UnityEngine;

public class IdleAnimationRandomizer : StateMachineBehaviour
{
    private int lastIdleAnimationIndex;

    public int GetNewRandomNumber(int minimumRandomNumber, int maximumRandomNumber, int previousRandomNumber)
    {
        int randomIdleAnimationIndex = Random.Range(minimumRandomNumber, maximumRandomNumber);

        while (randomIdleAnimationIndex == previousRandomNumber)
        {
            randomIdleAnimationIndex = Random.Range(minimumRandomNumber, maximumRandomNumber);
        }

        return randomIdleAnimationIndex;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int newRandomAnimationIndex = GetNewRandomNumber(0, 3, lastIdleAnimationIndex);

        lastIdleAnimationIndex = newRandomAnimationIndex;

        animator.SetInteger("Idle_Animation", newRandomAnimationIndex);
    }
}
