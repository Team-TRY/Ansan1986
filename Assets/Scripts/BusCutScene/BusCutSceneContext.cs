using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Content.Interaction;

public class BusCutsceneContext : MonoBehaviour
{
    public PlayableDirector TimeLine;
    public GameObject BusObject;
    public GameObject Lever;
    public GameObject AnimBus;
    public GameObject BusChar, BusChar1, BusChar2;
    public GameObject AnimChar, AnimChar1, AnimChar2;

    private IBusStopState currentState;
    [HideInInspector]
    public BoxCollider Collider;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public void SetState(IBusStopState state)
    {
        currentState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.HandleEnter(other, this);
    }

    public void StartCutsceneCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}