using UnityEngine;

public class Magnesium : MonoBehaviour, IHeatable
{
    [SerializeField] private ChemicalElement _magnesiumSO;
    [SerializeField] private MeshRenderer _meshRenderer;

    private float _heat = 0;

    public void Heat()
    {
        throw new System.NotImplementedException();
    }

    public void StopHeat()
    {
        throw new System.NotImplementedException();
    }
}
