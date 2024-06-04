using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public LevelController levelController;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            levelController.NextLevel();
        }
    }
}
