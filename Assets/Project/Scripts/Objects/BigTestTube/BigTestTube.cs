using System;
using System.Collections;
using UnityEngine;

public class BigTestTube : BaseLiquidTank
{
    [SerializeField] private BigTestTubeCollider _elementTriggerCollider;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem _bubbleParticles;
    [SerializeField] private ParticleSystem _steamParticles;

    private BaseChemicalElement _element;


    protected override void Start()
    {
        base.Start();
        _elementTriggerCollider.OnDissolvableEnter += OnDissolvableEnterCallback;
        _elementTriggerCollider.OnDissolvableExit += OnDissolvableExitCallback;
    }

    public override void FillLiquid(BaseLiquidElement liquid)
    {
        base.FillLiquid(liquid);
    }

    private void OnDissolvableExitCallback(object sender, ChemicalElementEventArgs e)
    {
        if (_element == null) return;

        var elementName = e.BaseChemicalElement.GetChemicalElementSO().elementName;

        if(_element.GetChemicalElementSO().elementName == elementName)
        {
            StopReaction();
        }
    }

    private void OnDissolvableEnterCallback(object sender, ChemicalElementEventArgs e)
    {
        if(_isFilled)
        {
            string args = "";
            _element = e.BaseChemicalElement;

            if (!(_element.GetChemicalElementSO().reactsWith == _liquid.GetLiquidElement().ElementName))
            {
                _element = null;
                return;
            }
                
            args = _element.GetChemicalElementSO().elementName;

            TaskManager.MarkTaskCompleted("BigTestTube", TaskActionType.BigTestTubeReactionProcess, args);

            StartReaction();
        }
    }

    public void StartReaction()
    {
        var dissolvable = _element as IDissolvable;

        TaskManager.MarkTaskCompleted("BigTestTube", TaskActionType.BigTestTubeReactionProcess, _element.GetChemicalElementSO().elementName);

        dissolvable.Dissolve();

        _bubbleParticles.Play();
        _steamParticles.Play();
    }

    public void StopReaction()
    {
        var dissolvable = _element as IDissolvable;

        dissolvable.StopDissolving();

        _element = null;

        _bubbleParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _steamParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }


}
