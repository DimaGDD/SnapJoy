using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SO_InputBinds _inputBinds;

    private InputActionReference _lookAction;
    private InputActionReference _moveAction;
    private InputActionReference _interact;
    private InputActionReference _rotateItem;
    private InputActionReference _skipDialogue;

    private States _playerStates;
    private RaycastFromMouse _raycastFromMouse;

    // Look
    public static event Action<Vector2> OnLookInput;

    // Movement
    public static event Action<Vector2> OnMoveInput;

    // Interact
    public static event Action OnInteractPressed;
    public static event Action OnDoubleInteractPressed;
    public static event Action<float> OnInteractHold;

    // Rotate Item
    public static event Action<Vector2> OnItemRotateInput;
    public static event Action OnItemRotateCanceled;

    // Skip Dialogue
    public static event Action OnSkipDialogue;

    private bool _isHittingCurrentItem = false;

    // Interact Parametrs
    private float _interactStartTime = 0f;
    private bool _holdTriggered = false;

    private void Awake()
    {
        _lookAction = _inputBinds.LookAction;
        _moveAction = _inputBinds.MoveAction;
        _interact = _inputBinds.Interact;
        _rotateItem = _inputBinds.RotateItem;
        _skipDialogue = _inputBinds.SkipDialogue;

        _playerStates = GetComponent<States>();
        _raycastFromMouse = GetComponent<RaycastFromMouse>();
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
        //_interact.action.performed += OnInteractAction;
        _interact.action.started += OnInteractStart;
        _interact.action.canceled += OnInteractCanceled;

        // Rotate Item
        _rotateItem.action.Enable();

        _rotateItem.action.started += ctx =>
        {
            if (_playerStates.IsViewItem)
                _isHittingCurrentItem = _raycastFromMouse.CastRayFromCursor(_playerStates);
        };

        _rotateItem.action.canceled += ctx =>
        {
            if (_playerStates.IsViewItem)
                _isHittingCurrentItem = false;
        };

        _skipDialogue.action.performed += OnSkipDialoguePerformed;
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
        if (!_playerStates.IsViewItem && !_playerStates.IsInDialogue)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            OnLookInput?.Invoke(Vector2.zero);
        }
    }

    // Movement
    private void OnMoveAction(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem && !_playerStates.IsInDialogue)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            OnMoveInput?.Invoke(Vector2.zero);
        }
    }

    private void OnInteractStart(InputAction.CallbackContext context)
    {
        if (!_playerStates.IsViewItem)
        {
            OnInteractPressed?.Invoke();
        }
        else
        {
            OnDoubleInteractPressed?.Invoke();
        }

        _interactStartTime = Time.time;
        _holdTriggered = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        float holdDuration = Time.time - _interactStartTime;
        _holdTriggered = false;

        OnInteractHold?.Invoke(-1f);
    }

    private void Update()
    {
        if (_holdTriggered)
        {
            float holdDuration = Time.time - _interactStartTime;
            OnInteractHold?.Invoke(holdDuration);
        }

        if (_playerStates.IsViewItem)
        {
            if (_rotateItem.action.IsPressed())
            {
                if (_isHittingCurrentItem)
                {
                    Vector2 lookDelta = _lookAction.action.ReadValue<Vector2>();

                    if (lookDelta.sqrMagnitude > 0.001f)
                    {
                        OnItemRotateInput?.Invoke(lookDelta);
                    }
                }
            }
            else
            {
                OnItemRotateCanceled?.Invoke();
            }
        }
    }

    private void OnSkipDialoguePerformed(InputAction.CallbackContext context)
    {
        if (_playerStates.IsInDialogue)
            OnSkipDialogue?.Invoke();
    }
}
