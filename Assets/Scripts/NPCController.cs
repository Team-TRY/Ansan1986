using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public GameObject path;
    public Transform[] pathPoints;

    public float minDistance;
    public int index = 0;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        pathPoints = new Transform[path.transform.childCount];

        for (int i = 0; i < pathPoints.Length; i++)
        {
            pathPoints[i] = path.transform.GetChild(i);
        }
    }

    private void Update()
    {
        Roam();
    }

    private void Roam()
    {
        if (Vector3.Distance(transform.position, pathPoints[index].position) < minDistance)
        {
            
            if (index >= 0 && index < pathPoints.Length-1)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }
        }
        
        agent.SetDestination(pathPoints[index].position);
        animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);
    }
}
