using UnityEngine;

public class BaseLiquidElement : MonoBehaviour
{
    [SerializeField] private LiquidElement _liquid;

    public LiquidElement GetLiquidElement() => _liquid;

    public BaseLiquidElement Clone()
    {
        var go = new GameObject("ClonedLiquidElement");
        var clone = go.AddComponent<BaseLiquidElement>();
        clone._liquid = Instantiate(_liquid);

        return clone;
    }
}
