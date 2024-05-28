using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 1)]
public class DialogueDataScriptableObject : ScriptableObject
{
    public List<DialogueData> dialogues;
}
