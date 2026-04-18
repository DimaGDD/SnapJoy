using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SO_Movement _movementParametrs;

    private CharacterController _controller;
    private float _walkSpeed;
    private float _gravity;
    private Vector2 _moveInput;
    private Vector3 _velocity;

    private void OnEnable()
    {
        InputHandler.OnMoveInput += OnMoveAction;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _walkSpeed = _movementParametrs.WalkSpeed;
        _gravity = _movementParametrs.Gravity;
    }

    private void Update()
    {
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * _walkSpeed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnMoveAction(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void OnDisable()
    {
        InputHandler.OnMoveInput -= OnMoveAction;
    }
}
