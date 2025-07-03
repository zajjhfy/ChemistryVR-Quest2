using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TestTubeLiquid : BaseTestTube
{
    [SerializeField] private ParticleSystem _pSystem;

    [SerializeField] private float _pourAngle = 85f;


    private void Update()
    {
        TryPourLiquid();
    }

    private void TryPourLiquid()
    {
        var angle = Vector3.Angle(transform.up, Vector3.up);

        if (angle >= _pourAngle && !_isCorkAttached)
        {
            if(!_pSystem.isPlaying) _pSystem.Play();

        }
        else
        {
            _pSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }


}
