using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    // Stop de game door de tijd te stoppen
    public void StopGameProcess()
    {
        Time.timeScale = 0f; // Stop de tijd
        Debug.Log("Game Stopped. Time is frozen.");
    }
}