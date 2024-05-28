using UnityEngine;

public class RouteVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform[] busStops;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = busStops.Length;
        for (int i = 0; i < busStops.Length; i++)
        {
            lineRenderer.SetPosition(i, busStops[i].position);
        }
    }

    public void UpdateLineRenderer(int currentStopIndex)
    {
        if (currentStopIndex < busStops.Length)
        {
            lineRenderer.positionCount = busStops.Length - currentStopIndex;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, busStops[currentStopIndex + i].position);
            }
        }
    }
}