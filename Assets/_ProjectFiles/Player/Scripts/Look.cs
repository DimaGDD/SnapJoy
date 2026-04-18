using UnityEngine;

public class Look : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SO_Look _lookParametrs;

    private Transform _cameraTransform;
    private float _mouseSensitivity;
    private float _maxLookAngle;
    private float _xRotation = 0f;
    private Vector2 _lookInput;

    public Vector2 LookInput
    {
        get { return _lookInput; }
        set { _lookInput = value; }
    }

    private void Awake()
    {
        UIManager.Instance.HideCursor();

        _mouseSensitivity = _lookParametrs.MouseSensitivity;
        _maxLookAngle = _lookParametrs.MaxLookAngle;

        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = _lookInput.x * _mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = _lookInput.y * _mouseSensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_maxLookAngle, _maxLookAngle);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}
