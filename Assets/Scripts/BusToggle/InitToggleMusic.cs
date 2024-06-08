using UnityEngine;
using UnityEngine.UI;

public class InitToggleMusic : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Button toggleButton;

    private void Awake()
    {
        var strategy = new ToggleMusicStrategy(musicSource);
        var controller = toggleButton.GetComponent<ToggleController>();
        controller.Initialize(strategy);
    }
}