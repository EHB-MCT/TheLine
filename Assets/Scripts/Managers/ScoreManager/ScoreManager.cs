using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    // Data voor leaderboard: een lijst van spelers en hun hoogste level + tijd
    private static List<ScoreEntry> ScoreEntries = new List<ScoreEntry>();

    // Functie om een nieuwe score toe te voegen aan het leaderboard
    public static void AddScore(string playerName, int levelReached, float timeTaken)
    {
        AddScore.Execute(playerName, levelReached, timeTaken, ScoreEntries);
    }

    // Functie om alle scores op te halen
    public static List<ScoreEntry> GetScores()
    {
        return GetScores.Execute(ScoreEntries);
    }
}

