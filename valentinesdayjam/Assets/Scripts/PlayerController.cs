using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float CRINGE_DISTANCE = 2f;

    public float acceleration;
    public float maxWalkSpeed;
    public float jumpSpeed;
    public float cringeMultiplier;
    public float characterFriction;

    private bool facingLeft = false;
    private bool jumping = false;
    private List<Collider2D> currentlyCringing = new List<Collider2D>();
    private Vector2 velocity = new Vector2(0,0);

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private BoxCollider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = Array.Find(GetComponents<BoxCollider2D>(), b => !b.isTrigger);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !jumping)
        {
            PerformJumpingAnimations();
            Debug.Log("jumped");
            //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
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



        // Horizontal Acceleration
        Vector2 horizontalAcceleration;
        horizontalAcceleration = new Vector2(_movementForce.x * acceleration, 0);
        //if (Math.Abs(rb.velocity.x) < maxWalkSpeed)
        //    horizontalAcceleration = new Vector2(_movementForce.x, 0);
        //else
        //    horizontalAcceleration = Vector2.zero;
        
        // Horizontal Friction
        float frictionToApply = Mathf.Min(Mathf.Abs(velocity.x), characterFriction) * (velocity.x > 0 ? -1 : 1); // Friction in opposite direction of movement
        
        if (horizontalAcceleration.x != 0) //Only apply friction when there is no player controlled movement
            frictionToApply = 0;
        
        

        Vector2 frictionAcceleration = new Vector2(frictionToApply, 0);
        
        // Gravity Acceleration
        Vector2 gravityAcceleration = new Vector2(0,-0.2f);
       
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
                    float distance = (transform.position - enemy.bounds.center).magnitude * 8; //Unity distances are very small
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

        // Apply acceleration to velocity
        
        
        velocity = velocity + horizontalAcceleration + frictionAcceleration + gravityAcceleration + cringeAcceleration;
        // temp cap x
        if (velocity.x > 0)
            velocity = new Vector2(Math.Min(velocity.x, maxWalkSpeed), velocity.y);
        else if (velocity.x < 0)
            velocity = new Vector2(Math.Max(velocity.x, -maxWalkSpeed), velocity.y);

        Debug.Log("velocity " + velocity.x + " friction " + frictionToApply);


        // Apply velocity to position
        //transform.position = new Vector3(transform.position.x + velocity.x, transform.position.y + velocity.y, transform.position.z);
        rb.velocity = velocity;
    }

        
    void OldFixedUpdate()
    {
        rb.velocity = _movementForce;

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
                    float distance = (transform.position - enemy.bounds.center).magnitude * 8; //Unity distances are very small
                    Debug.Log(distance);
                    float gravity = cringeMultiplier / (Mathf.Pow(distance,2));
                    Vector2 direction = (transform.position - enemy.bounds.center).normalized * gravity;

                    //Vector2 swappedDirection = direction;//new Vector2(direction.y * gravity, direction.x * gravity);

                    if (rb.velocity.x > 0)
                        rb.velocity = new Vector2(Math.Max(0, rb.velocity.x + direction.x), rb.velocity.y);
                    else if (rb.velocity.x < 0)
                        rb.velocity = new Vector2(Math.Min(0, rb.velocity.x + direction.x), rb.velocity.y);

                    if (rb.velocity.y > 0)
                        rb.velocity = new Vector2(rb.velocity.x, Math.Max(0, rb.velocity.y + direction.y));
                    else if (rb.velocity.y < 0)
                        rb.velocity = new Vector2(rb.velocity.x, Math.Min(0, rb.velocity.y + direction.y));
                    
                    //currentlyCringing.Add(enemy);
                }
            }      
        }
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
}
