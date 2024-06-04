using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject[] checkpoints; 
    private int currentCheckpointIndex = 0; 

    void Start()
    {
        ActivateCheckpoint(currentCheckpointIndex);
    }

    public void NextLevel()
    {
        
        if (currentCheckpointIndex < checkpoints.Length)
        {
            checkpoints[currentCheckpointIndex].SetActive(false);
            currentCheckpointIndex++;
            
            if (currentCheckpointIndex < checkpoints.Length)
            {
                ActivateCheckpoint(currentCheckpointIndex);
            }
            else
            {
                Debug.Log("All check point clear！");
            }
        }
    }

    private void ActivateCheckpoint(int index)
    {
        if (index < checkpoints.Length)
        {
            checkpoints[index].SetActive(true);
        }
    }
}
