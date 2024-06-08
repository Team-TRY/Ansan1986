using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Content.Interaction;

public class BusStopState3 : IBusStopState
{
    public void HandleEnter(Collider other, BusCutsceneContext context)
    {
        if (other.CompareTag("Player"))
        {
            context.TimeLine.Play();

            XRLever buslever = context.Lever.GetComponent<XRLever>();
            buslever.state = LeverState.Neutral;

            context.BusObject.SetActive(false);
            context.AnimBus.SetActive(true);
            context.Collider.enabled = false;

            context.StartCoroutine(ResumeGame(context));
        }
    }

    public IEnumerator ResumeGame(BusCutsceneContext context)
    {
        yield return new WaitForSeconds(10);

        context.TimeLine.Stop();
        context.BusObject.SetActive(true);
        context.AnimBus.SetActive(false);

        foreach (var busChar in context.BusChars)
        {
            busChar.SetActive(false);
        }
        foreach (var animChar in context.AnimChars)
        {
            animChar.SetActive(false);
        }
    }
}