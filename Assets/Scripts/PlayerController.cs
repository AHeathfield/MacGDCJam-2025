using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Scripting.APIUpdating;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    [Header("TimeChange Settings")]
    [SerializeField] private float timeChangeDuration = 0.2f;
    [SerializeField] private float timeChangeCooldown = 3.0f;

    // Globals
    private static Vector3 currentPos = new Vector3(0.0f, 0.0f, 0.0f);
    private static Quaternion currentRot = new Quaternion();


    private bool _canDash;
    private bool _isDashing;
    private bool _dashInput;
    
    private bool _canTimeChange;
    private bool _isTimeChanging;

    private bool _timeInput;
    private bool _presentInput;
    private bool _pastInput;
    private bool _futureInput;
    
    private InputSystem_Actions _playerInputActions;
    private Vector3 _input;
    private float _currentSpeed;
    private Vector3 _velocity;

    private ClosestSwitch closestSwitchPoint;

    void Awake()
    {
        _playerInputActions = new InputSystem_Actions();

        _canDash = true;
        _canTimeChange = true;
    }

    void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    void Start()
    {
        this.transform.position = PlayerController.currentPos;
        this.transform.rotation = PlayerController.currentRot;
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
        TimeChange();

        // Dash routine
        if (_dashInput && _canDash)
        {
            StartCoroutine(Dash());
        }

        // Time Change routine
        if (_canTimeChange && _timeInput)
        {
            _canTimeChange = false;
            StartCoroutine(TimeSwitch());
        }
    }


    void GatherInput()
    {
        Vector2 movementInput = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _input = new Vector3(movementInput.x, 0, movementInput.y);
        _dashInput = _playerInputActions.Player.Sprint.IsPressed();
        
        if (_canTimeChange)
        {
            _presentInput = _playerInputActions.Player.Present.IsPressed();
            _pastInput = _playerInputActions.Player.Past.IsPressed();
            _futureInput = _playerInputActions.Player.Future.IsPressed();
        }
        
        if (_presentInput || _pastInput || _futureInput)
        {
            _timeInput = true;
        }
        

        // Debug.Log(_timeInput);
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

    // Handles the time change
    void TimeChange()
    {
        if (_isTimeChanging)
        {
            if (_presentInput)
            {
                SceneManager.LoadScene("Present");
            }
            else if (_pastInput)
            {
                SceneManager.LoadScene("Past");
            }
            else if (_futureInput)
            {
                SceneManager.LoadScene("Future");
            }
        }
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

    // The coroutine for the time change
    IEnumerator TimeSwitch()
    {
        // So we don't spawn at the spawn point of the scene we keep our current position
        closestSwitchPoint = this.GetComponent<ClosestSwitch>();
        PlayerController.currentPos = closestSwitchPoint.getSwitchPoint();
        PlayerController.currentRot = transform.rotation;

        _canTimeChange = false;
        _isTimeChanging = true;
        yield return new WaitForSeconds(timeChangeDuration);
        _isTimeChanging = false;
        yield return new WaitForSeconds(timeChangeCooldown);
        _canTimeChange = true;

        // Putting us back to the position before we switched time
        // transform.position = currentPos; 
    }
}
