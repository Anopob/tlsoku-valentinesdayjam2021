﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float acceleration = 0.3f;
    public float maxWalkSpeed = 2f;
    
    private bool facingLeft = false;
    
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {        
        anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
        sp = GetComponent<SpriteRenderer> ();
   }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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
            flipSprite();
        }
        
        rb.velocity = new Vector2 (newHorizontalVelocity, rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
    }
    
    void flipSprite()
    {
        facingLeft = !facingLeft;
        sp.flipX = facingLeft;
    }
}
