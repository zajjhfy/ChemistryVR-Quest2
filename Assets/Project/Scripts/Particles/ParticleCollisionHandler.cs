using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    [Header("Liquid Reference")]
    [SerializeField] private BaseLiquidElement _liquid;

    [SerializeField] private float _waitForFillSeconds = 1f;

    private float _timeWithoutCollision = 0f;
    private bool _coroutineRunning = false;
    private IEnumerator _fillCoroutine = null;

    private ILiquidFillable _currentFillTarget = null;

    private Dictionary<ILiquidFillable, BaseLiquidElement> _liquidClones = new Dictionary<ILiquidFillable, BaseLiquidElement>();

    private void Update()
    {
        if (_coroutineRunning)
        {
            _timeWithoutCollision += Time.deltaTime;

            if (_timeWithoutCollision > 0.5f)
            {
                StopFillCoroutine();
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<ILiquidFillable>(out var fillable))
        {
            _timeWithoutCollision = 0f;

            if (_currentFillTarget != fillable)
            {
                StopFillCoroutine();

                _currentFillTarget = fillable;
                _fillCoroutine = FillLiquid(_currentFillTarget);
                StartFillCoroutine();
            }
        }
    }

    private void StartFillCoroutine()
    {
        if (_fillCoroutine != null)
        {
            StartCoroutine(_fillCoroutine);
            _coroutineRunning = true;
        }
    }

    private void StopFillCoroutine()
    {
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
        }

        if (_currentFillTarget != null && _liquidClones.TryGetValue(_currentFillTarget, out var clone))
        {
            Destroy(clone.gameObject);
            _liquidClones.Remove(_currentFillTarget);
        }

        _coroutineRunning = false;
        _currentFillTarget = null;
        _timeWithoutCollision = 0f;
    }

    private IEnumerator FillLiquid(ILiquidFillable fillable)
    {
        if (!_liquidClones.TryGetValue(fillable, out var liquidClone))
        {
            liquidClone = Instantiate(_liquid);
            _liquidClones.Add(fillable, liquidClone);
        }

        while (true)
        {
            yield return new WaitForSeconds(_waitForFillSeconds);

            if ((fillable as BaseLiquidTank).LiquidIsFilled)
            {
                Destroy(liquidClone.gameObject);
                _liquidClones.Remove(fillable);
                yield break;
            }

            fillable.FillLiquid(liquidClone);
        }
    }
}
