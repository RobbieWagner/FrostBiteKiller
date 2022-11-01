using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{

    [HideInInspector]
    public bool inSights;
    [HideInInspector]
    public bool isPlayerCaught;
    [HideInInspector]
    public bool chasing;

    private bool canChase;
    private bool waitingToChase;

    [SerializeField]
    private float turnTime;

    private GameObject playerGO;
    private PlayerMovement playerM;
    private Transform playerT;
    private Animator playerA;

    [SerializeField]
    private float killerSpeed;

    [SerializeField]
    private Animator killerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        inSights = false;
        isPlayerCaught = false;
        chasing = false;

        playerGO = GameObject.Find("Player");
        playerM = playerGO.GetComponent<PlayerMovement>();
        playerT = playerGO.transform;
        playerA = playerGO.GetComponent<Animator>();

        canChase = false;
        waitingToChase = false;
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
                transform.position = Vector3.MoveTowards(transform.position, playerT.position, killerSpeed * Time.fixedDeltaTime);
                
                Vector2 direction = playerT.position - transform.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            }
        }
        else chasing = false;

        if(chasing) killerAnimator.SetBool("chasing", true);
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
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 60);
        yield return new WaitForSeconds(turnTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 120);
        yield return new WaitForSeconds(turnTime);

        canChase = true;
        playerM.canMove = true;

        StopCoroutine(NoticePlayer());
    }
}
