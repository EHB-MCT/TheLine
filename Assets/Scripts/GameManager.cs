using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text TimerText;             // UI element om de tijd bovenaan te tonen
    public Text CountdownText;         // UI element voor de 3-2-1 countdown
    private float TotalTime = 0f;      // Totale speeltijd
    private bool IsPlaying = false;    // Geeft aan of de speler actief speelt of niet

    public int CurrentLevel = 1;       // Huidig level van de speler, begint altijd op 1
    public int MaxLevelReached = 1;    // Houdt bij tot welk level de speler is gekomen

    void Start()
    {
        // Initialiseer het eerste level en start de countdown
        StartCoroutine(StartLevelWithCountdown(CurrentLevel));
    }

    void Update()
    {
        // Alleen de timer bijwerken als het spel actief bezig is
        if (IsPlaying)
        {
            TotalTime += Time.deltaTime;   // Telt door zolang de speler speelt
            UpdateTimerUI();               // Update de UI zodat de speler de huidige tijd ziet
        }
    }

    // Functie om de timer bovenaan het scherm te tonen
    void UpdateTimerUI()
    {
        TimerText.text = "Time: " + TotalTime.ToString("F2") + " sec";  // Toon tijd in seconden met 2 decimalen
    }

    // Coroutine om een level te starten met een 3-2-1 countdown
    IEnumerator StartLevelWithCountdown(int level)
    {
        IsPlaying = false;              // Pauzeer het spel
        CountdownText.gameObject.SetActive(true);  // Toon de countdown UI

        // Countdown van 3 naar 1
        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);   // Wacht 1 seconde tussen elke tel
        }

        CountdownText.text = "Go!";     // "Go" als de countdown is voltooid
        yield return new WaitForSeconds(0.5f);     // Wacht een halve seconde voordat het spel begint

        CountdownText.gameObject.SetActive(false);  // Verberg de countdown UI
        IsPlaying = true;               // Start het spel

        StartLevel(level);              // Start het level
    }

    // Functie om een level te starten
    public void StartLevel(int level)
    {
        CurrentLevel = level;
        Debug.Log("Level " + level + " gestart.");
        // Hier komt logica voor het laden van een specifiek level
    }

    // Functie om het huidige level te voltooien
    public void CompleteLevel()
    {
        CurrentLevel++;
        MaxLevelReached = CurrentLevel > MaxLevelReached ? CurrentLevel : MaxLevelReached;

        // Laad het volgende level met een countdown
        StartCoroutine(StartLevelWithCountdown(CurrentLevel));
    }

    // Functie om het spel te beëindigen (bij verlies)
    public void EndGame()
    {
        // Sla de huidige score en tijd op en ga terug naar het hoofdmenu of herstart het spel
        Debug.Log("Game Over. Hoogste level bereikt: " + MaxLevelReached + ". Totale speeltijd: " + TotalTime + " seconden.");
        
        // Sla de score op voor het leaderboard
        SaveScoreToLeaderboard();

        // Ga terug naar het hoofdmenu na het verliezen
        SceneManager.LoadScene("MainMenuScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }

    // Functie om de naam, het level en de tijd naar de leaderboard te sturen
    public void SaveScoreToLeaderboard()
    {
        Leaderboard.AddScore(MainMenu.playerName, MaxLevelReached, TotalTime); // Use MainMenu.playerName
    }
}
