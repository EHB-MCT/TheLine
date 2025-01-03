using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:5033/api/test"; // Update URL naar jouw testendpoint

    public IEnumerator TestAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response from API: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    private void Start()
    {
        // Start de test wanneer het script begint
        StartCoroutine(TestAPI());
    }
}