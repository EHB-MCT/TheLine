using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton voor eenvoudige toegang vanuit andere scripts
    public static LevelManager Instance;

    void Awake()
    {
        // Zorg ervoor dat er maar één LevelManager is en maak hem toegankelijk via de singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Methode om de volgende scène te laden
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Controleer of de volgende scène bestaat in de Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to main menu or resetting.");
            // Hier kun je een fallback activeren als er geen volgende scène is, bijvoorbeeld terug naar het hoofdmenu:
            // SceneManager.LoadScene(0); // Laad scène met index 0 (bijv. hoofdmenu)
        }
    }
}

