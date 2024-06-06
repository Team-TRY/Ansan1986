using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Bus
{
    public class BusCollision : MonoBehaviour
    {
        public GameManager gameManager;
        public RemainTime remainTime;

        private void Start()
        {

        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "buliding")
            {
                Debug.Log("�����浹");
                remainTime.rTimeText.color = Color.red;
                StartCoroutine(TimerColor());
                RemainTime.rTime -= 5;
            }


        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "CheckPoint")
            {
                RemainTime.rTime += 15;
                gameManager.NextLevel();
            }
        }

        public IEnumerator TimerColor()
        {
            yield return new WaitForSeconds(1.0f);
            remainTime.rTimeText.color = Color.black;
        }
    }
}
