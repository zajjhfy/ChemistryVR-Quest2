using UnityEngine;

public class SnapPlayer : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;

    public Vector3 position;
    public Vector3 eulerAngles;

    private void Start() => TeleportPlayerToPoint();

    public void TeleportPlayerToPoint()
    {
        gameObj.transform.position = position;
        gameObj.transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
