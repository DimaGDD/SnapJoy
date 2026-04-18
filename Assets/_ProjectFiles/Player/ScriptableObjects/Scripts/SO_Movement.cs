using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_Movement", menuName = "Scriptable Objects/SO_Movement")]
public class SO_Movement : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    public float WalkSpeed
    {
        get { return _walkSpeed; }
    }

    public float Gravity
    {
        get { return _gravity; }
    }
}
