using UnityEngine;

public class BottleContainer : MonoBehaviour
{
    [SerializeField] private ChemicalElement _element;
    [SerializeField] private BottleSocketInteractorExtension _interactor;

    private bool _isLidAttached;

    public bool IsLidAttached => _isLidAttached;

    public ChemicalElement GetChemicalElement() => _element;

    private void OnEnable()
    {
        _interactor.OnLidAttached += LidAttachedHandler;
        _interactor.OnLidDetached += LidDetachedHandler;
    }

    private void OnDisable()
    {
        _interactor.OnLidAttached -= LidAttachedHandler;
        _interactor.OnLidDetached -= LidDetachedHandler;
    }


    private void LidAttachedHandler(object sender, System.EventArgs e) => _isLidAttached = true;
    private void LidDetachedHandler(object sender, System.EventArgs e) => _isLidAttached = false;

    
}
