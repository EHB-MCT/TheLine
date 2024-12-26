using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    private float timeElapsed; // Houdt de verstreken tijd bij
    public Text timerText;     // Text UI element voor het weergeven van de timer

    // Singleton pattern
    private static TimerScript instance;

    void Awake()
    {
        // Zorg ervoor dat er maar één instantie van de TimerScript is
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zorg ervoor dat dit object niet vernietigd wordt bij scenewissel
        }
        else
        {
            Destroy(gameObject); // Verwijder dit object als er al een andere timer is
        }
    }

    void Start()
    {
        if (instance == this)  // Start de timer alleen als dit de actieve instantie is
        {
            timeElapsed = 0f;  // Initialiseer de timer op 0 bij het starten van de game
        }
    }

    void Update()
    {
        if (instance == this)  // Zorg ervoor dat we de timer alleen bijwerken als dit de actieve instantie is
        {
            timeElapsed += Time.deltaTime; // Verhoog de tijd met de verstreken tijd per frame

            // Bereken minuten, seconden en milliseconden
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

            // Update de UI text met de nieuwe tijd
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);
        }
    }
}
