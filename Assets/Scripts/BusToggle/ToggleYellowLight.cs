using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleYellowLight : MonoBehaviour
{
    [SerializeField] private GameObject objectsToToggle;
    [SerializeField] private Button toggleButton;

    private bool areObjectsActive = true;

    private void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleObjectsState);
        }
    }

    private void ToggleObjectsState()
    {
        areObjectsActive = !areObjectsActive;
        
        objectsToToggle.SetActive(areObjectsActive);
    }
}