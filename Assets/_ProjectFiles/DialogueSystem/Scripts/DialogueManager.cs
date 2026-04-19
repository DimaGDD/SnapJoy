using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup _panelRoot;
    [SerializeField] private TextMeshProUGUI _npcText;
    [SerializeField] private Transform _optionsParent;
    [SerializeField] private GameObject _optionPrefab;

    [Header("Quest Notification")]
    [SerializeField] private GameObject _questPopup;

    [Header("ToolTip")]
    [SerializeField] private CanvasGroup _toolTip;

    private DialogueGraph _currentGraph;
    private DefaultNPC _npc;
    private InputActionReference _skipButton;
    private TextMeshProUGUI _toolTipText;

    private bool _waitForClick = false;
    private DefaultPickupItem _questItem;

    private States _playerStates;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        ClosePanel();

        _toolTipText = _toolTip.GetComponent<TextMeshProUGUI>();
    }

    public void StartDialogue(DialogueGraph graph, States playerStates, InputActionReference skipButton, DefaultNPC npc, DefaultPickupItem questItem)
    {
        InputHandler.OnSkipDialogue += SkipDialogue;

        _playerStates = playerStates;
        _npc = npc;
        _currentGraph = graph;
        _skipButton = skipButton;
        _questItem = questItem;

        OpenPanel();
        ShowNode("0");

        _playerStates.IsInDialogue = true;

        UIManager.Instance.ShowCursor();
        UIManager.Instance.HideDot();
    }

    private void ShowNode(string currentNode)
    {
        if (_currentGraph == null) return;

        DialogueNode node = _currentGraph.GetNode(currentNode);

        if (node == null)
        {
            EndDialogue();
            return;
        }

        _npcText.text = $"{_npc.Name}: {node.SpeakerText}";

        if (node.EndDialogue)
        {
            ClearButtons(node);
            StartCoroutine(WaitForClick(null));
            return;
        }

        if (!string.IsNullOrEmpty(node.ReturnToNode))
        {
            ClearButtons(node);
            StartCoroutine(WaitForClick(node.ReturnToNode));
            return;
        }

        ClearButtons(node);

        foreach (var option in node.Options)
        {
            GameObject btnObj = Instantiate(_optionPrefab, _optionsParent);

            if (option.QuestChain)
                btnObj.GetComponent<Image>().color = Color.yellow;

            var btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = option.ResponseText;

            Button btn = btnObj.GetComponent<Button>();

            string nextID = option.NextNodeID;
            string questName = option.QuestName;

            btn.onClick.AddListener(() => HandleOptionClick(node.NodeID, nextID, questName));
        }
    }

    private void ClearButtons(DialogueNode node)
    {
        foreach (Transform child in _optionsParent)
            Destroy(child.gameObject);
    }    

    private void HandleOptionClick(string currentNodeID, string nextNodeID, string questName)
    {
        if (!string.IsNullOrEmpty(questName))
        {
            TriggerQuest(questName);
        }

        if (string.IsNullOrEmpty(nextNodeID))
        {
            EndDialogue();
        }
        else
        {
            ShowNode(nextNodeID);
        }
    }

    private IEnumerator WaitForClick(string nodeID)
    {
        _waitForClick = true;

        string buttonName = _skipButton.action.GetBindingDisplayString();
        _toolTipText.text = $"Нажмите [ {buttonName} ], чтобы продолжить...";

        _toolTip.alpha = 1;

        while (_waitForClick)
        {
            yield return null;
        }

        _toolTip.alpha = 0;

        if (nodeID == null)
            EndDialogue();
        else
            ShowNode(nodeID);
    }

    private void TriggerQuest(string questName)
    {
        _npc.IsQuestTaken = true;
        UIManager.Instance.ShowQuestPopup(questName, _questItem.name, false);
    }

    private void EndDialogue()
    {
        ClosePanel();

        InputHandler.OnSkipDialogue -= SkipDialogue;

        _playerStates.IsInDialogue = false;
        UIManager.Instance.HideCursor();

        UIManager.Instance.ShowDot();
    }

    private void OpenPanel()
    {
        _panelRoot.alpha = 1;
    }

    private void ClosePanel()
    {
        _panelRoot.alpha = 0;
    }

    private void SkipDialogue()
    {
        _waitForClick = false;
    }
}
