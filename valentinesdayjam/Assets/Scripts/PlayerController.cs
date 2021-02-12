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


    private bool facingLeft = false;
    private bool jumping = false;
    private List<Collider2D> currentlyCringing = new List<Collider2D>();


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
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float newHorizontalVelocity;

        if (horizontal == 0)
            newHorizontalVelocity = 0;
        else //Instead of capping to max speed (ruins trap boosts), just don't add the input part above max speed
            newHorizontalVelocity = rb.velocity.x + (Mathf.Abs(rb.velocity.x) > maxWalkSpeed ? 0 : horizontal * acceleration);

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
                    Debug.DrawLine(transform.position, enemy.transform.position);
                    float distance = (transform.position - enemy.transform.position).magnitude * 8; //Unity distances are very small
                    Debug.Log(distance);
                    float gravity = cringeMultiplier / (Mathf.Pow(distance,2));
                    Vector2 direction = (transform.position - enemy.transform.position).normalized * gravity;

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
