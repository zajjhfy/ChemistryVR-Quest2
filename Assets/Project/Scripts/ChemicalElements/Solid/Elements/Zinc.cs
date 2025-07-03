using UnityEngine;

public class Zinc : BaseChemicalElement, IDissolvable
{
    [SerializeField] private ParticleSystem _bubbles;

    public void Dissolve()
    {
        if (animator.IsPaused)
        {
            animator.PlayAnimation();
            _bubbles.Play();
        }
    }

    public void StopDissolving()
    {
        _bubbles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        animator.StopAnimation();
    }

}
