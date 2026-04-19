using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DefaultNPC : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private string _name = "NPC";
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private DialogueGraph _dialogueGraph;
    [SerializeField] private List<DefaultPickupItem> _questItems = new List<DefaultPickupItem>();

    private DefaultPickupItem _selectedQuestItem;

    private bool _isQuestTaken = false;
    private bool _isQuestCompleted = false;

    public bool IsQuestTaken
    {
        get { return _isQuestTaken; }
        set { _isQuestTaken = value; }
    }

    public bool IsQuestCompleted
    {
        get { return  _isQuestCompleted; }
    }

    public string Name
    {
        get { return _name; }
    }

    public ItemInteractType InteractType
    {
        get { return ItemInteractType.Talk; }
    }

    public string InteractText
    {
        get { return _interactTextConfig.GetText(InteractType); }
    }

    private void Awake()
    {
        ChooseQuestItem();
    }

    public void InteractWithPlayer(States playerStates, InputActionReference skipButton, DefaultPickupItem itemInHand)
    {
        if (!_isQuestTaken)
            DialogueManager.Instance.StartDialogue(_dialogueGraph, playerStates, skipButton, this, _selectedQuestItem);
        else
        {
            if (itemInHand != null && itemInHand == _selectedQuestItem)
            {
                UIManager.Instance.UpdateQuestPopup(true);
                playerStates.CurrentItem = null;
                Destroy(itemInHand.gameObject);
                _isQuestCompleted = true;
            }
        }
    }

    private void ChooseQuestItem()
    {
        int randomIndex = Random.Range(0, _questItems.Count);
        _selectedQuestItem = _questItems[randomIndex];
    }
}
