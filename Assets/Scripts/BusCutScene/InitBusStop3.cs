using UnityEngine;

public class InitBusStop3 : MonoBehaviour
{
    [SerializeField] private BusCutsceneContext context;

    private void Start()
    {
        context.SetState(new BusStopState3());
    }
}