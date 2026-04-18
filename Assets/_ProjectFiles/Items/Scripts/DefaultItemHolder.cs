using UnityEngine;

public class DefaultItemHolder : MonoBehaviour
{
    [Header("Default Parametrs")]
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private ItemInteractType _itemInteractType;
    [SerializeField] private Transform _itemPosition;

    private bool _hasItem = false;
    private DefaultItem _itemInHolder;

    public Transform ItemPosition
    {
        get { return _itemPosition; }
    }

    public bool HasItem
    {
        get { return _hasItem; }
        set { _hasItem = value; }
    }

    public DefaultItem ItemInHolder
    {
        get { return _itemInHolder; }
        set { _itemInHolder = value; }
    }

    public ItemInteractType ItemInteractType
    {
        get { return _itemInteractType; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(_itemInteractType); }
    }
}
