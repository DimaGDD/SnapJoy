using UnityEngine;
using TMPro;
using System.Reflection.Emit;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("View Panel")]
    [SerializeField] private CanvasGroup _viewPanel;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    [Header("Interact Panel")]
    [SerializeField] private CanvasGroup _interactPanel;
    [SerializeField] private TextMeshProUGUI _interactAction;

    [Header("Interface")]
    [SerializeField] private CanvasGroup _dot;

    [Header("Quest Popup")]
    [SerializeField] private CanvasGroup _questPopupPanel;
    [SerializeField] private Text _questNameLabel;
    [SerializeField] private Toggle _questToggle;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleInventory(bool state)
    {
        _viewPanel.alpha = state ? 1 : 0;

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
        _viewPanel.alpha = 1;
    }

    public void HideViewPanel()
    {
        _viewPanel.alpha = 0;
        _itemDescription.text = null;
    }

    public void ShowInteractPanel(string keyName, string action)
    {
        string toolTip = $"{keyName} - [ {action} ]";

        _interactAction.text = toolTip;
        _interactPanel.alpha = 1;
    }

    public void HideInteractPanel()
    {
        _interactPanel.alpha = 0;
        _interactAction.text = null;
    }

    public void ShowDot()
    {
        _dot.alpha = 1;
    }

    public void HideDot()
    {
        _dot.alpha = 0;
    }

    public void ShowQuestPopup(string questName, string questItem, bool isComplete)
    {
        _questPopupPanel.alpha = 1;
        _questNameLabel.text = $"{questName} {questItem}";
        _questToggle.isOn = isComplete;
    }

    public void UpdateQuestPopup(bool isComplete)
    {
        _questToggle.isOn = isComplete;
    }
}
