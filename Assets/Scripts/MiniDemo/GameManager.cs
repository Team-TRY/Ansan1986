using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard, restartBtn;
    [SerializeField] float playTime = 180f;

    // Update is called once per frame
    void Update()
    {
        EndTime();
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
