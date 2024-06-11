using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ScoreboardManager : MonoBehaviour
{
    private string filePath;
    private Scoreboard scoreboard;

    void Start()
    {
        filePath = Application.persistentDataPath + "/scoreboard.json";
        Debug.Log(filePath);
        LoadScoreboard();
    }


    public void AddScore(string name, int score)
    {
        scoreboard.entries.Add(new ScoreEntry(name, score));
        scoreboard.entries.Sort((entry1, entry2) => entry2.score.CompareTo(entry1.score)); // 객롸鑒탤埼
        SaveScoreboard();
    }

    // Get top 10 score
    public List<ScoreEntry> GetTopScores(int limit = 10)
    {
        return scoreboard.entries.GetRange(0, Mathf.Min(limit, scoreboard.entries.Count));
    }

    
    private void SaveScoreboard()
    {
        string json = JsonUtility.ToJson(scoreboard, true);
        File.WriteAllText(filePath, json);
    }

    // load the rank list from files
    private void LoadScoreboard()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoreboard = JsonUtility.FromJson<Scoreboard>(json);
        }
        else
        {
            scoreboard = new Scoreboard();
        }
    }

    //Be careful
    public void DeleteAllScores()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            scoreboard = new Scoreboard(); // Reset the in-memory scoreboard
            Debug.Log("All scores deleted.");
        }
        else
        {
            Debug.Log("No score file found to delete.");
        }
    }

}
