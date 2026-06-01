using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CannonManager))]
public class CannonInputHandler : MonoBehaviour
{
    private InputAction _fireAction;
    private InputAction _spawnTargetAction;
    private InputAction _pauseAction;
    private CannonManager _cannonManager;

    private void Awake()
    {
        _cannonManager = GetComponent<CannonManager>();
        _fireAction = new InputAction("Fire", InputActionType.Button, binding: "<Keyboard>/space");
        _fireAction.performed += HandleFirePerformed;

        _spawnTargetAction = new InputAction("SpawnTarget", InputActionType.Button, binding: "<Keyboard>/r");
        _spawnTargetAction.performed += HandleSpawnTargetPerformed;

        _pauseAction = new InputAction("Pause", InputActionType.Button, binding: "<Keyboard>/escape");
        _pauseAction.performed += HandlePauseStarted;
    }

    private void OnEnable()
    {
        _fireAction?.Enable();
        _spawnTargetAction?.Enable();
        _pauseAction?.Enable();
    }

    private void OnDisable()
    {
        _fireAction?.Disable();
        _spawnTargetAction?.Disable();
        _pauseAction?.Disable();
    }

    private void OnDestroy()
    {
        if (_fireAction != null)
        {
            _fireAction.performed -= HandleFirePerformed;
            _fireAction.Dispose();
        }

        if (_spawnTargetAction != null)
        {
            _spawnTargetAction.performed -= HandleSpawnTargetPerformed;
            _spawnTargetAction.Dispose();
        }
        if (_pauseAction != null)
        {
            _pauseAction.performed -= ctx => Time.timeScale = Time.timeScale > 0 ? 0 : 1;
            _pauseAction.Dispose();
        }
    }

    private void HandleFirePerformed(InputAction.CallbackContext context)
    {
        _cannonManager?.Fire();
    }

    private void HandleSpawnTargetPerformed(InputAction.CallbackContext context)
    {
        TargetSpawner.Instance?.SpawnTarget();
    }
    private void HandlePauseStarted(InputAction.CallbackContext ctx)
    {
        Time.timeScale = (Time.timeScale > 0f) ? 0f : 1f;
    }
}