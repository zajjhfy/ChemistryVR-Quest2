using UnityEngine;

public class Sink : MonoBehaviour
{
    [SerializeField] private Transform _sinkLeverTransform;
    [SerializeField] private ParticleSystem _pSystem;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float angle = Vector3.Angle(_sinkLeverTransform.up, Vector3.up);

        if(angle > 80f)
        {
            if (!_pSystem.isPlaying)
            {
                _audioSource.Play();
                _pSystem.Play();
            }
        }
        else
        {
            _audioSource.Stop();
            _pSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
