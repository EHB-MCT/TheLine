using UnityEngine;

public class GameWonPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameWonPanel;  // UI Panel voor Game Won

    void Start()
    {
        // Zorg dat de Game Won popup verborgen is bij het starten
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Won Panel is not assigned in the Inspector!");
        }
    }

    // Toon de Game Won popup
    public void ShowGameWonPopup()
    {
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true); // Maak de Game Won popup zichtbaar
        }
    }
}
