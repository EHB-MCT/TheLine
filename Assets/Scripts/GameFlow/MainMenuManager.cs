using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Voeg deze namespace toe voor scene management

public class MainMenuManager : MonoBehaviour
{
    // Methode om naar het eerste level te gaan
    public void StartGame()
    {
        // Zorg ervoor dat het eerste level de juiste build index heeft in je Build Settings
        SceneManager.LoadScene(1); // Laad scène met index 1 (je eerste level)
    }
}

