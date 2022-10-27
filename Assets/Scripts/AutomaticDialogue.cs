using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDialogue : Interactable
{

    [SerializeField]
    private TextAsset[] textJSON;

    DialogueInteractable dialogueI;

    DialogueManager dialogueM;
    [SerializeField]
    List<DialogueInteractable.Dialogue> dialogues;

    private bool hasInteracted;

    [SerializeField]
    private bool dialogueOnStart;

    [SerializeField]
    private Collider2D[] triggers;

    [SerializeField]
    private Animator[] speakerAnimators;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        playerCanInteract = false;

        for(int i = 0; i < textJSON.Length; i++)
        {
            dialogues.Add(JsonUtility.FromJson<DialogueInteractable.DialogueD>(textJSON[i].text).dialogue);
        }

        dialogueM = GameObject.Find("TextBoxCanvas").GetComponent<DialogueManager>();

        hasInteracted = false;


    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasInteracted)
        {
            hasInteracted = true;
            Interact();
            foreach(Collider2D collider in triggers)
            {
                collider.enabled = false;
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }

    protected override void Interact()
    {
        base.Interact();
        StartCoroutine(ReadDialogueList());
    }

    IEnumerator ReadDialogueList()
    {
        foreach(DialogueInteractable.Dialogue dialogue in dialogues)
        {
            dialogueM.StartDialogue(dialogue);
            yield return new WaitForSeconds(.25f);
            while(player.isReadingDialogue)
            {
                yield return null;
            }
            yield return new WaitForSeconds(.5f);

            if(dialogue.interDialogueAction != null && dialogue.interDialogueAction.Equals("FlipGhostAround"))
            {
                speakerAnimators[0].SetBool("FacingForward", true);
                yield return new WaitForSeconds(.75f);
            }
        }

        StopCoroutine(ReadDialogueList());
    }

    protected override void Update()
    {
    }
}
