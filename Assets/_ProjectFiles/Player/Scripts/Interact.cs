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
        InputHandler.OnInteractPressed += OnInteract;
        InputHandler.OnDoubleInteractPressed += OnDoubleInteract;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        _interactRange = _interactParametrs.InteractRange;
        _interactLayerMask = _interactParametrs.InteractLayerMask;

        _playerStates = GetComponent<States>();
    }

    private void OnInteract()
    {
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactLayerMask))
        {
            DefaultItem defaultItem = hit.collider.GetComponent<DefaultItem>();

            if (defaultItem != null)
            {
                if (!_playerStates.IsViewItem && defaultItem.InteractType == ItemInteractType.Pickup && _playerStates.CurrentItem == null)
                {
                    defaultItem.MoveToTarget(_itemViewPosition, true);
                    _playerStates.IsViewItem = true;
                    _playerStates.CurrentItem = defaultItem;

                    UIManager.Instance.ShowViewPanel(defaultItem.Description);
                    UIManager.Instance.ShowCursor();
                }
            }
        }
    }

    private void OnDoubleInteract()
    {
        _playerStates.CurrentItem.MoveToTarget(_itemHandPosition, false);
        _playerStates.IsViewItem = false;

        UIManager.Instance.HideViewPanel();
        UIManager.Instance.HideCursor();
    }

    private void OnDisable ()
    {
        InputHandler.OnInteractPressed -= OnInteract;
        InputHandler.OnDoubleInteractPressed -= OnDoubleInteract;
    }
}