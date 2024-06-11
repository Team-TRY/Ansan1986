using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    static public int score;
    public TMP_Text tmp;
    public TMP_Text leaderboardText;

    void Start()
    {
        CalculateScore();
        DisplayScore();
        DisplayLeaderboard();
    }

    void CalculateScore()
    {
        score = 0;
        score += (int)(RemainTime.rTime * 10);

        this.transform.Find("Star1").gameObject.SetActive(false);
        this.transform.Find("Star2").gameObject.SetActive(false);
        this.transform.Find("Star3").gameObject.SetActive(false);

        if (score >= 100) this.transform.Find("Star1").gameObject.SetActive(true);
        if (score >= 600) this.transform.Find("Star2").gameObject.SetActive(true);
        if (score >= 900) this.transform.Find("Star3").gameObject.SetActive(true);
    }

    void DisplayScore()
    {
        tmp.text = string.Format("Score : {0}", score);
    }

    public void DisplayLeaderboard()
    {
        ScoreboardManager scoreboardManager = FindObjectOfType<ScoreboardManager>();
        List<ScoreEntry> topScores = scoreboardManager.GetTopScores();

        leaderboardText.text = "Top Scores:\n";
        foreach (var entry in topScores)
        {
            leaderboardText.text += entry.name + ": " + entry.score + "\n";
        }
    }
}