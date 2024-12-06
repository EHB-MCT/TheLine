using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScoreEntry
{
    public string PlayerName;
    public int LevelReached;
    public float TimeTaken;

    public ScoreEntry(string name, int level, float time)
    {
        PlayerName = name;
        LevelReached = level;
        TimeTaken = time;
    }
}

