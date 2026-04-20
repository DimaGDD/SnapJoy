using UnityEngine;

public class DefaultHolderItem : MonoBehaviour
{
    [Header("Default Parametrs")]
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private Transform _itemPosition;

    private bool _hasItem = false;
    private DefaultPickupItem _itemInHolder;

    public Transform ItemPosition
    {
        get { return _itemPosition; }
    }

    public bool HasItem
    {
        get { return _hasItem; }
        set { _hasItem = value; }
    }

    public DefaultPickupItem ItemInHolder
    {
        get { return _itemInHolder; }
        set { _itemInHolder = value; }
    }

    public ItemInteractType ItemInteractType
    {
        get { return ItemInteractType.Holder; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(ItemInteractType); }
    }

    public void Awake()
    {
        DefaultPickupItem pickupItem = GetComponentInChildren<DefaultPickupItem>();

        if (pickupItem != null )
        {
            _hasItem = true;
            _itemInHolder = pickupItem;

            pickupItem.transform.position = _itemPosition.position;
            pickupItem.transform.rotation = _itemPosition.rotation;
        }
    }
}
