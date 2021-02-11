using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float timeSinceStart;
    
    private Animator anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceStart = 0;
        
        anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
   
   }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        // Basic animation loop test
        timeSinceStart++;
        if (timeSinceStart > 60)
        {
            anim.SetFloat("speed", 1);
        }
        
        if (timeSinceStart > 120)
        {
            anim.SetFloat("speed", 2);
        }
        
        if (timeSinceStart > 180)
        {
            anim.SetFloat("speed", 0);
            timeSinceStart = 0;
        }
    }
}
