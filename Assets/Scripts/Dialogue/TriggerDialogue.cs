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

    private GameObject player;
    private Animator playerAnimator;
    private PlayerMovement playerM;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = JsonUtility.FromJson<DialogueInteractable.DialogueD>(textJSON.text).dialogue;
        dialogueManager = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();

        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerM = player.GetComponent<PlayerMovement>();

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            dialogueManager.StartDialogue(dialogue);
            playerAnimator.SetBool("moving", false);
            playerM.playerFootstepSounds.Stop();
        }
    }
}
