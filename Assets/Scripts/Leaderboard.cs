using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public Text LeaderboardText;  // UI Text-element om scores te tonen

    // Data voor leaderboard: een lijst van spelers en hun hoogste level + tijd
    private static List<ScoreEntry> ScoreEntries = new List<ScoreEntry>();

    // Struct om naam, score en tijd op te slaan
    [System.Serializable]
    public class ScoreEntry
    {
        public string PlayerName;
        public int LevelReached;
        public float TimeTaken;

        public ScoreEntry(string name, int level, float time)
        {
            PlayerName = name;
            LevelReached = level;
            TimeTaken = time;
        }
    }

    // Functie om een nieuwe score toe te voegen aan het leaderboard
    public static void AddScore(string playerName, int levelReached, float timeTaken)
    {
        ScoreEntries.Add(new ScoreEntry(playerName, levelReached, timeTaken));
        // Sorteer eerst op hoogste level, daarna op kortste tijd
        ScoreEntries.Sort((x, y) => 
        {
            if (y.LevelReached != x.LevelReached)
                return y.LevelReached.CompareTo(x.LevelReached);
            else
                return x.TimeTaken.CompareTo(y.TimeTaken);  // Kleinere tijd is beter
        });
    }

    // Functie om het leaderboard te tonen
    void Start()
    {
        UpdateLeaderboardDisplay();
    }

    // Functie om de leaderboard weer te geven in het Text-element
    void UpdateLeaderboardDisplay()
    {
        LeaderboardText.text = "Leaderboard\n";
        foreach (ScoreEntry entry in ScoreEntries)
        {
            LeaderboardText.text += entry.PlayerName + ": Level " + entry.LevelReached + " - " + entry.TimeTaken.ToString("F2") + " sec\n";
        }
    }

    // Functie om terug te keren naar het hoofdmenu
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); // Zorg ervoor dat de naam van de MainMenu scene correct is
    }
}
