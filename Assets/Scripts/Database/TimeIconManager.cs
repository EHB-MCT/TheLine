using UnityEngine;

public class TimeIconManager : MonoBehaviour
{
    public static TimeIconManager Instance { get; private set; }

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

    public void ResetTimeIcons()
    {
        TimeIconsCollected = 0;
        Debug.Log("Time icon data reset.");
    }
}
