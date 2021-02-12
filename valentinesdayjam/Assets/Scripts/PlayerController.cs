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

    private bool facingLeft = false;
    private bool jumping = false;

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
        else if (horizontal < 0)
            newHorizontalVelocity = Mathf.Max(maxWalkSpeed * -1, rb.velocity.x + horizontal * acceleration);
        else
            newHorizontalVelocity = Mathf.Min(maxWalkSpeed, rb.velocity.x + horizontal * acceleration);

        if ((newHorizontalVelocity < 0) != facingLeft)
        {
            FlipSprite();
        }

        rb.velocity = new Vector2(newHorizontalVelocity, rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }

    void FixedUpdate()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, CRINGE_DISTANCE);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Trap"))
            {
                Debug.DrawLine(transform.position, enemy.transform.position);
                Vector2 direction = (transform.position - enemy.transform.position).normalized;
                rb.AddForce(-direction);
                //rb.velocity = new Vector2(rb.velocity.x + direction.x * Time.deltaTime, rb.velocity.y + direction.y * Time.deltaTime);
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
