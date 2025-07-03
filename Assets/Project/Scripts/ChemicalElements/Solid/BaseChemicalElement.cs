using UnityEngine;

public class BaseChemicalElement : MonoBehaviour
{
    [SerializeField] protected ChemicalElement chemicalElementSO;
    [SerializeField] protected MeshRenderer meshRenderer;

    protected ElementReactionAnimator animator;

    protected virtual void OnEnable()
    {
        animator = GetComponentInChildren<ElementReactionAnimator>();
        animator.SetAnimationClip(chemicalElementSO.reactionAnimationName);
    }

    public ChemicalElement GetChemicalElementSO() => chemicalElementSO;
    public MeshRenderer GetVisualMeshRenderer() => meshRenderer;
}
