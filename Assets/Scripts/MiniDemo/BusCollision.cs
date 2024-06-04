using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class BusCollision : MonoBehaviour
{
    public GameManager gameManager;
    public RemainTime remainTime;
    
    private void Start()
    {

    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "buliding")
        {
            Debug.Log("빌딩충돌");
            remainTime.rTimeText.color = Color.red;
            TimerColor(1.0f);
            RemainTime.rTime -= 5;
        }


    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "CheckPoint")
        {
            RemainTime.rTime += 15;
            gameManager.NextLevel();
        }
    }

    public IEnumerator TimerColor(float duration)
    {
        float time = 0.0f;

        while(time<1.0f)
        {
            time += Time.deltaTime/duration;
            Debug.Log("코루틴");
        }

        yield return null;
    }
}

