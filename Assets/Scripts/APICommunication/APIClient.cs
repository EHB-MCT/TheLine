// This script is responsible for sending a test API request to a predefined endpoint.
// It handles the response from the server and logs the result or any errors.

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private const string BASE_URL = "http://localhost:5033/api/test"; // Update URL to your test endpoint

    // Sends a GET request to the API and processes the response.
    public IEnumerator TestAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL);
        yield return request.SendWebRequest();

        // Checks the result of the request and logs the response or error.
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response from API: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    // Starts the API test when the script begins.
    private void Start()
    {
        StartCoroutine(TestAPI());
    }
}
