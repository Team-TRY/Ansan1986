using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusCollision : MonoBehaviour
{
    public GameManager gameManager;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "buliding")
        {
            Debug.Log("ºôµùÃæµ¹");
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
}

