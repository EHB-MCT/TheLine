using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public string PlayerId { get; private set; }
    public int HighestLevelReached { get; private set; }

    private string updateLevelUrl = "http://localhost:5033/api/player/update-highest-level";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Blijf bestaan tussen scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerData(string playerId, int highestLevelReached)
    {
        PlayerId = playerId;
        HighestLevelReached = highestLevelReached;
        Debug.Log($"PlayerManager initialized: PlayerId={PlayerId}, HighestLevelReached={HighestLevelReached}");
    }

    public void UpdateHighestLevel(int newLevel)
    {
        if (newLevel > HighestLevelReached)
        {
            HighestLevelReached = newLevel;
            Debug.Log($"New highest level reached: {HighestLevelReached}");
            StartCoroutine(UpdateHighestLevelInDatabase());
        }
    }

    private IEnumerator UpdateHighestLevelInDatabase()
    {
        var json = JsonUtility.ToJson(new UpdateLevelRequest
        {
            PlayerId = PlayerId,
            NewLevel = HighestLevelReached
        });

        using (UnityWebRequest request = new UnityWebRequest(updateLevelUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Highest level updated successfully in the database.");
            }
            else
            {
                Debug.LogError($"Failed to update highest level: {request.error}");
            }
        }
    }

    [System.Serializable]
    public class UpdateLevelRequest
    {
        public string PlayerId;
        public int NewLevel;
    }
}
