using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public double shotDelay;
    public Dagger dagger;
    public double speed;

    private int framesDelay;
    private int sinceLastShot = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        framesDelay = (int)(60 * shotDelay);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sinceLastShot == framesDelay)
        {
           Shoot();
           sinceLastShot = 0;
        }
        sinceLastShot++;
    }
    
    void Shoot()
    {
        Dagger d = Instantiate(dagger, transform.position, Quaternion.identity);
        d.angle = transform.rotation.eulerAngles.z + 90;
        d.speed = (float)speed;
//        d.angle = 270;
    }
}
