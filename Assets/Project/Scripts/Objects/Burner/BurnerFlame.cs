using System;
using UnityEngine;

public class BurnerFlame : MonoBehaviour
{
    public event EventHandler<FlameEventArgs> OnFlameEnter;
    public event EventHandler<FlameEventArgs> OnFlameExit;

    private void OnTriggerEnter(Collider other)
    {
        IHeatable heatable;

        if(other.gameObject.TryGetComponent<IHeatable>(out heatable))
        {
            OnFlameEnter?.Invoke(this, new FlameEventArgs { heatable = heatable});
        }

    }

    private void OnTriggerExit(Collider other)
    {
        IHeatable heatable;

        if (other.gameObject.TryGetComponent<IHeatable>(out heatable))
        {
            OnFlameExit?.Invoke(this, new FlameEventArgs { heatable = heatable });
        }
    }
}
