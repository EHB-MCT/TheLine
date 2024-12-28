using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // Singleton voor eenvoudige toegang vanuit andere scripts
    public static LoadNextScene Instance;

    void Awake()
    {
        // Zorg ervoor dat er maar één LoadNextScene is en maak hem toegankelijk via de singleton
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
    public void LoadNextSceneProcess()
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
            LevelCheck.Instance.CheckForMoreLevels();  // Roep de LevelCheck aan om te controleren of er nog levels zijn
        }
    }
}
