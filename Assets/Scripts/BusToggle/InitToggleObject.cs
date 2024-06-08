using UnityEngine;
using UnityEngine.UI;

public class InitToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToToggle;
    [SerializeField] private Button toggleButton;
    [SerializeField] private bool initialState = false;

    private void Awake()
    {
        var strategy = new ToggleObjectStrategy(objectToToggle, initialState);
        var controller = toggleButton.GetComponent<ToggleController>();
        controller.Initialize(strategy);
    }
}