using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CorkSocketInteractorExtension : XRSocketInteractor
{
    private Vector3 _originalColliderSize;
    private Vector3 _originalColliderCenter;

    [SerializeField] private AudioSource _audioSource;

    public event EventHandler OnCorkAttached;
    public event EventHandler OnCorkDetached;

    protected override bool StartSocketSnapping(XRGrabInteractable grabInteractable)
    {
        var collider = grabInteractable.GetComponent<BoxCollider>();
        _originalColliderCenter = collider.center;
        _originalColliderSize = collider.size;

        collider.size = UpdateColliderSize(collider);
        collider.center = UpdateColliderCenter(collider);

        OnCorkAttached?.Invoke(this, EventArgs.Empty);

        return base.StartSocketSnapping(grabInteractable);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if(args.interactableObject is XRGrabInteractable grabInteractable)
        {
            var coll = args.interactableObject.colliders.First().GetComponent<BoxCollider>();
            coll.size = _originalColliderSize;
            coll.center = _originalColliderCenter;

            _audioSource.Play();

            OnCorkDetached?.Invoke(this, EventArgs.Empty);
        }


        base.OnSelectExited(args);
    }

    private Vector3 UpdateColliderSize(BoxCollider originalColliderSize)
    {
        return new Vector3(originalColliderSize.size.x, 0.734f, originalColliderSize.size.z);
    }

    private Vector3 UpdateColliderCenter(BoxCollider originalColliderCenter)
    {
        return new Vector3(originalColliderCenter.center.x, 0.697f, originalColliderCenter.center.z);
    }

}
