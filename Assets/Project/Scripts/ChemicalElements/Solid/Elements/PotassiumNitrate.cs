using UnityEngine;

public class PotassiumNitrate : BaseChemicalElement, IDecompositionable
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void Decompose()
    {
        if (animator.IsPaused) animator.PlayAnimation();
    }

    public void StopDecomposition()
    {
        animator.StopAnimation();
    }
}
