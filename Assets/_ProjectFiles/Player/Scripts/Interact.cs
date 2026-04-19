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

        DefaultPickupItem hitItem = hit.collider.GetComponent<DefaultPickupItem>();
        DefaultHolderItem hitHolder = hit.collider.GetComponent<DefaultHolderItem>();
        DefaultOpenItem hitOpen = hit.collider.GetComponent<DefaultOpenItem>();

        if (hitHolder != null && !hitHolder.HasItem && _playerStates.CurrentItem != null)
        {
            PutItemInHolder(hitHolder);
            return;
        }

        if (hitOpen != null)
        {
            OpenItem(hitOpen);
            return;
        }

        if (hitItem != null && _playerStates.CurrentItem == null)
        {
            PickupItem(hitItem);
            return;
        }
    }

    private void PickupItem(DefaultPickupItem item)
    {
        DefaultHolderItem oldHolder = item.GetComponentInParent<DefaultHolderItem>();
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

    private void PutItemInHolder(DefaultHolderItem holder)
    {
        _playerStates.CurrentItem.MoveToTarget(holder.ItemPosition, false);

        holder.HasItem = true;
        holder.ItemInHolder = _playerStates.CurrentItem;

        _playerStates.CurrentItem = null;
    }

    private void OpenItem(DefaultOpenItem openItem)
    {
        openItem.TryUnlock(_playerStates.CurrentItem);
    }

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