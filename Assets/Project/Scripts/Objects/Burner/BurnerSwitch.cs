using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BurnerSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _flame;
    [SerializeField] private AudioSource _audioSorce;

    public event EventHandler OnTurnedOn;
    public event EventHandler OnTurnedOff;

    private XRGrabInteractable _interactable;
    private bool _isTurnedOff = true;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _interactable.selectEntered.AddListener(SwitchBurner);
    }

    private void SwitchBurner(SelectEnterEventArgs arg0)
    {
        if (!_isTurnedOff)
        {
            _audioSorce.Stop();
            _flame.SetActive(false);
            _isTurnedOff = true;

            OnTurnedOff?.Invoke(this, EventArgs.Empty);
        }
        else if (_isTurnedOff)
        {
            _audioSorce.Play();
            _flame.SetActive(true);
            _isTurnedOff = false;

            OnTurnedOn?.Invoke(this, EventArgs.Empty);
        }
    }

}
