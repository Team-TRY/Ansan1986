using System;
using System.Collections.Generic;

[Serializable]
public class ScoreEntry
{
    public string name;
    public int score;

    public ScoreEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[Serializable]
public class Scoreboard
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}
