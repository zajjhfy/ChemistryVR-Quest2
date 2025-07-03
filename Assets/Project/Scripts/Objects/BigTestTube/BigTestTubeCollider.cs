using System;
using UnityEngine;

public class BigTestTubeCollider : MonoBehaviour
{
    public event EventHandler<ChemicalElementEventArgs> OnDissolvableEnter;
    public event EventHandler<ChemicalElementEventArgs> OnDissolvableExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDissolvable>(out var dissolvable))
        {
            var element = dissolvable as BaseChemicalElement;

            OnDissolvableEnter?.Invoke(this, new ChemicalElementEventArgs() {BaseChemicalElement = element});
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDissolvable>(out var dissolvable))
        {
            var element = dissolvable as BaseChemicalElement;

            OnDissolvableExit?.Invoke(this, new ChemicalElementEventArgs() { BaseChemicalElement = element });
        }
    }
}
