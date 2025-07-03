using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Transform _cameraTransform;

    private GameObject _menu = null;

    private PlayerIA _inputActions;

    private void Start()
    {
        _inputActions = new PlayerIA();

        _inputActions.Player.Actions.Enable();
        _inputActions.Player.Actions.performed += Actions_performed;
    }

    private void Actions_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(_menu == null)
        {
            Vector3 p = _cameraTransform.position + _cameraTransform.forward;

            _menu = Instantiate(_canvas, p, _cameraTransform.rotation);
        } else
        {
            Destroy(_menu);
        }
    }

    private void OnDisable()
    {
        _inputActions.Player.Actions.Disable();
        _inputActions.Player.Actions.performed -= Actions_performed;
    }
}
