using UnityEngine;

public class ToggleMusicStrategy : IToggleStrategy
{
    private AudioSource musicSource;
    private bool isMusicPlaying;

    public ToggleMusicStrategy(AudioSource source)
    {
        musicSource = source;
        isMusicPlaying = musicSource.isPlaying;
    }

    public void Execute()
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