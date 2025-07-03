using System;
using System.Collections;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [Header("Flame Instance")]
    [SerializeField] private BurnerFlame _flameInstance;

    [Header("Switch")]
    [SerializeField] private BurnerSwitch _switch;

    public bool GetIsFlameActive => _isFlameActive;

    private bool _isFlameActive = false;

    private IEnumerator _heatCoroutine;
    private IHeatable _element;

    private void Start()
    {
        _flameInstance.OnFlameEnter += StartHeat;
        _flameInstance.OnFlameExit += StopHeat;
        _heatCoroutine = Heat();

        _switch.OnTurnedOn += OnSwitchTurnedOn;
        _switch.OnTurnedOff += OnSwitchTurnedOff;
    }

    private void OnSwitchTurnedOff(object sender, EventArgs e) => _isFlameActive = false;

    private void OnSwitchTurnedOn(object sender, EventArgs e)
    {
         _isFlameActive = true;
        TaskManager.MarkTaskCompleted("Burner", TaskActionType.BurnerActivation, "");
    }



    private void StopHeat(object sender, FlameEventArgs e)
    {
        _element.StopHeat();

        StopCoroutine(_heatCoroutine);
        _element = null;
    }

    private void StartHeat(object sender, FlameEventArgs e)
    {
        string args = "";

        var heatable = e.heatable as BaseChemicalElement;

        args = heatable.GetChemicalElementSO().elementName;

        TaskManager.MarkTaskCompleted("Burner", TaskActionType.BurnerReactionProcess, args);
        _element = e.heatable;
        StartCoroutine(_heatCoroutine);
    }

    private IEnumerator Heat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (_element != null) 
                _element.Heat();
        }
    }


}
