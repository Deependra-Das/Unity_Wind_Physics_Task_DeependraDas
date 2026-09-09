using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _ascendAction;
    [SerializeField] private InputActionReference _descendAction;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 12f;
    [SerializeField] private float _verticalSpeed = 8f;
    [SerializeField] private float _acceleration = 8f;
    [SerializeField] private float _rotationSpeed = 8f;
    [SerializeField] private float _maxTilt = 20f;
    [SerializeField] private float _tiltSpeed = 6f;
    [SerializeField] private Transform _visualRoot;

    private Rigidbody _droneRB;
    private Vector2 _moveInput;
    private float _verticalInput;

    private void Awake()
    {
        _droneRB = GetComponent<Rigidbody>();

        _droneRB.useGravity = false;
        _droneRB.linearDamping = 2f;
        _droneRB.angularDamping = 4f;
        _droneRB.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void OnEnable()
    {
        _moveAction.action.performed += OnMove;
        _moveAction.action.canceled += OnMove;

        _ascendAction.action.performed += OnAscend;
        _ascendAction.action.canceled += OnAscend;

        _descendAction.action.performed += OnDescend;
        _descendAction.action.canceled += OnDescend;

        _moveAction.action.Enable();
        _ascendAction.action.Enable();
        _descendAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= OnMove;
        _moveAction.action.canceled -= OnMove;

        _ascendAction.action.performed -= OnAscend;
        _ascendAction.action.canceled -= OnAscend;

        _descendAction.action.performed -= OnDescend;
        _descendAction.action.canceled -= OnDescend;

        _moveAction.action.Disable();
        _ascendAction.action.Disable();
        _descendAction.action.Disable();
    }

    private void FixedUpdate()
    {
        MoveDrone();
        RotateDrone();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnAscend(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _verticalInput += 1f;
        }
        else if (context.canceled)
        {
            _verticalInput -= 1f;
        }
    }

    private void OnDescend(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _verticalInput -= 1f;
        }
        else if (context.canceled)
        {
            _verticalInput += 1f;
        }
    }

    private void MoveDrone()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 horizontalMovement = cameraRight * _moveInput.x + cameraForward * _moveInput.y;

        if (horizontalMovement.sqrMagnitude > 1f)
        {
            horizontalMovement.Normalize();
        }

        horizontalMovement *= _moveSpeed;

        Vector3 verticalMovement = Vector3.up * _verticalInput * _verticalSpeed;
        Vector3 targetVelocity = horizontalMovement + verticalMovement;

        _droneRB.linearVelocity = Vector3.Lerp(_droneRB.linearVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);
    }


    private void RotateDrone()
    {
        Vector3 horizontalVelocity = new Vector3( _droneRB.linearVelocity.x, 0f, _droneRB.linearVelocity.z);

        if (horizontalVelocity.sqrMagnitude < 0.05f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity.normalized, Vector3.up);
        _droneRB.MoveRotation(Quaternion.Slerp(_droneRB.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
    }
}
