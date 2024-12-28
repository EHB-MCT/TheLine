using UnityEngine;

public class LevelCheck : MonoBehaviour
{
    // Singleton voor eenvoudige toegang vanuit andere scripts
    public static LevelCheck Instance;

    // Verwijzing naar GameWonPopup (zorg ervoor dat dit script ook correct is toegewezen)
    public GameWonPopup gameWonPopup;

    void Awake()
    {
        // Zorg ervoor dat er maar één LevelCheck is en maak hem toegankelijk via de singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Controleer of er meer levels zijn
    public void CheckForMoreLevels()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Als er geen volgende scène is
        if (nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            gameWonPopup.ShowGameWonPopup();  // Als er geen verdere levels zijn, toon de GameWonPopup
        }
    }
}
