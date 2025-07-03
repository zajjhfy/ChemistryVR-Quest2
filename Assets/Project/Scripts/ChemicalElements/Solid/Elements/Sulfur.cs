using UnityEngine;

public class Sulfur : MonoBehaviour, IHeatable
{
    [SerializeField] private ChemicalElement _sulfurSO;
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
