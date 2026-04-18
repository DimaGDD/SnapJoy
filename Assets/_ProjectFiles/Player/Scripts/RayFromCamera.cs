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
        if (_playerStates.IsViewItem)
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

        DefaultItem defaultObject = hit.collider.GetComponent<DefaultItem>();
        DefaultItemHolder defaultItemHolder = hit.collider.GetComponent<DefaultItemHolder>();

        string keyName = _inputBinds.Interact.action.GetBindingDisplayString();

        bool shouldShowPanel = false;
        string textToShow = "";

        if (defaultItemHolder != null && _playerStates.CurrentItem != null)
        {
            shouldShowPanel = true;
            textToShow = defaultItemHolder.InteractText;
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
