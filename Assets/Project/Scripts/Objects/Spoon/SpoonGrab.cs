using System.Collections;
using UnityEngine;

public class SpoonGrab : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;

    private ChemicalElement _element;
    private GameObject _instantiatedElement;

    private bool _isOutOfTrigger = true;
    private bool _isGrabThreshold = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<TestTubeCollider>(out var collider))
        {
            if (_element != null) _element = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<BottleContainer>(out BottleContainer container))
        {
            if(_element == null && !_isGrabThreshold && _isOutOfTrigger)
            {
                if (!container.IsLidAttached)
                {
                    _element = container.GetChemicalElement();

                    Debug.Log(_element);
                    _instantiatedElement = Instantiate(_element.prefab, _attachPoint);

                    TaskManager.MarkTaskCompleted("Spoon", TaskActionType.SpoonElementGrab, _element.elementName);

                    _isOutOfTrigger = false;
                    StartCoroutine(GrabTreshold());
                }
            }
            else if(_instantiatedElement != null && _isOutOfTrigger && !_isGrabThreshold & !container.IsLidAttached)
            {
                if(container.GetChemicalElement().elementName == _element.elementName)
                {
                    Destroy(_instantiatedElement.gameObject);

                    _isOutOfTrigger = false;
                    _element = null;
                    StartCoroutine(GrabTreshold());
                }
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<BottleContainer>(out BottleContainer container))
        {
            _isOutOfTrigger = true;
        }
    }

    private IEnumerator GrabTreshold()
    {
        _isGrabThreshold = true;
        yield return new WaitForSeconds(1.5f);
        _isGrabThreshold = false;
    }
}
