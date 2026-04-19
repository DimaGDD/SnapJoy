using UnityEngine;
using System.Collections;

public class DefaultHoldItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private Transform _holdItem;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _maxRotationAngle = 480f;
    [SerializeField] private float _resetSpeed = 5f;

    private float _currentTotalAngle = 0f;
    private Coroutine _resetCoroutine;
    private Quaternion _baseRotation;

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

        _holdItem.localRotation = _baseRotation * Quaternion.Euler(0, _currentTotalAngle, 0);
    }
    public void CanceledHold()
    {
        if (_resetCoroutine != null)
            StopCoroutine(_resetCoroutine);

        _resetCoroutine = StartCoroutine(SmoothReset());
    }

    private IEnumerator SmoothReset()
    {
        while (_currentTotalAngle > 0.01f)
        {
            _currentTotalAngle = Mathf.MoveTowards(_currentTotalAngle, 0f, _resetSpeed * Time.deltaTime);

            _holdItem.localRotation = _baseRotation * Quaternion.Euler(0, _currentTotalAngle, 0);

            yield return null;
        }

        _currentTotalAngle = 0f;
        _holdItem.localRotation = _baseRotation;
        _resetCoroutine = null;
    }
}
