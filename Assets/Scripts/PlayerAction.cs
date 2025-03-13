using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAction : MonoBehaviour
{

    private CharacterController _characterController;
    private InputSystem_Actions _playerInputActions;
    void Awake()
    {
        _playerInputActions = new InputSystem_Actions();
        _characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    void GatherInput()
    {
        //Vector2 timeInput = _playerInputActions.Player.

        //Debug.Log(_input);
    }
}
