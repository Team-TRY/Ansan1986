using UnityEngine;
using UnityEngine.AI;

public class Passenger : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform assignedSeat;

    public string destinationStop;
    public bool isSeated = false; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void MoveToBus(Vector3 doorPosition)
    {
        agent.SetDestination(doorPosition);
        WalkToward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BusDoor"))
        {
            EnterBus();
        }
        else if (other.CompareTag("Seat"))
        {
            SeatManager seatManager = other.GetComponent<SeatManager>();
            if (seatManager != null && other.transform == assignedSeat && seatManager.TryToSit())
            {
                SitDown(assignedSeat);
            }
        }
    }

    private void EnterBus()
    {
        if (assignedSeat != null)
        {
            agent.SetDestination(assignedSeat.position);
            WalkToward();
        }
    }
    
    private void WalkToward()
    {
        animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);
    }

    private void SitDown(Transform seatTransform)
    {
        transform.SetParent(seatTransform);
        agent.enabled = false;  
        transform.localPosition = Vector3.zero;  
        transform.localRotation = Quaternion.identity;  
        animator.SetBool("isSitting", true); 
    }
    
    public void Disembark()
    {
        isSeated = false;
        gameObject.SetActive(false);
    }
}