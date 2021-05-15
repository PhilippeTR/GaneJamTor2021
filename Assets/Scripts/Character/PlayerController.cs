using UnityEngine;
using UnityEngine.InputSystem;

public struct Inputs
{
    public float Horizontal;
    public float Vertical;
    public bool Jump;
}

// This script requires thoses components and will be added if they aren't already there
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterMovement))]

public class PlayerController : MonoBehaviour
{
    private Inputs CurrentInputs;

    [Header("Actions name")]
    [SerializeField]
    private string _moveActionName = "Move";
    [SerializeField]
    private string _jumpActionName = "Jump";

    private PlayerInput _playerInput;
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _characterMovement = GetComponent<CharacterMovement>();

        CurrentInputs = new Inputs();

        BindActions();
    }

    private void BindActions()
    {
        _playerInput.actions[_moveActionName].started += OnMove;
        _playerInput.actions[_moveActionName].performed += OnMove;
        _playerInput.actions[_moveActionName].canceled += OnMove;

        _playerInput.actions[_jumpActionName].started += OnJump;
    }

    private void OnDisable()
    {
        UnBindActions();

    }

    private void UnBindActions()
    {
        _playerInput.actions[_moveActionName].started -= OnMove;
        _playerInput.actions[_moveActionName].performed -= OnMove;
        _playerInput.actions[_moveActionName].canceled -= OnMove;

        _playerInput.actions[_jumpActionName].started -= OnJump;
    }

    // Methods used to update the inputs
    #region Input action callbacks
    private void OnMove(InputAction.CallbackContext input)
    {
        Vector2 moveInput = input.ReadValue<Vector2>();

        CurrentInputs.Horizontal = moveInput.x;
        CurrentInputs.Vertical = moveInput.y;
    }

    private void OnJump(InputAction.CallbackContext input)
    {
        float jumpValue = input.ReadValue<float>();

        switch (jumpValue)
        {
            // Press
            case 1.0f:
                CurrentInputs.Jump = true;
                break;
            // Release
            case .0f:
                break;
        }
    }
    #endregion

    private void Update()
    {
        // Only update when time isn't stop
        if (Time.deltaTime > .0f)
        {
            //Inputs inputsUsed = CurrentInputs;
            _characterMovement.UpdateInput(CurrentInputs);
        }
    }

    private void LateUpdate()
    {
        ResetNecessaryInputs();
    }

    private void ResetNecessaryInputs()
    {
        CurrentInputs.Jump = false;
    }
}
