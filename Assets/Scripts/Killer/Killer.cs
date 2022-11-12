using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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
    [HideInInspector]
    public bool playerNoticed;
    [HideInInspector]
    private bool pathSet;


    [SerializeField]
    private Transform goal;
    [SerializeField]
    private RND random;
    [SerializeField]
    private int yMin;
    [SerializeField]
    private int yMax;
    [SerializeField]
    private int xMin;
    [SerializeField]
    private int xMax;

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
    private NavMeshAgent killerNVA;

    [SerializeField]
    private Animator killerAnimator;
    [SerializeField]
    private Animator bloodA;

    [SerializeField]
    private GameObject invisibleBounds1;

    [SerializeField]
    private Canvas deathScreen;

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

        killerNVA.updateRotation = false;
        killerNVA.updateUpAxis = false;
        killerNVA.speed = killerSpeed;

        tutorialC = GameObject.Find("TutorialCanvas").GetComponent<TutorialCanvas>();

        deathScreen.enabled = false;

        pathSet = false;
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
                killerNVA.SetDestination(playerT.position);
            }
            //else Debug.Log(playerNoticed + " " + waitingToChase + " " + canChase);
        }
        else if(playerNoticed && !waitingToChase && !chasing)
        {
            FollowPath();

            if (!killerNVA.pathPending)
            {
                if (killerNVA.remainingDistance <= killerNVA.stoppingDistance)
                {
                    if (!killerNVA.hasPath || killerNVA.velocity.sqrMagnitude == 0f)
                    {
                        pathSet = false;
                    }
                }
            }
        }
        else
        {
            chasing = false;
        }

        if(chasing)
        {
            canEnterSights = false;
            killerAnimator.SetBool("chasing", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            isPlayerCaught = true;
            canChase = false;
            killerAnimator.SetBool("chasing", false);
            playerA.SetBool("moving", false);
            StartCoroutine(KillPlayer());
        }
    }

    private void FollowPath()
    {
        if(!pathSet)
        {
            float nextYPath = random.rnd.Next(yMin, yMax);
            float nextXPath = random.rnd.Next(xMin, xMax);
            goal.position = new Vector3(nextXPath, nextYPath, 0);
            
            pathSet = killerNVA.SetDestination(goal.position);
        }
    }

    public void ToggleKillerMovement(bool stop)
    {
        if(stop)
        {
            killerNVA.speed = 0;
        }
        else
        {
            killerNVA.speed = killerSpeed;
        }
    }

    private IEnumerator KillPlayer()
    {
        killerNVA.speed = 0;
        Camera.main.transform.position = new Vector3(playerT.position.x, playerT.position.y, -10);
        playerM.canMove = false;
        yield return new WaitForSeconds(.1f);
        killerAnimator.SetBool("killing", true);
        yield return new WaitForSeconds(.5f);
        deathScreen.enabled = true;
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Menu");

        StopCoroutine(KillPlayer());
    }

    private IEnumerator NoticePlayer()
    {
        playerM.canMove = false;
        playerT.position = new Vector3(playerT.position.x, playerT.position.y + 2, playerT.position.z);
        playerA.SetBool("moving", false);
        bloodA.SetBool("bleed", true);
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

        playerNoticed = true;
        waitingToChase = false;

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
