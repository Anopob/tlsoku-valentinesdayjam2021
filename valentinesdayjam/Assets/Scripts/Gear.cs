using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float GearSpeed;
    public Transform[] Waypoints;
    private int _waypointIndex;

    private void Start()
    {

        if (Waypoints.Length > 0)
        {
            _waypointIndex = (_waypointIndex + 1) % Waypoints.Length;
        }
    }

    private void Update()
    {
        float step = GearSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, Waypoints[_waypointIndex].position, step);
        transform.Rotate(0,0,1);

        if ((transform.position - Waypoints[_waypointIndex].position).magnitude <= 0.2f)
        {
            _waypointIndex = (_waypointIndex + 1) % Waypoints.Length;
        }
    }
}
