using UnityEngine;

[CreateAssetMenu()]
public class ChemicalElement : ScriptableObject
{
    public string elementName;
    public string reactionAnimationName;

    public GameObject prefab;
    public string reactsWith;

}
