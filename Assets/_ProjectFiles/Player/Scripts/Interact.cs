using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SO_Interact _interactParametrs;

    [Header("Other")]
    [SerializeField] private Transform _itemViewPosition;
    [SerializeField] private Transform _itemPosition;

    private Camera _mainCamera;
    private float _interactRange;
    private LayerMask _interactLayerMask;

    private States _playerStates;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _interactRange = _interactParametrs.InteractRange;
        _interactLayerMask = _interactParametrs.InteractLayerMask;

        _playerStates = GetComponent<States>();
    }

    public void TryInteract()
    {
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactLayerMask))
        {
            DefaultItem defaultItem = hit.collider.GetComponent<DefaultItem>();

            if (defaultItem != null)
            {
                if (!_playerStates.IsViewItem && defaultItem.InteractType == ItemInteractType.Pickup)
                {
                    defaultItem.View(_itemViewPosition);
                    _playerStates.IsViewItem = true;

                    UIManager.Instance.ShowViewPanel(defaultItem.Description);
                    UIManager.Instance.ShowCursor();
                }
            }
        }
    }
}