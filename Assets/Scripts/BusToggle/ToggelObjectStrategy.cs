using UnityEngine;

public class ToggleObjectStrategy : IToggleStrategy
{
    private GameObject objectToToggle;
    private bool isActive;

    public ToggleObjectStrategy(GameObject obj, bool initialState)
    {
        objectToToggle = obj;
        isActive = initialState;
    }

    public void Execute()
    {
        isActive = !isActive;
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(isActive);
        }
    }
}