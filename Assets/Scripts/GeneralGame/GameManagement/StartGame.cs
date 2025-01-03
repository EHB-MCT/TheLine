using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using System.IO; // Voor Path en File
using Newtonsoft.Json; // Voor JsonConvert

public class StartGame : MonoBehaviour
{

    public void StartGameProcess()
    {
        Debug.Log("Starting the game...");
        SceneManager.LoadScene(2); // Laad het eerste level
    }
}
