using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{

    [HideInInspector]
    public bool inSights;
    [HideInInspector]
    public bool isPlayerCaught;
    private bool chasing;

    private Transform player;

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

        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inSights && !isPlayerCaught)
        {
            chasing = true;
            transform.position = Vector3.MoveTowards(transform.position, player.position, killerSpeed * Time.fixedDeltaTime);
            //transform.LookAt(player); only works 3d
        }
        else chasing = false;

        if(chasing) killerAnimator.SetBool("chasing", true);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag.Equals("Player"))
        isPlayerCaught = true;
    }
}
