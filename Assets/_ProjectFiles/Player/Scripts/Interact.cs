using UnityEngine;
using UnityEngine.Rendering.UI;

public class Interact : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SO_Interact _interactParametrs;
    [SerializeField] SO_InputBinds _inputBinds;

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
        InputHandler.OnInteractHold += OnHoldInteractAction;
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
        DefaultNPC hitNPC = hit.collider.GetComponent<DefaultNPC>();

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

        if (hitNPC != null)
        {
            InteractWithNPC(hitNPC);
        }

        if (hitItem != null && _playerStates.CurrentItem == null)
        {
            PickupItem(hitItem);
            return;
        }
    }

    private void InteractWithNPC(DefaultNPC hitNPC)
    {
        hitNPC.InteractWithPlayer(_playerStates, _inputBinds.SkipDialogue, _playerStates.CurrentItem);
    }

    private void OnHoldInteractAction(float time)
    {
        if (_playerStates.IsViewItem) return;

        bool shouldCancel = time < 0;

        if (!shouldCancel)
        {
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactLayerMask))
            {
                DefaultHoldItem holdItem = hit.collider.GetComponent<DefaultHoldItem>();

                if (holdItem != null)
                {
                    _playerStates.CurentHoldItem = holdItem;
                    holdItem.HoldItem();
                    return;
                }
            }

            shouldCancel = true;
        }

        if (shouldCancel && _playerStates.CurentHoldItem != null)
        {
            if (_playerStates.CurentHoldItem != null)
            {
                _playerStates.CurentHoldItem.CanceledHold();
                _playerStates.CurentHoldItem = null;
            }
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

        UIManager.Instance.HideDot();
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

        UIManager.Instance.ShowDot();
    }

    private void OnDisable()
    {
        InputHandler.OnInteractPressed -= OnInteractAction;
        InputHandler.OnDoubleInteractPressed -= OnDoubleInteractAction;
    }
}