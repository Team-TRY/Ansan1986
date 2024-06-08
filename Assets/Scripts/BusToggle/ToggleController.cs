using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Button toggleButton;
    private IToggleStrategy toggleStrategy;

    public void Initialize(IToggleStrategy strategy)
    {
        toggleStrategy = strategy;
    }

    private void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleState);
        }
    }

    private void ToggleState()
    {
        toggleStrategy?.Execute();
    }
}