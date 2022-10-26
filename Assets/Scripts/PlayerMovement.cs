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

    [HideInInspector]
    public bool canMove;

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

            if(movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            if(movement.x == 1)
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            if(movement.x == -1)
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            if(movement.y == 1)
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            if(movement.y == -1)
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);

            body.MovePosition(body.position + movement * runSpeed * Time.fixedDeltaTime);
        }
    }
}
