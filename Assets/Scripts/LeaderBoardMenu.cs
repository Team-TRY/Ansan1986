using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardMenu : MonoBehaviour
{
    public TMP_Text leaderboardText;

    private void Start()
    {
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        ScoreboardManager scoreboardManager = FindObjectOfType<ScoreboardManager>();
        List<ScoreEntry> topScores = scoreboardManager.GetTopScores();

        leaderboardText.text = "\n";
        for (int i = 0; i < topScores.Count; i++)
        {
            var entry = topScores[i];
            leaderboardText.text += string.Format("{0}. {1}: {2}\n", i + 1, entry.name, entry.score);
        }
    }
}
