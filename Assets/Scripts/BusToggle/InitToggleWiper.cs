using UnityEngine;
using UnityEngine.UI;

public class InitToggleWiper : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private Button toggleButton;

    private void Awake()
    {
        var strategy = new ToggleWiperStrategy(targetAnimator);
        var controller = toggleButton.GetComponent<ToggleController>();
        controller.Initialize(strategy);
    }
}