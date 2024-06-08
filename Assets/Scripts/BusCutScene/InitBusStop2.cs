using UnityEngine;

public class InitBusStop2 : MonoBehaviour
{
    [SerializeField] private BusCutsceneContext context;

    private void Start()
    {
        context.SetState(new BusStopState2());
    }
}