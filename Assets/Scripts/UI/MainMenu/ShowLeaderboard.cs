using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ShowLeaderboard : MonoBehaviour
{
    public void Show()
    {
        SceneManager.LoadScene("LeaderboardScene"); // Dit moet later worden aangepast met de juiste scenenaam
    }
}

