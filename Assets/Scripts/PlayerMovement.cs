using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    Animator animator;

    Vector2 movement;

    [SerializeField]
    private float runSpeed = 5.0f;
    float moveLimiter = 0.7f;

    [HideInInspector]
    public bool canMove;

    public AudioSource playerFootstepSounds;

    void Start()
    {
        movement = new Vector2(0,0);

        canMove = true;
    }

    void Update()
    {
        if(canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); 
            movement.y = Input.GetAxisRaw("Vertical"); 

            if((movement.x != 0 || movement.y != 0) && canMove)
            {
                animator.SetBool("moving", true);
                if(!playerFootstepSounds.isPlaying) playerFootstepSounds.Play();
            }
            else
            {
                animator.SetBool("moving", false);
                if(playerFootstepSounds.isPlaying) playerFootstepSounds.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            if(movement.x == 1)
            {
                if(movement.y == 1)gameObject.transform.rotation = Quaternion.Euler(0, 0, -45);
                else if(movement.y == -1) gameObject.transform.rotation = Quaternion.Euler(0, 0, -135);
                else gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if(movement.x == -1)
            {
                if(movement.y == 1)gameObject.transform.rotation = Quaternion.Euler(0, 0, 45);
                else if(movement.y == -1) gameObject.transform.rotation = Quaternion.Euler(0, 0, 135);
                else gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if(movement.y == 1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(movement.y == -1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            if(movement.x != 0 && movement.y != 0)
            {
                movement.x *= moveLimiter;
                movement.y *= moveLimiter;
            }

            body.MovePosition(body.position + movement * runSpeed * Time.fixedDeltaTime);
        }
    }
}
