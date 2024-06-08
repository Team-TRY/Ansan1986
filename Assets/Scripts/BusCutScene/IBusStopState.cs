using System.Collections;
using UnityEngine;

public interface IBusStopState
{
    void HandleEnter(Collider other, BusCutsceneContext context);
    IEnumerator ResumeGame(BusCutsceneContext context);
}