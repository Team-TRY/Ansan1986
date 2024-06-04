using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BusCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "buliding")
        {
            Debug.Log("ºôµùÃæµ¹");
            RemainTime.rTime -= 5;
        }


    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "EndPoint")
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("GameOver");
        }
    }
}

