using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRDecompositionSocketInteractor : XRSocketInteractor
{
    public event EventHandler OnObjectAttached;
    public event EventHandler OnObjectDetached;

    public GameObject GetAttachedGameObject => _attachedGameObject;

    private GameObject _attachedGameObject = null;


    protected override bool StartSocketSnapping(XRGrabInteractable grabInteractable)
    {
        _attachedGameObject = grabInteractable.gameObject;

        OnObjectAttached?.Invoke(this, EventArgs.Empty);

        return base.StartSocketSnapping(grabInteractable);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactableObject is XRGrabInteractable grabInteractable)
        {
            OnObjectDetached?.Invoke(this, EventArgs.Empty);

            _attachedGameObject = null;
        }


        base.OnSelectExited(args);
    }
}
