using UnityEngine;

public class Copper : BaseChemicalElement, IHeatable
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void Heat()
    {
        if (animator.IsPaused) animator.PlayAnimation();

        Debug.Log(animator.GetClipIsFinishedPlaying());
    }

    public void StopHeat()
    {
        animator.StopAnimation();
    }
}
