using UnityEngine;

public class DefaultPushItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InteractTextConfig _interactTextConfig;

    [Header("Settings")]
    [SerializeField] private float _pushForce = 500f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public ItemInteractType InteractType
    {
        get { return ItemInteractType.Push; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(InteractType); }
    }

    public void PushItem(Vector3 direction)
    {
        _rb.AddForce(direction * _pushForce, ForceMode.Impulse);
    }
}
