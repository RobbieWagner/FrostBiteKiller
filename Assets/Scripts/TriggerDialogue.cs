using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField]
    private TextAsset textJSON;
    private DialogueInteractable.Dialogue dialogue;

    [SerializeField]
    private DialogueManager dialogueManager;

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = JsonUtility.FromJson<DialogueInteractable.DialogueD>(textJSON.text).dialogue;
        dialogueManager = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();

        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            dialogueManager.StartDialogue(dialogue);
            playerAnimator.SetBool("moving", false);
        }
    }
}
