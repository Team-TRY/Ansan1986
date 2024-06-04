using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard, restartBtn;
    [SerializeField] float playTime = 180f;

    public GameObject[] checkpoints; 
    public int currentCheckpointIndex;
    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        currentCheckpointIndex=0;
        ActivateCheckpoint(currentCheckpointIndex);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentCheckpointIndex);
        EndTime();
    }

    public void NextLevel()
    {
        
        if (currentCheckpointIndex < checkpoints.Length)
        {
//            Debug.Log("NextLevel");
            checkpoints[currentCheckpointIndex].SetActive(false);
            currentCheckpointIndex++;
            
            if (currentCheckpointIndex < checkpoints.Length)
            {
//                Debug.Log("currentCheckpointIndex++");
                ActivateCheckpoint(currentCheckpointIndex);
            }
            else
            {
//                Debug.Log("GameEnd");
                Time.timeScale = 0;
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void ActivateCheckpoint(int index)
    {
        if (index < checkpoints.Length)
        {
//            Debug.Log("ActivateCheckpoint");
            checkpoints[index].SetActive(true);
        }
    }

    void EndTime()
    {
        if(RemainTime.rTime <=0)
        {
            Time.timeScale = 0;

            SceneManager.LoadScene("GameOver");

        }
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        RemainTime.rTime = playTime;
        SceneManager.LoadScene("MiniDemo");
    }
}
