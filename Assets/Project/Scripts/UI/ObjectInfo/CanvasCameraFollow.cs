using UnityEngine;

public class CanvasCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    public float transformForward = 0.6f;
    public float yPos = 0.5f;

    void Update()
    {
        float y = _cameraTransform.position.y + yPos;
        Vector3 cameraVector = new Vector3(_cameraTransform.position.x, 
            y, _cameraTransform.position.z);

        Vector3 pos = cameraVector + _cameraTransform.forward * transformForward;

        transform.position = pos;

        transform.rotation = _cameraTransform.rotation;

    }
}
