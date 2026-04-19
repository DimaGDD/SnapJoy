using UnityEngine;

public class DefaultLinkedItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _linkedObject;

    [Header("Settings")]
    [SerializeField] private float _moveDistance = 2f;

    [Header("Move Axis")]
    [SerializeField] private bool _x = false;
    [SerializeField] private bool _y = true;
    [SerializeField] private bool _z = false;

    private Vector3 _startPosition;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _startPosition = _linkedObject.localPosition;

        _moveDirection = new Vector3(
            _x ? 1f : 0f,
            _y ? 1f : 0f,
            _z ? 1f : 0f
        );
    }

    public void MoveObject(float param)
    {
        float clampedParam = Mathf.Clamp01(param);

        Vector3 offset = _moveDirection * _moveDistance * clampedParam;

        _linkedObject.localPosition = _startPosition + offset;
    }    
}
