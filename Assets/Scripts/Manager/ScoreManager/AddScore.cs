using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AddScore
{
    // Functie om een nieuwe score toe te voegen aan het leaderboard
    public static void Execute(string playerName, int levelReached, float timeTaken, List<ScoreEntry> scoreEntries)
    {
        scoreEntries.Add(new ScoreEntry(playerName, levelReached, timeTaken));
        // Sorteer eerst op hoogste level, daarna op kortste tijd
        scoreEntries.Sort((x, y) =>
        {
            if (y.LevelReached != x.LevelReached)
                return y.LevelReached.CompareTo(x.LevelReached);
            else
                return x.TimeTaken.CompareTo(y.TimeTaken); // Kleinere tijd is beter
        });
    }
}

