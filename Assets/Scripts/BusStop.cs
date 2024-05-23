using UnityEngine;

public class BusStop : MonoBehaviour
{
    public TestBusController busController;
    public Transform doorTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == busController.gameObject)
        {
            busController.SetSpeed(1); 
            NotifyNPCsBusHasArrived();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == busController.gameObject)
        {
            busController.ResetSpeed(); 
        }
    }

    private void NotifyNPCsBusHasArrived()
    {
        foreach (Transform child in transform)
        {
            Passenger npc = child.GetComponent<Passenger>();
            if (npc != null)
            {
                npc.MoveToBus(doorTransform.position);
            }
        }
    }
}