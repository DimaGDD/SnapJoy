using UnityEngine;
using System.Collections;

public class DefaultHoldItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private Transform _holdItem;
    [SerializeField] private DefaultLinkedItem _linkedObject;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _maxRotationAngle = 480f;
    [SerializeField] private float _resetSpeed = 5f;

    [Header("Rotation Axis")]
    [SerializeField] private bool _rotateX = false;
    [SerializeField] private bool _rotateY = true;
    [SerializeField] private bool _rotateZ = false;

    private float _currentTotalAngle = 0f;
    private Coroutine _resetCoroutine;
    private Quaternion _baseRotation;
    private Vector3 _rotationAxisVector;

    public ItemInteractType InteractType
    {
        get { return ItemInteractType.Hold; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(InteractType); }
    }

    private void Awake()
    {
        _baseRotation = _holdItem.localRotation;
        _currentTotalAngle = 0f;

        _rotationAxisVector = new Vector3(
            _rotateX ? 1f : 0f,
            _rotateY ? 1f : 0f,
            _rotateZ ? 1f : 0f
        );
    }
    public void HoldItem()
    {
        if (_currentTotalAngle >= _maxRotationAngle) return;

        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }

        float angleDelta = _rotationSpeed * Time.deltaTime;
        _currentTotalAngle += angleDelta;

        if (_currentTotalAngle > _maxRotationAngle)
            _currentTotalAngle = _maxRotationAngle;

        Quaternion targetRotation = Quaternion.AngleAxis(_currentTotalAngle, _rotationAxisVector);
        _holdItem.localRotation = _baseRotation * targetRotation;

        if (_linkedObject != null)
        {
            float progress = _currentTotalAngle / _maxRotationAngle;
            _linkedObject.MoveObject(progress);
        }
    }
    public void CanceledHold()
    {
        if (_resetCoroutine != null)
            StopCoroutine(_resetCoroutine);

        _resetCoroutine = StartCoroutine(SmoothReset());
    }

    private IEnumerator SmoothReset()
    {
        float progress;

        while (_currentTotalAngle > 0.01f)
        {
            _currentTotalAngle = Mathf.MoveTowards(_currentTotalAngle, 0f, _resetSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.AngleAxis(_currentTotalAngle, _rotationAxisVector);
            _holdItem.localRotation = _baseRotation * targetRotation;

            if (_linkedObject != null)
            {
                progress = _currentTotalAngle / _maxRotationAngle;
                _linkedObject.MoveObject(progress);
            }

            yield return null;
        }

        _currentTotalAngle = 0f;
        _holdItem.localRotation = _baseRotation;

        if (_linkedObject != null)
            _linkedObject.MoveObject(0f);

        _resetCoroutine = null;
    }
}
