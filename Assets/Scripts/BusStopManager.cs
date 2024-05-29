using UnityEngine;
using System.Collections;

public class BusStopManager : MonoBehaviour
{
    public Transform[] busStops;
    private int currentStopIndex = 0;
    private bool isStopped = false;
    public float stopDuration = 2f;

    private TestBusController busController;
    public WayPoints wayPoints; // 외부에서 할당하도록 public으로 설정

    void Start()
    {
        busController = GetComponent<TestBusController>();

        // wayPoints는 외부에서 수동으로 할당되도록 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BusStop") && other.transform == busStops[currentStopIndex])
        {
            StartCoroutine(StopAtBusStop());
        }
    }

    IEnumerator StopAtBusStop()
    {
        isStopped = true;
        busController.SetSpeed(0);
        yield return new WaitForSeconds(stopDuration);

        busStops[currentStopIndex].gameObject.SetActive(false);

        currentStopIndex = (currentStopIndex + 1) % busStops.Length;

        if (wayPoints != null)
        {
            wayPoints.UpdateTarget(currentStopIndex);
        }

        busController.ResetSpeed();
        isStopped = false;
    }
}