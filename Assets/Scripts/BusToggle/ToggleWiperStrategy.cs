using UnityEngine;

public class ToggleWiperStrategy : IToggleStrategy
{
    private Animator targetAnimator;

    public ToggleWiperStrategy(Animator animator)
    {
        targetAnimator = animator;
    }

    public void Execute()
    {
        if (targetAnimator != null)
        {
            bool currentState = targetAnimator.GetBool("IsWiping");
            targetAnimator.SetBool("IsWiping", !currentState);
        }
    }
}