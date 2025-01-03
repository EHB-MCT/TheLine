using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:5000/api/players";

    public IEnumerator GetPlayers()
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            // Parse de JSON hier als nodig
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}

