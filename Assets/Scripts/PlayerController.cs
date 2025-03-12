using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float accelerationFactor = 5f;
    [SerializeField] private float decelerationFactor = 10f;

    [SerializeField] private float rotationOffset = 45f;  // Field just in case we change isometric offset

    [SerializeField] private float gravity = -9.81f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.5f;

    private bool _canDash;
    private bool _isDashing;
    private bool _dashInput;
    
    private InputSystem_Actions _playerInputActions;
    private Vector3 _input;
    private float _currentSpeed;
    private Vector3 _velocity;

    void Awake()
    {
        _playerInputActions = new InputSystem_Actions();

        _canDash = true;
    }

    void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    void Update()
    {
        // bool isGrounded = _characterController.isGrounded;
        bool isGrounded = true;
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (!isGrounded)
        {
            _velocity.y = gravity * Time.deltaTime;
        }
        
        GatherInput();
        Look();
        CalculateSpeed();
        Move();

        if (_dashInput && _canDash)
        {
            StartCoroutine(Dash());
        }
    }


    void GatherInput()
    {
        Vector2 movementInput = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _input = new Vector3(movementInput.x, 0, movementInput.y);
        _dashInput = _playerInputActions.Player.Sprint.IsPressed();

        Debug.Log(_input);
    }

    void Look()
    {
        if (_input == Vector3.zero) return;
        
        Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 multipliedMatrix = isometricMatrix.MultiplyPoint3x4(_input);

        Quaternion rotation = Quaternion.LookRotation(multipliedMatrix, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void CalculateSpeed()
    {
        if (_input == Vector3.zero && _currentSpeed > 0)
        {
            _currentSpeed -= decelerationFactor * Time.deltaTime;
        }
        else if (_input != Vector3.zero && _currentSpeed < maxSpeed)
        {
            _currentSpeed += accelerationFactor * Time.deltaTime;
        }

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, maxSpeed);
    }

    void Move()
    {
        // Normalizing in case diagonal faster
        _input.Normalize();
        Vector3 moveDir = _input * _currentSpeed * Time.deltaTime;
        moveDir = Quaternion.Euler(0, rotationOffset, 0) * moveDir; // Making up for the isometric offset

        if (_isDashing)
        {
            transform.Translate(transform.forward * dashSpeed * Time.deltaTime, Space.World);
            return;
        }

        transform.Translate(moveDir, Space.World);
        
    }

    IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
}
