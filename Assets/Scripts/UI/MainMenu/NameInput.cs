using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    public InputField playerNameInput;  // InputField voor de spelernaam
    public static string playerName;    // Statich zodat het kan worden doorgegeven tussen scènes

    // Functie om de naam van de speler te bevestigen en op te slaan
    public void ConfirmName()
    {
        // Haal de tekst uit de input en sla de naam op
        playerName = playerNameInput.text;

        // Check of een naam is ingevoerd, anders geef je een waarschuwing (optioneel)
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Voer een naam in.");
            // Toon eventueel een visuele waarschuwing aan de gebruiker
            return;
        }

        // Ga naar het spel na het invoeren van de naam
        // SceneManager.LoadScene("GameScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }
}