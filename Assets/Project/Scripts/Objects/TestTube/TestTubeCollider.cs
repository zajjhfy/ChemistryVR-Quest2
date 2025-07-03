using System;
using UnityEngine;

public class TestTubeCollider : MonoBehaviour
{
    public event EventHandler<ChemicalElementEventArgs> OnElementAdd;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IDecompositionable>(out var obj))
        {
            var element = obj as BaseChemicalElement;

            OnElementAdd?.Invoke(this, new ChemicalElementEventArgs { BaseChemicalElement = element});
        }
    }
}
