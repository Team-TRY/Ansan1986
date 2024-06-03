using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] busStops; 
    private int currentStopIndex = 0;
    public float arrowspeed = 2f;
    private Transform target;

    void Start()
    {
        if (busStops.Length > 0)
        {
            target = busStops[currentStopIndex];
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * arrowspeed);
        }
    }

    public void UpdateTarget(int newStopIndex)
    {
        currentStopIndex = newStopIndex;
        target = busStops[currentStopIndex];
    }
}