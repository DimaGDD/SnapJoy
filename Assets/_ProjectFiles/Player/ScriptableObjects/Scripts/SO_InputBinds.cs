using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_InputBinds", menuName = "Scriptable Objects/SO_InputBinds")]
public class SO_InputBinds : ScriptableObject
{
    [Header("Binds")]
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _interact;
    [SerializeField] private InputActionReference _rotateItem;

    public InputActionReference LookAction
    {
        get { return _lookAction; }
    }

    public InputActionReference MoveAction
    {
        get { return _moveAction; }
    }

    public InputActionReference Interact
    {
        get { return _interact; }
    }

    public InputActionReference RotateItem
    {
        get { return _rotateItem; }
    }
}
