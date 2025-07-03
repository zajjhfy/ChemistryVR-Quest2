using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Coal : BaseChemicalElement, IHeatable
{
    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private ParticleSystem _hints;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void Heat()
    {
        if (animator.IsPaused)
        {
            _sparks.Play();
            _hints.Play();
            animator.PlayAnimation();
        }
    }

    public void StopHeat()
    {
        _sparks.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _hints.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        animator.StopAnimation();
    }
}
