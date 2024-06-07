using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleYellowLight : MonoBehaviour
{
    [SerializeField] private GameObject objectsToToggle;
    [SerializeField] private Button toggleButton;

    private bool areObjectsActive = false; 

    private void Start()
    {
        if (objectsToToggle != null)
        {
            objectsToToggle.SetActive(areObjectsActive);
        }

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleObjectsState);
        }
    }

    private void ToggleObjectsState()
    {
        areObjectsActive = !areObjectsActive;

        if (objectsToToggle != null)
        {
            objectsToToggle.SetActive(areObjectsActive);
        }
    }
}