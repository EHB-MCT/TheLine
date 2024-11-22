using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public InputField playerNameInput;  // InputField voor de spelernaam
    public static string playerName;     // Static zodat het kan worden doorgegeven tussen scènes
    public VideoPlayer videoPlayer;      // Referentie naar de Video Player
    public RawImage rawImage;            // Referentie naar de Raw Image voor de video

    void Start()
    {
        PlayBackgroundVideo(); // Start de achtergrondvideo wanneer het menu wordt geladen
    }

    // Functie om de achtergrondvideo af te spelen
    void PlayBackgroundVideo()
    {
        // Zorg ervoor dat de video player correct is ingesteld
        if (videoPlayer != null && rawImage != null)
        {
            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0); // Stel de render texture in
            rawImage.texture = videoPlayer.targetTexture; // Koppel de render texture aan de Raw Image

            videoPlayer.Play(); // Speel de video af
        }
        else
        {
            Debug.LogError("VideoPlayer of RawImage is niet ingesteld.");
        }
    }

    // Functie om het spel te starten wanneer op de Play-knop wordt gedrukt
    public void PlayGame()
    {
        // Haal de tekst uit de input en sla de naam op
        playerName = playerNameInput.text;

        // Check of er een naam is ingevoerd, anders geef een waarschuwing
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Voer een geldige naam in.");
            // Optioneel: Geef visuele feedback aan de speler dat een naam verplicht is
            return;
        }

        // Laad de spel-scène nadat een naam is ingevoerd
        SceneManager.LoadScene("GameScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }

    // Functie om het leaderboard te tonen wanneer op de Leaderboard-knop wordt gedrukt
    public void ShowLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }

    // Functie om het spel af te sluiten
    public void QuitGame()
    {
        Application.Quit();
    }
}
