using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWiper : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator; 
    [SerializeField] private Button toggleButton; 

    private void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleAnimationParameter);
        }
    }

    private void ToggleAnimationParameter()
    {
        if (targetAnimator != null)
        {
            bool currentState = targetAnimator.GetBool("IsWiping");
            targetAnimator.SetBool("IsWiping", !currentState); 
        }
    }
}
