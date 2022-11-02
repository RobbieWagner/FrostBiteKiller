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
    private float initialPlayerSpeed = 5.0f;
    [SerializeField]
    private float currentPlayerSpeed;
    float moveLimiter = 0.7f;

    [HideInInspector]
    public bool canMove;

    public AudioSource playerFootstepSounds;

    bool startsRunning;
    bool running;
    [SerializeField]
    private int maxStamina;
    private int currentStamina;
    [SerializeField]
    private float staminaLossRate;
    [SerializeField]
    private float staminaGainRate;

    void Start()
    {
        movement = new Vector2(0,0);
        canMove = true;
        currentPlayerSpeed = initialPlayerSpeed;
        currentStamina = maxStamina;
        startsRunning = false;
        running = false;

        currentPlayerSpeed = initialPlayerSpeed;
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

        
        if(Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero && !running && currentStamina > maxStamina/4)
        { 
            startsRunning = true;
        }

        if(Input.GetKeyUp(KeyCode.Space) && running)
        {
            running = false;
            startsRunning = false;
            currentPlayerSpeed = initialPlayerSpeed;
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

            if(startsRunning && !running)
            {
                currentPlayerSpeed = initialPlayerSpeed * 2.5f;
                running = true;
                startsRunning = false;
                StartCoroutine(CharacterRun());
            }

            if(movement.x != 0 && movement.y != 0)
            {
                movement.x *= moveLimiter;
                movement.y *= moveLimiter;
            }

            body.MovePosition(body.position + movement * currentPlayerSpeed * Time.fixedDeltaTime);
        }
    }

    public IEnumerator CharacterRun()
    {
        currentPlayerSpeed = initialPlayerSpeed * 1.5f;
        StartCoroutine(ReduceStamina());
        while(Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            yield return null;
        }

        running = false;

        StartCoroutine(ReplenishStamina());
        StopCoroutine(CharacterRun());
    }

    public IEnumerator ReduceStamina()
    {
        while(Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            currentStamina--;
            yield return new WaitForSeconds(staminaLossRate);
        }
        StopCoroutine(ReduceStamina());
    }

    public IEnumerator ReplenishStamina()
    {
        currentPlayerSpeed = initialPlayerSpeed;
        while(Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(staminaGainRate);
        while(!startsRunning && !running && currentStamina < maxStamina)
        {
            currentStamina++;
            yield return new WaitForSeconds(1f);
        }
        StopCoroutine(ReplenishStamina());
    }
}
