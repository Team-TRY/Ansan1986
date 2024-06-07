using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleRadio : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private Button toggleButton; 

    private bool isMusicPlaying = false; 

    private void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleMusicState);
        }
        
        if (musicSource != null)
        {
            isMusicPlaying = musicSource.isPlaying;
        }
    }

    private void ToggleMusicState()
    {
        if (musicSource != null)
        {
            if (isMusicPlaying)
            {
                musicSource.Pause();
            }
            else
            {
                musicSource.Play();
            }

            isMusicPlaying = !isMusicPlaying; 
        }
    }
}