using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager Instance { get; private set; }

    public int TimeIconsCollected { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTimeIcon()
    {
        TimeIconsCollected++;
        Debug.Log($"TimeIconsCollected: {TimeIconsCollected}");
    }

    public void ResetData()
    {
        TimeIconsCollected = 0;
        Debug.Log("Level data reset.");
    }
}

