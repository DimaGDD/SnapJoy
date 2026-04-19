using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DefaultNPC : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private string _name = "NPC";
    [SerializeField] private InteractTextConfig _interactTextConfig;
    [SerializeField] private DialogueGraph _dialogueGraph;
    [SerializeField] private List<GameObject> _questItems = new List<GameObject>();

    private GameObject _selectedQuestItem;

    private bool _isQuestTaken = false;

    public bool IsQuestTaken
    {
        get { return _isQuestTaken; }
        set { _isQuestTaken = value; }
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
        //ChooseQuestItem();
    }

    public void InteractWithPlayer(States playerStates, InputActionReference skipButton)
    {
        if (!_isQuestTaken)
            DialogueManager.Instance.StartDialogue(_dialogueGraph, playerStates, skipButton, this);
        else
        {

        }
    }

    private void ChooseQuestItem()
    {
        int randomIndex = Random.Range(0, _questItems.Count);
        _selectedQuestItem = _questItems[randomIndex];
    }
}
