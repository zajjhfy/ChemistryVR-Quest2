using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Net.Sockets;

public class BottleSocketInteractorExtension : XRSocketInteractor
{
    public event EventHandler OnLidAttached;
    public event EventHandler OnLidDetached;

    public AudioClip attachedClip;
    public AudioClip detachedClip;

    [SerializeField] private AudioSource _audioSource;

    protected override bool StartSocketSnapping(XRGrabInteractable grabInteractable)
    {
        _audioSource.PlayOneShot(attachedClip);

        OnLidAttached?.Invoke(this, EventArgs.Empty);

        return base.StartSocketSnapping(grabInteractable);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        _audioSource.PlayOneShot(detachedClip);

        OnLidDetached?.Invoke(this, EventArgs.Empty);

        base.OnSelectExited(args);
    }

}
