using UnityEngine;

[CreateAssetMenu(fileName = "InteractTextConfig", menuName = "Scriptable Objects/InteractTextConfig")]
public class InteractTextConfig : ScriptableObject
{
    [System.Serializable]
    public class InteractTextPair
    {
        public ItemInteractType Type;
        [TextArea(2, 4)]
        public string Text;
    }

    public InteractTextPair[] Interactions;

    public string GetText(ItemInteractType type)
    {
        foreach (var pair in Interactions)
        {
            if (pair.Type == type)
                return pair.Text;
        }
        return "No text defined";
    }
}
