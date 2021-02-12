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

    private Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        clockwise = initialClockwise;
        trans = GetComponent<Transform>();
        startAngle = trans.rotation.eulerAngles.z;
        endAngle = startAngle + rotationArc;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentEulerAngles = trans.rotation.eulerAngles;

        if (currentEulerAngles.z > endAngle)
            clockwise = !initialClockwise;
        if (currentEulerAngles.z < startAngle)
            clockwise = initialClockwise;

        Quaternion zrot = Quaternion.Euler(0.0f, 0.0f, (rotationSpeed * (clockwise ? -1 : 1)));
        trans.localRotation *= zrot;

        Debug.Log(rotationSpeed * (clockwise ? -1 : 1));
        Debug.Log(trans.rotation.eulerAngles);
    }
}
