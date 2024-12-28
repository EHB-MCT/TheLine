using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    // Terugkeren naar het hoofdmenu
    public void BackToMainMenuProcess()
    {
        Debug.Log("Returning to the main menu...");
        SceneManager.LoadScene("MainMenu"); // Zorg dat de scène "MainMenu" heet
    }
}