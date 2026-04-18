using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SO_InputBinds _inputBinds;

    private InputActionReference _lookAction;
    private InputActionReference _moveAction;
    private InputActionReference _interact;
    private InputActionReference _rotateItem;

    private Look _lookScript;
    private Movement _movementScript;
    private Interact _interactScript;
    private States _playerStates;

    public static event Action<Vector2> OnItemRotateInput;
    public static event Action OnItemRotateCanceled;

    private void Awake()
    {
        _lookAction = _inputBinds.LookAction;
        _moveAction = _inputBinds.MoveAction;
        _interact = _inputBinds.Interact;
        _rotateItem = _inputBinds.RotateItem;

        _lookScript = GetComponent<Look>();
        _movementScript = GetComponent<Movement>();
        _interactScript = GetComponent<Interact>();
        _playerStates = GetComponent<States>();
    }

    private void OnEnable()
    {
        // Look
        _lookAction.action.Enable();
        _lookAction.action.performed += OnLookTrigger;
        _lookAction.action.canceled += OnLookTrigger;

        // Movement
        _moveAction.action.Enable();
        _moveAction.action.performed += OnMoveTrigger;
        _moveAction.action.canceled += OnMoveTrigger;

        // Interact
        _interact.action.Enable();
        _interact.action.performed += OnInteractPerformed;

        // Rotate Item
        _rotateItem.action.Enable();
    }

    private void OnDisable()
    {
        // Look
        _lookAction.action.performed -= OnLookTrigger;
        _lookAction.action.canceled -= OnLookTrigger;
        _lookAction.action.Disable();

        // Movement
        _moveAction.action.performed -= OnMoveTrigger;
        _moveAction.action.canceled -= OnMoveTrigger;
        _moveAction.action.Disable();

        // Interact
        _interact.action.performed -= OnInteractPerformed;
        _interact.action.Disable();

        // Rotate Item
        _rotateItem.action.Disable();
    }

    // Look
    private void OnLookTrigger(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
            _lookScript.LookInput = context.ReadValue<Vector2>();
    }

    // Movement
    private void OnMoveTrigger(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
            _movementScript.MoveInput = context.ReadValue<Vector2>();
    }

    // Interact
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
            _interactScript.TryInteract();
    }

    private void Update()
    {
        if (_playerStates.IsViewItem)
        {
            if (_rotateItem.action.IsPressed())
            {
                Vector2 lookDelta = _lookAction.action.ReadValue<Vector2>();

                if (lookDelta.sqrMagnitude > 0.001f)
                {
                    OnItemRotateInput?.Invoke(lookDelta);
                }
            }
            else
            {
                OnItemRotateCanceled?.Invoke();
            }
        }
    }
}
