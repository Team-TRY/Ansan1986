using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 충돌 함수를 처리하기 위해서는 충돌하는 두 물체 중 한가지는 Rigidbody를 설정해줘야 된다.
    private void OnCollisionEnter(Collision collision)
    {
        // 만약 특정 태그가 부딪친 것을 감지한다면 (예시에선 "Plane"이라는 태그가 부딪침 감지가 된다면)
        if (collision.collider.gameObject.CompareTag("Plane"))
        {
        //해당 줄에 실행 처리 (eg: 점수 감소, 부딪침 알림 등)
            Debug.Log("This is plane");
        }
    }
}
