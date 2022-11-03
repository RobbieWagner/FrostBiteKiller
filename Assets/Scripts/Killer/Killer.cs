using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Killer : MonoBehaviour
{
    [HideInInspector]
    public bool inSights;
    [HideInInspector]
    public bool canEnterSights;
    [HideInInspector]
    public bool isPlayerCaught;
    [HideInInspector]
    public bool chasing;

    private bool canChase;
    private bool waitingToChase;

    private TutorialCanvas tutorialC;
    [SerializeField]
    private Tutorial runTutorial;

    [SerializeField]
    private float turnTime;

    private GameObject playerGO;
    private PlayerMovement playerM;
    private Transform playerT;
    private Animator playerA;

    [SerializeField]
    private float killerSpeed;
    [SerializeField]
    private Rigidbody2D killerRB2D;

    [SerializeField]
    private Animator killerAnimator;

    [SerializeField]
    private GameObject invisibleBounds1;

    // Start is called before the first frame update
    void Start()
    {
        inSights = false;
        canEnterSights = true;
        isPlayerCaught = false;
        chasing = false;

        playerGO = GameObject.Find("Player");
        playerM = playerGO.GetComponent<PlayerMovement>();
        playerT = playerGO.transform;
        playerA = playerGO.GetComponent<Animator>();

        canChase = false;
        waitingToChase = false;

        tutorialC = GameObject.Find("TutorialCanvas").GetComponent<TutorialCanvas>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inSights && !isPlayerCaught)
        {
            if(!canChase && !waitingToChase)
            {
                waitingToChase = true;
                StartCoroutine(NoticePlayer());
            }
            else if(canChase)
            {
                chasing = true;
                
                Vector2 lookDirection = playerT.position - transform.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, lookDirection);

                Vector2 moveDirection = new Vector2(playerT.position.x - transform.position.x, playerT.position.y - transform.position.y);
                moveDirection.Normalize();
                killerRB2D.velocity = moveDirection * killerSpeed;
            }
        }
        else chasing = false;

        if(chasing)
        {
            canEnterSights = false;
            killerAnimator.SetBool("chasing", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag.Equals("Player"))
        isPlayerCaught = true;
    }

    private IEnumerator NoticePlayer()
    {
        playerM.canMove = false;
        playerT.position = new Vector3(playerT.position.x, playerT.position.y + 2, playerT.position.z);
        playerA.SetBool("moving", false);
        playerM.playerFootstepSounds.Stop();
        yield return new WaitForSeconds(turnTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90);
        yield return new WaitForSeconds(turnTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180);
        tutorialC.tmproText.text = runTutorial.GetText();
        yield return new WaitForSeconds(turnTime);

        canChase = true;
        playerM.canMove = true;

        if(invisibleBounds1.activeInHierarchy)
        {
            invisibleBounds1.SetActive(false);
        }

        StopCoroutine(NoticePlayer());
    }

    public IEnumerator CooldownChase()
    {
        if(tutorialC.tmproText.text.Equals(runTutorial.GetText())) tutorialC.tmproText.text = "";

        canEnterSights = false;

        yield return new WaitForSeconds(2f);

        canEnterSights = true;

        StopCoroutine(CooldownChase());
    }
}
