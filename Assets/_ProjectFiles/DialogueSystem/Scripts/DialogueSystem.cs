using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string NodeID;

    [TextArea(3, 5)]
    public string SpeakerText;
    public bool EndDialogue;
    public string ReturnToNode;
    public List<DialogueOption> Options = new List<DialogueOption>();
}

[Serializable]
public class DialogueOption
{
    [TextArea(1, 2)]
    public string ResponseText;
    public string NextNodeID;
    public string QuestName;
    public bool QuestChain;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue Graph")]
public class DialogueGraph : ScriptableObject
{
    public List<DialogueNode> Nodes = new List<DialogueNode>();

    public DialogueNode GetNode(string id)
    {
        return Nodes.Find(n => n.NodeID == id);
    }
}
