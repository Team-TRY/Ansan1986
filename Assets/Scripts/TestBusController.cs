using UnityEngine;
using System.Collections;

public class TestBusController : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float originalSpeed;
    public Transform[] busStops; 
    private int currentStopIndex = 0;
    private bool isStopped = false;
    public float stopDuration = 2f; 

    private Rigidbody rb;
    public RouteVisualizer routeVisualizer; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        routeVisualizer.UpdateLineRenderer(currentStopIndex); 
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            Move();
            Turn();
        }
    }

    void Move()
    {
        float moveVertical = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + transform.forward * moveVertical * speed * Time.fixedDeltaTime);
    }

    void Turn()
    {
        float turnHorizontal = Input.GetAxis("Horizontal");
        float turn = turnHorizontal * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
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
        SetSpeed(0);
        yield return new WaitForSeconds(stopDuration);

        if (currentStopIndex > 0)
        {
            routeVisualizer.UpdateLineRenderer(currentStopIndex);
        }

        busStops[currentStopIndex].gameObject.SetActive(false);

        currentStopIndex = (currentStopIndex + 1) % busStops.Length;

        ResetSpeed();
        isStopped = false;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
}
