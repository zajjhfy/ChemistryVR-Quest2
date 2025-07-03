using UnityEngine;

public class PotassiumPermanganate : BaseChemicalElement, IDecompositionable
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
