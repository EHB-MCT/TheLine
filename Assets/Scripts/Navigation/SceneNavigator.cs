using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    // Functie om naar een specifieke scène te navigeren
    public void NavigateToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
