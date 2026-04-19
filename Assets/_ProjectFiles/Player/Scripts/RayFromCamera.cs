using UnityEngine;
using UnityEngine.InputSystem;

public class RayFromCamera : MonoBehaviour
{
    [Header("Parametrs")]
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private SO_InputBinds _inputBinds;

    private Camera _mainCamera;

    private States _playerStates;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _playerStates = GetComponent<States>();
    }

    private void Update()
    {
        if (_playerStates.IsViewItem && _playerStates.IsInDialogue)
        {
            UIManager.Instance.HideInteractPanel();
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        if (!Physics.Raycast(ray, out RaycastHit hit, _maxDistance))
        {
            UIManager.Instance.HideInteractPanel();
            return;
        }

        DefaultPickupItem defaultObject = hit.collider.GetComponent<DefaultPickupItem>();
        DefaultHolderItem defaultItemHolder = hit.collider.GetComponent<DefaultHolderItem>();
        DefaultOpenItem defaultOpenItem = hit.collider.GetComponent<DefaultOpenItem>();
        DefaultHoldItem defaultHoldItem = hit.collider.GetComponent<DefaultHoldItem>();
        DefaultNPC defaultNPC = hit.collider.GetComponent<DefaultNPC>();

        string keyName = _inputBinds.Interact.action.GetBindingDisplayString();

        bool shouldShowPanel = false;
        string textToShow = "";

        if (defaultItemHolder != null && _playerStates.CurrentItem != null)
        {
            shouldShowPanel = true;
            textToShow = defaultItemHolder.InteractText;
        }
        else if (defaultOpenItem != null && !defaultOpenItem.IsUnlock)
        {
            shouldShowPanel = true;
            textToShow = defaultOpenItem.InteractText;
        }
        else if (defaultHoldItem != null)
        {
            shouldShowPanel = true;
            textToShow = defaultHoldItem.InteractText;
        }
        else if (defaultNPC != null)
        {
            shouldShowPanel = true;
            textToShow = defaultNPC.InteractText;
        }
        else if (defaultObject != null && defaultObject != _playerStates.CurrentItem)
        {
            shouldShowPanel = true;
            textToShow = defaultObject.InteractText;
        }

        if (shouldShowPanel)
        {
            UIManager.Instance.ShowInteractPanel(keyName, textToShow);
        }
        else
        {
            UIManager.Instance.HideInteractPanel();
        }
    }
}
