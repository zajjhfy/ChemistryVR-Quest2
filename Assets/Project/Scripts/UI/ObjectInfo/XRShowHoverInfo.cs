using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRShowHoverInfo : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextMeshProUGUI _objectText;

    public string objectInfo;

    private XRGrabInteractable _interactable;

    private float _time;
    private bool _isSelected = false;
    private bool _isExited = true;

    private void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();

        _interactable.hoverEntered.AddListener(x =>
        {
            if(!_isSelected && _isExited)
            {
                _isSelected = true;
                _isExited = false;
            }
        });
        _interactable.hoverExited.AddListener(x => _isExited = true);
    }

    private void Update()
    {
        if (_isSelected)
        {
            _time += Time.deltaTime;

            _canvas.SetActive(true);
            _objectText.text = objectInfo;

            if (_time >= 1.5f)
            {
                _time = 0f;
                _isSelected = false;
                _canvas.SetActive(false);
            }
        }
    }

}
