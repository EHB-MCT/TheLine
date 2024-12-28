using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Herstart de game door terug te keren naar het MainMenu
    public void RestartGameProcess()
    {
        Time.timeScale = 1f; // Zet de tijd terug naar normaal
        SceneManager.LoadScene("MainMenu"); // Zorg dat de scène "MainMenu" heet
        TimerScript.Instance.ResetTimer(); // Reset de timer naar 0 bij het herstarten
    }
}

