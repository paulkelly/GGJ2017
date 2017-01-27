using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private const string PlayerprefKey = "SuperMarketShriekHighscores";
    private const int MaxEntries = 3;
    public LeaderboardData Data = new LeaderboardData();

    private void Awake()
    {
        Load();
    }

    public bool AddEntry(ref LeaderboardEntry newEntry)
    {
        bool wasAdded = false;
        for (int i = 0; i < MaxEntries; i++)
        {
            if (i >= Data.EntryList.Count)
            {
                Data.EntryList.Insert(i, newEntry);
                wasAdded = true;
                break;
            }
            if (Data.EntryList[i].Time > newEntry.Time)
            {
                Data.EntryList.Insert(i, newEntry);
                wasAdded = true;
                break;
            }
        }

        if (Data.EntryList.Count > MaxEntries)
        {
            Data.EntryList.RemoveAt(MaxEntries-1);
        }

        return wasAdded;
    }

    public int GetRank(ref LeaderboardEntry newEntry)
    {
        for (int i = 0; i < Data.EntryList.Count; i++)
        {
            if (Data.EntryList[i].GetHashCode() == newEntry.GetHashCode())
            {
                return i+1;
            }
        }

        return -1;
    }

    public void Save()
    {
        PlayerPrefs.SetString(PlayerprefKey, JsonUtility.ToJson(Data));
        Debug.Log("SAVE: " + JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(PlayerprefKey))
        {
            Debug.Log("LOADING");
            Data = (LeaderboardData) JsonUtility.FromJson(PlayerPrefs.GetString(PlayerprefKey), typeof(LeaderboardData));
        }
    }
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> EntryList = new List<LeaderboardEntry>();
}

[System.Serializable]
public class LeaderboardEntry
{
    public string Name = "";
    public float Time;
}