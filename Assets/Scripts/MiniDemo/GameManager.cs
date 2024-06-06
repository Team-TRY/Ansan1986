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
        [SerializeField] Transform[] targets; // 타겟들
        public GameObject[] checkpoints; // 체크포인트들
        public int currentCheckpointIndex;
        public static GameManager instance;

        private Navigator navigator; // Navigator 인스턴스
        private Map map; // Map 인스턴스
        private bool isInitialized = false; // 초기화 여부

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
            navigator = FindObjectOfType<Navigator>(); // Navigator 초기화
            map = FindObjectOfType<Map>(); // Map 초기화
            ActivateCheckpoint(currentCheckpointIndex);
        }

        void Update()
        {
            if (!isInitialized) // 맵 초기화
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
                SetNewDestination(currentCheckpointIndex); // 현재 타겟 색상 변경
                checkpoints[currentCheckpointIndex].SetActive(false);
                currentCheckpointIndex++;

                if (currentCheckpointIndex < checkpoints.Length)
                {
                    ActivateCheckpoint(currentCheckpointIndex);
                    SetNewDestination(currentCheckpointIndex); // 다음 타겟 설정
                }
                else
                {
                    Time.timeScale = 0;
                    SceneManager.LoadScene("GameOver");
                }
            }
        }

        private void ActivateCheckpoint(int index)
        {
            if (index < checkpoints.Length)
            {
                checkpoints[index].SetActive(true);
                SetNewDestination(index); // 타겟을 활성화할 때 색상 변경
            }
        }

        private void SetNewDestination(int number) // 새로운 타겟 설정 메서드
        {
            if (targets.Length > number && targets[number])
            {
                if (number > 0)
                {
                    targets[number - 1].GetComponent<Renderer>().material.color = Color.gray; // 이전 타겟 색상 변경
                }

                targets[number].GetComponent<Renderer>().material.color = Color.red; // 현재 타겟 색상 변경
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
                Time.timeScale = 0;
                SceneManager.LoadScene("GameOver");
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            RemainTime.rTime = playTime;
            SceneManager.LoadScene("MiniDemo");
        }
    }
}
