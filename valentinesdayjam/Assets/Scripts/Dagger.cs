using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public float speed;
    public float angle;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().rotation = Quaternion.Euler(0.0f, 0.0f,  angle + 90.0f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle  *  Mathf.Deg2Rad)) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}