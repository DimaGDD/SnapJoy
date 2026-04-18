using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_Look", menuName = "Scriptable Objects/SO_Look")]
public class SO_Look : ScriptableObject
{
    [Header("Mouse Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 80f;

    public float MouseSensitivity
    {
        get { return _mouseSensitivity; }
    }

    public float MaxLookAngle
    {
        get { return _maxLookAngle; }
    }
}
