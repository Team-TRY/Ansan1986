using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public GameObject passengerPrefab;
    public int maxPassengers;
    private int currentPassengers;

    private void Start()
    {
        for (int i = 0; i < maxPassengers; i++)
        {
            Instantiate(passengerPrefab, transform.position + new Vector3(i * 2, 0, 0), Quaternion.identity, transform);
        }
        currentPassengers = maxPassengers;
    }
}