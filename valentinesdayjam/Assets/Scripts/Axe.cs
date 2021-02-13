using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public int rotationArc;
    public float rotationSpeed;
    public bool initialClockwise;

    private bool clockwise;
    private float startAngle;
    private float endAngle;

    private int totalRotations;
    private int rotations = 0;

    private Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        clockwise = initialClockwise;
        trans = GetComponent<Transform>();
        startAngle = trans.rotation.eulerAngles.z;
        endAngle = startAngle + rotationArc;
        Debug.Log("Start Angle: " + startAngle + "End Angle: " + endAngle);
        
        totalRotations = (int)(rotationArc / rotationSpeed); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotations++;
        
        if (rotations >= totalRotations)
        {
            clockwise = !clockwise;
            rotations = 0;
        }
     
        Quaternion zrot = Quaternion.Euler(0.0f, 0.0f, (rotationSpeed * (clockwise ? -1 : 1)));
        trans.localRotation *= zrot;
//        Debug.Log(currentEulerAngles.z);
    }
}
