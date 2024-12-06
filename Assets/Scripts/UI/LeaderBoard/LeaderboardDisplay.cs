using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    public Text LeaderboardText;  // UI Text-element om scores te tonen

    void Start()
    {
        UpdateLeaderboardDisplay();
    }

    // Functie om de leaderboard weer te geven in het Text-element
    public void UpdateLeaderboardDisplay()
    {
        LeaderboardText.text = "Leaderboard\n";
        List<ScoreManager.ScoreEntry> scores = ScoreManager.GetScores();

        foreach (ScoreManager.ScoreEntry entry in scores)
        {
            LeaderboardText.text += entry.PlayerName + ": Level " + entry.LevelReached + " - " + entry.TimeTaken.ToString("F2") + " sec\n";
        }
    }
}

