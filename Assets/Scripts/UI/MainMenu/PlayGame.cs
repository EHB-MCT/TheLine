using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public InputField playerNameInput;  // InputField voor de spelernaam
    public static string playerName;     // Static zodat het kan worden doorgegeven tussen scènes

    public void Play()
    {
        // Haal de tekst uit de input en sla de naam op
        playerName = playerNameInput.text;

        // Check of er een naam is ingevoerd, anders geef een waarschuwing
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Voer een geldige naam in.");
            return;
        }

        // Laad de spel-scène nadat een naam is ingevoerd
        SceneManager.LoadScene("GameScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }
}

