using UnityEngine;

public class Clip : BaseChemicalElement, IDissolvable
{
    public void Dissolve()
    {
        if (animator.IsPaused) animator.PlayAnimation();
    }

    public void StopDissolving()
    {
        animator.StopAnimation();
    }

}
