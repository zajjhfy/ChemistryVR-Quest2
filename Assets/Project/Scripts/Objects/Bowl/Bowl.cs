using UnityEngine;

public class Bowl : BaseLiquidTank
{
    protected override void Start()
    {
        base.Start();
    }

    public override void FillLiquid(BaseLiquidElement liquid)
    {
        if (liquid.GetLiquidElement().ElementName != "Water") return;

        if (_liquid == null)
        {
            _liquid = liquid;
            _liquidRenderer.material.SetColor("_TopColor", _liquid.GetLiquidElement().TopColor);
            _liquidRenderer.material.SetColor("_SideColor", _liquid.GetLiquidElement().SideColor);

            _liquidFillProgress += 0.001f;
        }
        else if (_liquidFillProgress < 0.377f)
        {
            _liquidFillProgress += 0.001f;
            _liquidRenderer.material.SetFloat("_Fill", _liquidFillProgress);
        }

        else if (_liquidFillProgress >= 0.377f)
        {
            _isFilled = true;
            TaskManager.MarkTaskCompleted("Bowl", TaskActionType.BowlFill, "");
        }
    }
}
