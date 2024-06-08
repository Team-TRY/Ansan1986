using UnityEngine;

public class InitBusStop1 : MonoBehaviour
{
    [SerializeField] private BusCutsceneContext context;

    private void Start()
    {
        context.SetState(new BusStopState1());
    }
}