using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float _velocity = 10f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private GameObject _cannonWheel;
    private InputAction _moveAction;
    private Rigidbody2D _rigidbody2D;
    private Rigidbody2D _cannonWheelRigidbody2D;
    private Vector2 _moveInput;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_cannonWheel != null)
        {
            _cannonWheelRigidbody2D = _cannonWheel.GetComponent<Rigidbody2D>();
        }

        _moveAction = new InputAction(
            "Move",
            InputActionType.Value,
            expectedControlType: "Axis"
        );
        _moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/d");
        _moveAction.performed += ctx => _moveInput = new Vector2(ctx.ReadValue<float>(), 0);
        _moveAction.canceled += _ => _moveInput = Vector2.zero;
        _moveAction.Enable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(_moveInput);
    }
    private void Move(Vector2 direction)
    {
        if (IsSliderSelected())
        {
            StopMovement();
            return;
        }

        RotateWheel(direction.x);
        if (_rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = new Vector2(direction.x * _velocity, direction.y * _velocity);
        }

        if (_cannonWheelRigidbody2D != null)
        {
            _cannonWheelRigidbody2D.linearVelocity = new Vector2(direction.x * _velocity, direction.y * _velocity);
        }
    }

    private void StopMovement()
    {
        if (_rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
        }

        if (_cannonWheelRigidbody2D != null)
        {
            _cannonWheelRigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_moveAction != null)
        {
            _moveAction.Dispose();
        }
    }

    private bool IsSliderSelected()
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null)
        {
            return false;
        }

        return EventSystem.current.currentSelectedGameObject.GetComponent<Slider>() != null;
    }

    private void RotateWheel(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            float rotationDirection = horizontalInput > 0 ? -1 : 1;
            _cannonWheel.transform.Rotate(0, 0, rotationDirection * _rotationSpeed * Time.deltaTime);
        }
    }
}
