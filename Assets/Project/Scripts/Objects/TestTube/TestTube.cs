using System;
using UnityEngine;

public class TestTube : BaseTestTube
{
    [Header("Transform References")]
    [SerializeField] Transform _elementTransform; 

    [Header("Element Collider")]
    [SerializeField] private TestTubeCollider _collider;

    private BaseChemicalElement _elementInTube;

    public BaseChemicalElement ElementIn => _elementInTube;


    protected override void OnEnable()
    {
        base.OnEnable();
        _collider.OnElementAdd += OnElementAddHandler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _collider.OnElementAdd -= OnElementAddHandler;
    }

    protected override void CorkAttachedHandler(object sender, EventArgs e)
    {
        base.CorkAttachedHandler(sender, e);

        if(_elementInTube != null)
        {
            TaskManager.MarkTaskCompleted("TestTube", TaskActionType.TestTubeElementFill, 
                _elementInTube.GetChemicalElementSO().elementName);
        }
    }

    private void OnElementAddHandler(object sender, ChemicalElementEventArgs args)
    {
        BaseChemicalElement element = args.BaseChemicalElement;

        if (_elementInTube == null)
        {
            GameObject instantiated = Instantiate(element.GetChemicalElementSO().prefab, _elementTransform);
            _elementInTube = instantiated.GetComponent<BaseChemicalElement>();

            Destroy(element.gameObject);

        }

    }


}
