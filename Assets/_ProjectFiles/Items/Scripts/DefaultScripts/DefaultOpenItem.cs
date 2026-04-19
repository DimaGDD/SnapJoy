using UnityEngine;
using UnityEngine.Rendering;

public class DefaultOpenItem : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private DefaultPickupItem _itemToUnlock;

    private Animator _animator;

    private bool _isUnlock = false;

    public ItemInteractType InteractType
    {
        get { return ItemInteractType.Open; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(InteractType); }
    }

    public bool IsUnlock
    {
        get { return _isUnlock; }
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();  
    }

    public void TryUnlock(DefaultPickupItem itemToUnlock)
    {
        if (_isUnlock) return;

        if (itemToUnlock != null && itemToUnlock == _itemToUnlock)
        {
            OnTrueKey(itemToUnlock);
        }
        else
        {
            OnWrongKey();
        }
    }

    private void OnTrueKey(DefaultPickupItem itemToUnlock)
    {
        _isUnlock = true;
        _animator.SetTrigger("Unlock");
        Destroy(itemToUnlock.gameObject);
    }

    private void OnWrongKey()
    {
        _animator.SetTrigger("Fail");
    }
}