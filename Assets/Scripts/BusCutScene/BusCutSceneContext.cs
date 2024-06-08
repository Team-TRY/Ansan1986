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
    public GameObject[] BusChars;
    public GameObject[] AnimChars;

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