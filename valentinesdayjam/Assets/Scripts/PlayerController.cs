using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using Assets.Scripts;

public class PlayerController : MonoBehaviour
{
    private const float CRINGE_DISTANCE = 2f;
    private const float ABSOLUTE_SPEED_CAP = 6f;
    public float acceleration;
    public float maxWalkSpeed;
    public float jumpSpeed;
    public float cringeMultiplier;
    public float characterFriction;

    public AudioClip jumpSound;
    public AudioClip deathSound;

    private bool facingLeft;
    private bool jumping;
    private List<Collider2D> currentlyCringing = new List<Collider2D>();
    private Vector2 velocity;
    private Vector3 startingPosition;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private BoxCollider2D playerCollider;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerCollider = Array.Find(GetComponents<BoxCollider2D>(), b => !b.isTrigger);
        startingPosition = transform.position;
        Restart();
    }
    
    // Sets up some initial variables, can be used after death
    void Restart()
    {
        transform.position = startingPosition;
        facingLeft = false;
        jumping = false;
        velocity = new Vector2(0,0);
        sp.flipX = facingLeft;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<SceneCalculator>().GoToMainMenu();
        }

        if (Input.GetButtonDown("Jump") && !jumping)
        {
            PerformJumpingAnimations();
            audioSource.PlayOneShot(jumpSound);
            velocity = new Vector2(velocity.x, jumpSpeed);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float newHorizontalVelocity;

        if (horizontal == 0)
            newHorizontalVelocity = 0;
        else //Instead of capping to max speed (ruins trap boosts), just don't add the input part above max speed
            newHorizontalVelocity = horizontal;//rb.velocity.x + (Mathf.Abs(rb.velocity.x) > maxWalkSpeed ? 0 : horizontal * acceleration);

        if (newHorizontalVelocity != 0 &&
            (newHorizontalVelocity < 0) != facingLeft)
        {
            FlipSprite();
        }

        _movementForce = new Vector2(newHorizontalVelocity, rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    private Vector2 _movementForce;

    void FixedUpdate()
    {
        //  OldFixedUpdate();



        // Horizontal Acceleration.  Caused by player's input.  Set to 0 if maxWalkSpeed is already reached in that direction.
        Vector2 horizontalAcceleration; 
        float horizontalToApply = _movementForce.x * acceleration;
        
        // When acceleration is in the same direction as already capped velocity
        if (Math.Abs(velocity.x) > maxWalkSpeed && (velocity.x * horizontalToApply > 0)) 
            horizontalToApply = 0;

        horizontalAcceleration = new Vector2(horizontalToApply, 0);

        //if (Math.Abs(rb.velocity.x) < maxWalkSpeed)
        //    horizontalAcceleration = new Vector2(_movementForce.x, 0);
        //else
        //    horizontalAcceleration = Vector2.zero;
        
        // Horizontal Friction
        float frictionToApply = Mathf.Min(Mathf.Abs(velocity.x), characterFriction) * (velocity.x > 0 ? -1 : 1); // Friction in opposite direction of movement
        
        if (horizontalAcceleration.x != 0 || jumping) //Only apply friction when there is no player controlled movement on the ground
            frictionToApply = 0;
        if (Mathf.Abs(velocity.x) < 0.01)
            frictionToApply = 0;
        

        Vector2 frictionAcceleration = new Vector2(frictionToApply, 0);
        
        // Gravity Acceleration
        Vector2 gravityAcceleration = new Vector2(0, (jumping ? -0.2f : 0));	
        
        // Cringe Acceleration
        Vector2 cringeAcceleration = new Vector2(0,0);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, CRINGE_DISTANCE);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Trap"))
            {
                if (currentlyCringing.Contains(enemy))
                {
                    
                }
                else
                {
                    Debug.DrawLine(transform.position, enemy.bounds.center);
                    float distance = (transform.position - enemy.bounds.center).magnitude * 6; //Unity distances are very small
                    float cringeGravity = cringeMultiplier / (Mathf.Pow(distance,2));
                    Vector2 direction = (transform.position - enemy.bounds.center).normalized * cringeGravity;
                    cringeAcceleration += direction;
                    
                    /*
                    if (rb.velocity.x > 0)
                        rb.velocity = new Vector2(Math.Max(0, rb.velocity.x + direction.x), rb.velocity.y);
                    else if (rb.velocity.x < 0)
                        rb.velocity = new Vector2(Math.Min(0, rb.velocity.x + direction.x), rb.velocity.y);

                    if (rb.velocity.y > 0)
                        rb.velocity = new Vector2(rb.velocity.x, Math.Max(0, rb.velocity.y + direction.y));
                    else if (rb.velocity.y < 0)
                        rb.velocity = new Vector2(rb.velocity.x, Math.Min(0, rb.velocity.y + direction.y));
                    */
                    //currentlyCringing.Add(enemy);
                }

            }      
        }

                
        // Cringing cannot pick you up off the ground
        if (!jumping)
            cringeAcceleration = new Vector2(cringeAcceleration.x, 0);  

        // Apply acceleration to velocity
        velocity = velocity + horizontalAcceleration + frictionAcceleration + gravityAcceleration + cringeAcceleration;
        
        // Cap X and Y to prevent phasing through walls
        float cappedX = 0;
        float cappedY = 0;
        
        if (velocity.x > 0)
            cappedX = (Math.Min(velocity.x, ABSOLUTE_SPEED_CAP));
        else if (velocity.x < 0)
            cappedX = (Math.Max(velocity.x, -ABSOLUTE_SPEED_CAP));

        if (velocity.y > 0)
            cappedY = (Math.Min(velocity.y, ABSOLUTE_SPEED_CAP));
        else if (velocity.y < 0)
            cappedY = (Math.Max(velocity.y, -ABSOLUTE_SPEED_CAP));

        velocity = new Vector2(cappedX, cappedY);

        //Debug.Log("velocity " + velocity.x + " friction " + frictionToApply);

        if (!jumping)	
            cappedY = 0;

        // Failsafe for falling off the bottom of the screen
        if (transform.position.y < -12)
            KillPlayer();

        // Apply velocity to position
        //transform.position = new Vector3(transform.position.x + velocity.x, transform.position.y + velocity.y, transform.position.z);
        rb.velocity = velocity;        
    }

    void FlipSprite()
    {
        facingLeft = !facingLeft;
        sp.flipX = facingLeft;
    }

    void PerformJumpingAnimations()
    {
        anim.ResetTrigger("landed");
        anim.SetTrigger("jumped");
        jumping = true;
    }

    void PerformLandingAnimations()
    {
        jumping = false;
        anim.ResetTrigger("jumped");
        anim.SetTrigger("landed");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other);
        if (other != playerCollider)
            PerformLandingAnimations();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other != playerCollider)
            PerformJumpingAnimations();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Trap"))
            KillPlayer();
        //Door collision
//        level.Restart();
    }
    
    void KillPlayer(){
        audioSource.PlayOneShot(deathSound);
        // Play audio/visual effect
        Restart();
    }
}
