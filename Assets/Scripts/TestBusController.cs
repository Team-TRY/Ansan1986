using UnityEngine;

public class TestBusController : MonoBehaviour
{
    public float speed = 10.0f;         
    public float rotationSpeed = 100.0f; 
    public float originalSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    void FixedUpdate()
    {
        Move();
        Turn();
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
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
}