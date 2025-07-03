using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DecompositionConstruction : MonoBehaviour
{
    [Header("Socket References")]
    [SerializeField] private XRDecompositionSocketInteractor _elementTestTubeInteractor;
    [SerializeField] private XRDecompositionSocketInteractor _voidTestTubeInteractor;
    [SerializeField] private XRDecompositionSocketInteractor _bowlInteractor;
    [SerializeField] private XRDecompositionSocketInteractor _holderInteractor;
    [SerializeField] private XRDecompositionSocketInteractor _burnerInteractor;

    [Header("Tubes References")]
    [SerializeField] private GameObject _tube1;
    [SerializeField] private GameObject _tube2;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem _oxygen;

    // Link to objects
    private TestTube _elementTestTube;
    private TestTube _voidTestTube;
    private Burner _burner;

    private IDecompositionable _decompositionable;

    private enum ConstructionObjects
    {
        None,
        ElementTestTube,
        VoidTestTube,
        Bowl,
        Holder,
        Burner
    }

    private Dictionary<ConstructionObjects, bool> _objectActiveDictionary = new Dictionary<ConstructionObjects, bool>();

    private void Start()
    {
        InitializeDictionary();
    }

    private void Update()
    {
        CheckObjectConditionsMet();
        if (TryStartDecomposition())
        {
            if (!_oxygen.isPlaying) _oxygen.Play();

            var element = _elementTestTube.ElementIn;

            string args = element.GetChemicalElementSO().elementName;

            TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionReactionProcess, args);

            _decompositionable = element as IDecompositionable;
            _decompositionable.Decompose();

            SetUpTubes(true);
        }
        else
        {
            if(_decompositionable != null)
            {
                _decompositionable.StopDecomposition();
            }

            if(_elementTestTube == null)
            {
                _decompositionable = null;
            }

            _oxygen.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            SetUpTubes(false);
        }

        
    }

    private void OnEnable()
    {
        SubscribeAll();
    }

    private void OnDisable()
    {
        UnsubscribeAll();
    }

    private void InitializeDictionary()
    {
        _objectActiveDictionary.Add(ConstructionObjects.ElementTestTube, false);
        _objectActiveDictionary.Add(ConstructionObjects.VoidTestTube, false);
        _objectActiveDictionary.Add(ConstructionObjects.Bowl, false);
        _objectActiveDictionary.Add(ConstructionObjects.Holder, false);
        _objectActiveDictionary.Add(ConstructionObjects.Burner, false);
    }

    private void SubscribeToSocketEvents(XRDecompositionSocketInteractor socket,
        EventHandler attachedHandler, EventHandler detachedHandler)
    {
        socket.OnObjectAttached += attachedHandler;
        socket.OnObjectDetached += detachedHandler;
    }

    private void UnsubscribeFromSocketEvents(XRDecompositionSocketInteractor socket,
        EventHandler attachedHandler, EventHandler detachedHandler)
    {
        socket.OnObjectAttached -= attachedHandler;
        socket.OnObjectDetached -= detachedHandler;
    }

    private void SubscribeAll()
    {
        SubscribeToSocketEvents(_elementTestTubeInteractor, ElementTestTubeAttachedHandler, ElementTestTubeDetachedHandler);

        SubscribeToSocketEvents(_voidTestTubeInteractor, VoidTestTubeAttachedHandler, VoidTestTubeDetachedHandler);

        SubscribeToSocketEvents(_bowlInteractor, BowlAttachedHandler, BowlDetachedHandler);

        SubscribeToSocketEvents(_holderInteractor, HolderAttachedHandler, HolderDetachedHandler);

        SubscribeToSocketEvents(_burnerInteractor, BurnerAttachedHandler, BurnerDetachedHandler);
    }

    private void UnsubscribeAll()
    {
        UnsubscribeFromSocketEvents(_elementTestTubeInteractor, ElementTestTubeAttachedHandler, ElementTestTubeDetachedHandler);

        UnsubscribeFromSocketEvents(_voidTestTubeInteractor, VoidTestTubeAttachedHandler, VoidTestTubeDetachedHandler);

        UnsubscribeFromSocketEvents(_bowlInteractor, BowlAttachedHandler, BowlDetachedHandler);

        UnsubscribeFromSocketEvents(_holderInteractor, HolderAttachedHandler, HolderDetachedHandler);

        UnsubscribeFromSocketEvents(_burnerInteractor, BurnerAttachedHandler, BurnerDetachedHandler);
    }



    private void HolderDetachedHandler(object sender, EventArgs e) => SetObjectActive(ConstructionObjects.Holder, false);
    private void BowlDetachedHandler(object sender, EventArgs e) => SetObjectActive(ConstructionObjects.Bowl, false);

    private void BurnerDetachedHandler(object sender, EventArgs e)
    {
        _burner = null;
        SetObjectActive(ConstructionObjects.Burner, false);
    }

    private void VoidTestTubeDetachedHandler(object sender, EventArgs e)
    {
        _voidTestTube = null;
        SetObjectActive(ConstructionObjects.VoidTestTube, false);
    }

    private void ElementTestTubeDetachedHandler(object sender, EventArgs e)
    {
        _elementTestTube = null;
        SetObjectActive(ConstructionObjects.ElementTestTube, false);
    }

    private void HolderAttachedHandler(object sender, EventArgs e) 
    {
        TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionConstructObject, "Holder");

        SetObjectActive(ConstructionObjects.Holder, true); 
    }

    private void BurnerAttachedHandler(object sender, EventArgs e)
    {
        var interactor = sender as XRDecompositionSocketInteractor;

        Burner burner = interactor.GetAttachedGameObject.GetComponent<Burner>();

        _burner = burner;
    }

    private void ElementTestTubeAttachedHandler(object sender, EventArgs e)
    {
        var interactor = sender as XRDecompositionSocketInteractor;
        
        TestTube elementTestTube = interactor.GetAttachedGameObject.GetComponent<TestTube>();

        _elementTestTube = elementTestTube;
    }

    private void VoidTestTubeAttachedHandler(object sender, EventArgs e)
    {
        var interactor = sender as XRDecompositionSocketInteractor;

        TestTube voidTestTube = interactor.GetAttachedGameObject.GetComponent<TestTube>();

        _voidTestTube = voidTestTube;
    }

    private void BowlAttachedHandler(object sender, EventArgs e)
    {
        var interactor = sender as XRDecompositionSocketInteractor;

        Bowl bowl = interactor.GetAttachedGameObject.GetComponent<Bowl>();

        var isFilled = bowl.LiquidIsFilled;

        if (isFilled)
        {
            TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionConstructObject, "Bowl");

            SetObjectActive(ConstructionObjects.Bowl, true);
        }
    }


    private bool SetObjectActive(ConstructionObjects cObject, bool isActive)
    {
        _objectActiveDictionary[cObject] = isActive;
        return isActive;
    }

    private void SetUpTubes(bool trigger)
    {
        _tube1.SetActive(trigger);
        _tube2.SetActive(trigger);
    }

    private void CheckObjectConditionsMet()
    {
        if(_burner != null)
        {
            bool activated = SetObjectActive(ConstructionObjects.Burner, _burner.GetIsFlameActive);

            if(activated) TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionConstructObject, "Burner");
        }

        if (_elementTestTube != null)
        {
            bool activated = SetObjectActive(ConstructionObjects.ElementTestTube, _elementTestTube.ElementIn != null && _elementTestTube.IsCorkAttached);

            if (activated) TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionConstructObject, "ElementTestTube");
        }

        if (_voidTestTube != null)
        {
            bool activated = SetObjectActive(ConstructionObjects.VoidTestTube, _voidTestTube.ElementIn == null && !_voidTestTube.IsCorkAttached);

            Debug.Log(_voidTestTube);
            if (activated) TaskManager.MarkTaskCompleted("DecompositionConstruction",
                TaskActionType.DecompositionConstructObject, "VoidTestTube");
        }
            
    }

    private bool TryStartDecomposition() => _objectActiveDictionary.All(x => x.Value == true);


}
