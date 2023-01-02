using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to trigger the dialogue
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void Start()
    {
        TriggerDialogue();
    }
}
