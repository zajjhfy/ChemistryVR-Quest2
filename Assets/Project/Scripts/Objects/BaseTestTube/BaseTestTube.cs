using UnityEngine;

public class BaseTestTube : MonoBehaviour
{
    [SerializeField] protected CorkSocketInteractorExtension _interactor;

    public bool IsCorkAttached => _isCorkAttached;

    protected bool _isCorkAttached;

    protected virtual void OnEnable()
    {
        _interactor.OnCorkAttached += CorkAttachedHandler;
        _interactor.OnCorkDetached += CorkDetachedHandler;
    }

    protected virtual void OnDisable()
    {
        _interactor.OnCorkAttached -= CorkAttachedHandler;
        _interactor.OnCorkDetached -= CorkDetachedHandler;
    }

    protected virtual void CorkAttachedHandler(object sender, System.EventArgs e)
    {
        _isCorkAttached = true;
        Debug.Log("Attached: " + _isCorkAttached);
    }
    protected virtual void CorkDetachedHandler(object sender, System.EventArgs e)
    {
        _isCorkAttached = false;
        Debug.Log("Detached: " + _isCorkAttached);
    }

}
