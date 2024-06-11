using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public DialogueDataScriptableObject dialogueData;



    void Start()
    {
        //Call this function when u need.
        StartDialogue();
    }

    public void StartDialogue()
    {
        StartCoroutine(PlayDialogues());
    }
    IEnumerator PlayDialogues()
    {
        foreach (DialogueData dialogue in dialogueData.dialogues)
        {
            dialogueText.text = dialogue.content;
            yield return new WaitForSeconds(dialogue.displayTime);
        }
        dialogueText.text = ""; // Clear the dialogue after all dialogues are shown
    }
}
