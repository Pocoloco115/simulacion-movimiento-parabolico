using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CannonManager))]
public class CannonInputHandler : MonoBehaviour
{
    private InputAction _fireAction;
    private CannonManager _cannonManager;

    private void Awake()
    {
        _cannonManager = GetComponent<CannonManager>();
        _fireAction = new InputAction("Fire", InputActionType.Button, binding: "<Keyboard>/space");
        _fireAction.performed += HandleFirePerformed;
    }

    private void OnEnable()
    {
        _fireAction?.Enable();
    }

    private void OnDisable()
    {
        _fireAction?.Disable();
    }

    private void OnDestroy()
    {
        if (_fireAction != null)
        {
            _fireAction.performed -= HandleFirePerformed;
            _fireAction.Dispose();
        }
    }

    private void HandleFirePerformed(InputAction.CallbackContext context)
    {
        _cannonManager?.Fire();
    }
}