using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("View Panel")]
    [SerializeField] private GameObject _viewPanel;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    [Header("Interact Panel")]
    [SerializeField] private GameObject _interactPanel;
    [SerializeField] private TextMeshProUGUI _interactAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleInventory(bool state)
    {
        _viewPanel.SetActive(state);
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowViewPanel(string description)
    {
        _itemDescription.text = description;
        _viewPanel.SetActive(true);
    }

    public void HideViewPanel()
    {
        _viewPanel.SetActive(false);
        _itemDescription.text = null;
    }

    public void ShowInteractPanel(string keyName, string action)
    {
        string toolTip = keyName + " - [ " + action + " ]";

        _interactAction.text = toolTip;
        _interactPanel.SetActive(true);
    }

    public void HideInteractPanel()
    {
        _interactPanel.SetActive(false);
        _interactAction.text = null;
    }
}
