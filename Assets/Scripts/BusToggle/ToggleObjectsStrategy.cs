using UnityEngine;

public class ToggleObjectsStrategy : IToggleStrategy
{
    private GameObject[] objectsToToggle;
    private bool areObjectsActive;

    public ToggleObjectsStrategy(GameObject[] objects, bool initialState)
    {
        objectsToToggle = objects;
        areObjectsActive = initialState;
    }

    public void Execute()
    {
        areObjectsActive = !areObjectsActive;
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(areObjectsActive);
            }
        }
    }
}