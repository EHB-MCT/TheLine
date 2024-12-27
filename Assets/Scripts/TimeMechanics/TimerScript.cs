using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    private float timeElapsed; // Houdt de verstreken tijd bij
    public Text timerText;     // Text UI element voor het weergeven van de timer

    // Singleton patroon
    public static TimerScript Instance { get; private set; }

    void Awake()
    {
        // Zorg ervoor dat er maar één instantie van de TimerScript is
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zorg ervoor dat dit object niet vernietigd wordt bij scenewissel
        }
        else
        {
            Destroy(gameObject); // Verwijder dit object als er al een andere timer is
        }
    }

    void Start()
    {
        if (Instance == this)  // Start de timer alleen als dit de actieve instantie is
        {
            timeElapsed = 0f;  // Initialiseer de timer op 0 bij het starten van de game
        }
    }

    void Update()
    {
        if (Instance == this)  // Zorg ervoor dat we de timer alleen bijwerken als dit de actieve instantie is
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Controleer of we op het main menu of de leaderboard scene zijn en stop de timer als dat het geval is
            if (currentSceneName == "MainMenu" || currentSceneName == "Leaderboard") // MainMenu en Leaderboard zijn de namen van je schermen
            {
                return; // Stop de timer wanneer we op het main menu of leaderboard scherm zijn
            }

            // Als we in de game zijn, blijf de tijd bijhouden
            timeElapsed += Time.deltaTime; // Verhoog de tijd met de verstreken tijd per frame

            // Bereken minuten, seconden en milliseconden
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

            // Update de UI text met de nieuwe tijd
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);
        }
    }

    // Voeg deze methode toe om de timer opnieuw in te stellen naar 0
    public void ResetTimer()
    {
        timeElapsed = 0f; // Zet de tijd weer op 0
        timerText.text = "00:00:000"; // Zet de timer UI ook naar 0
    }

    public void ReduceTime(float amount)
    {
        timeElapsed -= amount; // Trek de opgegeven hoeveelheid tijd af
        if (timeElapsed < 0f)
        {
            timeElapsed = 0f; // Zorg ervoor dat de timer niet negatief gaat
        }
    }
}
