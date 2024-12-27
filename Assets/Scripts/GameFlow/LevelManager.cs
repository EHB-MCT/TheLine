using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton voor eenvoudige toegang vanuit andere scripts
    public static LevelManager Instance;

    // Verwijzing naar GameUIManager
    public GameUIManager gameUIManager;

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
            ShowNoMoreLevelsPopup();
        }
    }

    // Toon de Game Over popup als er geen volgende scène is
    private void ShowNoMoreLevelsPopup()
    {
        if (gameUIManager != null)
        {
            gameUIManager.ShowGameWonPopup(); // Roep de popup aan van GameUIManager
        }
    }
}
