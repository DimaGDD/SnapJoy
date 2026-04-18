using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using System.Collections;
using Mono.Cecil.Cil;

public enum ItemInteractType
{
    Pickup,
    Talk,
    Spin,
    Put,
    Open
}

public class DefaultItem : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private ItemInteractType _interactType;
    [SerializeField] private string _description;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeedWhilePickup = 10f;
    [SerializeField] private float _rotationSpeedWhileView = 10f;

    private Quaternion _defaultRotation;
    private Coroutine _coroutine;
    private bool _isMoving = false;

    public ItemInteractType InteractType
    {
        get { return _interactType; }
    }

    public string Description
    {
        get { return _description; }
    }

    public void MoveToTarget(Transform itemViewPosition, bool isSubActions)
    {
        transform.SetParent(itemViewPosition);

        StartCoroutine(SmoothMove(itemViewPosition, isSubActions));
    }

    private IEnumerator SmoothMove(Transform itemPosition, bool isSubActions)
    {
        _isMoving = true; 

        while (Vector3.Distance(transform.position, itemPosition.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, itemPosition.position, Time.deltaTime * _moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, itemPosition.rotation, Time.deltaTime * _rotationSpeedWhilePickup);

            yield return null;
        }

        transform.position = itemPosition.position;
        transform.rotation = itemPosition.rotation;

        _defaultRotation = transform.rotation;

        _isMoving = false;

        if (isSubActions)
        {
            InputHandler.OnItemRotateInput += HandleRotationInput;
            InputHandler.OnItemRotateCanceled += HandleRotationReset;
        }
    }

    private void HandleRotationInput(Vector2 input)
    {
        if (_isMoving) return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        transform.Rotate(Vector3.up, -input.x * _rotationSpeedWhileView * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right, input.y * _rotationSpeedWhileView * Time.deltaTime, Space.Self);
    }

    private void HandleRotationReset()
    {
        if (_isMoving) return;

        _coroutine = StartCoroutine(SmoothReset());
    }

    private IEnumerator SmoothReset()
    {
        while (Quaternion.Angle(transform.rotation, _defaultRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _defaultRotation, Time.deltaTime * _rotationSpeedWhileView);
            yield return null;
        }

        transform.rotation = _defaultRotation;
    }

    private void OnDestroy()
    {
        InputHandler.OnItemRotateInput -= HandleRotationInput;
        InputHandler.OnItemRotateCanceled -= HandleRotationReset;
    }
}
