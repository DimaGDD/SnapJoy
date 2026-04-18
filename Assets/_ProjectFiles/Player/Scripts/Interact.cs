using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SO_Interact _interactParametrs;

    [Header("Other")]
    [SerializeField] private Transform _itemViewPosition;
    [SerializeField] private Transform _itemHandPosition;

    private Camera _mainCamera;
    private float _interactRange;
    private LayerMask _interactLayerMask;

    private States _playerStates;

    private void OnEnable()
    {
        InputHandler.OnInteractPressed += OnInteractAction;
        InputHandler.OnDoubleInteractPressed += OnDoubleInteractAction;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _interactRange = _interactParametrs.InteractRange;
        _interactLayerMask = _interactParametrs.InteractLayerMask;

        _playerStates = GetComponent<States>();
    }

    private void OnInteractAction()
    {
        if (_playerStates.IsViewItem)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (!Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactLayerMask))
            return;

        DefaultItem hitItem = hit.collider.GetComponent<DefaultItem>();
        DefaultItemHolder hitHolder = hit.collider.GetComponent<DefaultItemHolder>();

        if (hitHolder != null && !hitHolder.HasItem && _playerStates.CurrentItem != null)
        {
            PutItemInHolder(hitHolder);
            return;
        }

        if (hitItem != null && hitItem.InteractType == ItemInteractType.Pickup && _playerStates.CurrentItem == null)
        {
            PickupItem(hitItem);
            return;
        }
    }

    private void PickupItem(DefaultItem item)
    {
        DefaultItemHolder oldHolder = item.GetComponentInParent<DefaultItemHolder>();
        if (oldHolder != null)
        {
            oldHolder.HasItem = false;
            oldHolder.ItemInHolder = null;
        }

        item.MoveToTarget(_itemViewPosition, true);

        _playerStates.IsViewItem = true;
        _playerStates.CurrentItem = item;

        UIManager.Instance.ShowViewPanel(item.Description);
        UIManager.Instance.ShowCursor();
    }

    private void PutItemInHolder(DefaultItemHolder holder)
    {
        _playerStates.CurrentItem.MoveToTarget(holder.ItemPosition, false);

        holder.HasItem = true;
        holder.ItemInHolder = _playerStates.CurrentItem;

        _playerStates.CurrentItem = null;
    }

    //private void OnInteractAction()
    //{
    //    Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

    //    if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactLayerMask))
    //    {
    //        DefaultItem defaultItem = hit.collider.GetComponent<DefaultItem>();
    //        DefaultItemHolder defaultItemHolder = hit.collider.GetComponent<DefaultItemHolder>();

    //        if (defaultItem != null)
    //        {
    //            if (defaultItem.InteractType == ItemInteractType.Pickup)
    //            {
    //                if (!_playerStates.IsViewItem && _playerStates.CurrentItem == null)
    //                {
    //                    DefaultItemHolder itemHolder = defaultItem.GetComponentInParent<DefaultItemHolder>();
    //                    if (itemHolder != null)
    //                    {
    //                        itemHolder.HasItem = false;
    //                        itemHolder.ItemInHolder = null;
    //                    }

    //                    defaultItem.MoveToTarget(_itemViewPosition, true);
    //                    _playerStates.IsViewItem = true;
    //                    _playerStates.CurrentItem = defaultItem;

    //                    UIManager.Instance.ShowViewPanel(defaultItem.Description);
    //                    UIManager.Instance.ShowCursor();
    //                }
    //            }    
    //        }

    //        if (defaultItemHolder != null)
    //        {
    //            if (_playerStates.CurrentItem != null)
    //            {
    //                if (!defaultItemHolder.HasItem)
    //                {
    //                    _playerStates.CurrentItem.MoveToTarget(defaultItemHolder.ItemPosition, false);
    //                    defaultItemHolder.HasItem = true;
    //                    defaultItemHolder.ItemInHolder = _playerStates.CurrentItem;
    //                    _playerStates.CurrentItem = null;

    //                }
    //            }
    //        }
    //    }
    //}

    private void OnDoubleInteractAction()
    {
        _playerStates.CurrentItem.MoveToTarget(_itemHandPosition, false);
        _playerStates.IsViewItem = false;

        UIManager.Instance.HideViewPanel();
        UIManager.Instance.HideCursor();
    }

    private void OnDisable()
    {
        InputHandler.OnInteractPressed -= OnInteractAction;
        InputHandler.OnDoubleInteractPressed -= OnDoubleInteractAction;
    }
}