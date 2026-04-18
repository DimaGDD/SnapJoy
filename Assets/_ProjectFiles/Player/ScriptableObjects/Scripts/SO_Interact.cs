using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_Interact", menuName = "Scriptable Objects/SO_Interact")]
public class SO_Interact : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private float _interactRange = 3f;
    [SerializeField] private LayerMask _interactLayerMask;

    public float InteractRange
    {
        get { return _interactRange; }
    }

    public LayerMask InteractLayerMask
    {
        get { return _interactLayerMask; }
    }
}
