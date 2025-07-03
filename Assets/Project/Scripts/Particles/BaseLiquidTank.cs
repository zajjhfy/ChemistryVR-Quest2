using UnityEngine;

public class BaseLiquidTank : MonoBehaviour, ILiquidFillable
{
    [SerializeField] protected Renderer _liquidRenderer;

    [SerializeField] protected float _liquidFillProgress = 0.336f;


    public BaseLiquidElement LiquidElement => _liquid;
    public bool LiquidIsFilled => _isFilled;

    protected BaseLiquidElement _liquid;
    protected bool _isFilled;


    protected virtual void Start()
    {
        _liquidRenderer.material.SetFloat("_Fill", _liquidFillProgress);
    }

    public virtual void FillLiquid(BaseLiquidElement liquid)
    {
        if (_liquid == null)
        {
            _liquid = liquid;
            _liquidRenderer.material.SetColor("_TopColor", _liquid.GetLiquidElement().TopColor);
            _liquidRenderer.material.SetColor("_SideColor", _liquid.GetLiquidElement().SideColor);
            _liquidFillProgress += 0.001f;
            _liquidRenderer.material.SetFloat("_Fill", _liquidFillProgress);
        }
        else
        {
            if (_liquidFillProgress < 0.362f)
            {
                _liquidFillProgress += 0.005f;
                _liquidRenderer.material.SetFloat("_Fill", _liquidFillProgress);
            }
            else if (_liquidFillProgress >= 0.362f)
            {
                _isFilled = true;

                string args = _liquid.GetLiquidElement().ElementName;

                TaskManager.MarkTaskCompleted("BigTestTube", TaskActionType.BigTestTubeFill, args);
            }
        }
    }

}
