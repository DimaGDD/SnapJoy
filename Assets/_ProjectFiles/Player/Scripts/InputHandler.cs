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

    private States _playerStates;

    // Look
    public static event Action<Vector2> OnLookInput;

    // Movement
    public static event Action<Vector2> OnMoveInput;

    // Interact
    public static event Action OnInteractPressed;
    public static event Action OnDoubleInteractPressed;

    // Rotate Item
    public static event Action<Vector2> OnItemRotateInput;
    public static event Action OnItemRotateCanceled;

    private void Awake()
    {
        _lookAction = _inputBinds.LookAction;
        _moveAction = _inputBinds.MoveAction;
        _interact = _inputBinds.Interact;
        _rotateItem = _inputBinds.RotateItem;

        _playerStates = GetComponent<States>();
    }

    private void OnEnable()
    {
        // Look
        _lookAction.action.Enable();
        _lookAction.action.performed += OnLookAction;
        _lookAction.action.canceled += OnLookAction;

        // Movement
        _moveAction.action.Enable();
        _moveAction.action.performed += OnMoveAction;
        _moveAction.action.canceled += OnMoveAction;

        // Interact
        _interact.action.Enable();
        _interact.action.performed += OnInteractAction;

        // Rotate Item
        _rotateItem.action.Enable();
    }

    private void OnDisable()
    {
        _lookAction.action.Disable();
        _moveAction.action.Disable();
        _interact.action.Disable();
        _rotateItem.action.Disable();
    }

    // Movement
    private void OnLookAction(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }
    }

    // Movement
    private void OnMoveAction(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
    }

    // Interact
    private void OnInteractAction(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
        {
            OnInteractPressed?.Invoke();
        }
        else
        {
            OnDoubleInteractPressed?.Invoke();
        }
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
