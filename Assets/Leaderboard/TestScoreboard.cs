using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TestScoreboard : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField scoreInput;
    public Button submitButton;
    public TMP_Text leaderboardText;
    public ScoreboardManager scoreboardManager;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
        DisplayLeaderboard();
    }

    void OnSubmit()
    {
        string name = nameInput.text;
        int score;
        if (int.TryParse(scoreInput.text, out score))
        {
            scoreboardManager.AddScore(name, score);
            DisplayLeaderboard();
        }
        else
        {
            Debug.LogError("Invalid score input");
        }
    }

    void DisplayLeaderboard()
    {
        List<ScoreEntry> topScores = scoreboardManager.GetTopScores();
        leaderboardText.text = "Top Scores:\n";
        foreach (var entry in topScores)
        {
            leaderboardText.text += entry.name + ": " + entry.score + "\n";
        }
    }
}
