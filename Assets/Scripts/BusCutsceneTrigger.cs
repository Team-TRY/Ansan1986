using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Content.Interaction;

public class BusCutsceneTrigger : MonoBehaviour
{
    public PlayableDirector timeLine;
    public GameObject busObject;
    public GameObject lever;
    public GameObject animBus;
    public GameObject busChar;
    public GameObject animChar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeLine.Play();

            XRLever buslever = lever.GetComponent<XRLever>();
            buslever.state = LeverState.Neutral;
            
            busObject.SetActive(false);
            animBus.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            
            StartCoroutine(ResumeGame());
        }
    }

    IEnumerator ResumeGame()
    {
        yield return new WaitForSeconds(10);
        
        timeLine.Stop();
        busObject.SetActive(true);
        animBus.SetActive(false);
        
        busChar.SetActive(true);
        animChar.SetActive(false);
    }
}
