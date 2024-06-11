using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bus
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject scoreBoard, restartBtn;
        [SerializeField] float playTime = 180f;
        [SerializeField] Transform[] targets; 
        public GameObject[] checkpoints; 
        public int currentCheckpointIndex;
        public static GameManager instance;

        private Navigator navigator; 
        private Map map;
        private bool isInitialized = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {
            currentCheckpointIndex = 0;
            navigator = FindObjectOfType<Navigator>();
            map = FindObjectOfType<Map>();
            ActivateCheckpoint(currentCheckpointIndex);
        }

        void Update()
        {
            if (!isInitialized)
            {
                map.ShowAsNavigator();
                isInitialized = true;
            }

            Debug.Log(currentCheckpointIndex);
            EndTime();
        }

        public void NextLevel()
        {
            if (currentCheckpointIndex < checkpoints.Length)
            {
                SetNewDestination(currentCheckpointIndex);
                checkpoints[currentCheckpointIndex].SetActive(false);
                currentCheckpointIndex++;

                if (currentCheckpointIndex < checkpoints.Length)
                {
                    ActivateCheckpoint(currentCheckpointIndex);
                    SetNewDestination(currentCheckpointIndex);
                }
                else
                {
                    ShowScoreBoard();
                }
            }
        }

        private void ActivateCheckpoint(int index)
        {
            if (index < checkpoints.Length)
            {
                checkpoints[index].SetActive(true);
                SetNewDestination(index);
            }
        }

        private void SetNewDestination(int number)
        {
            if (targets.Length > number && targets[number])
            {
                if (number > 0)
                {
                    targets[number - 1].GetComponent<Renderer>().material.color = Color.gray; 
                }

                targets[number].GetComponent<Renderer>().material.color = Color.red;
                navigator.SetTargetPoint(targets[number].position);
            }
            else
            {
                Debug.LogWarning("No target set up in GameManager script. Check it for null fields.");
            }
        }

        void EndTime()
        {
            if (RemainTime.rTime <= 0)
            {
                ShowScoreBoard();
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            RemainTime.rTime = playTime;
            SceneManager.LoadScene("MiniDemo");
        }

        private void ShowScoreBoard()
        {
            Time.timeScale = 0;
            scoreBoard.SetActive(true);
            restartBtn.SetActive(true);
        }
    }
}
