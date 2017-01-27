using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public Leaderboard Leaderboard;

    public LeaderboardUIEntry[] LeaderboardUIEntries = new LeaderboardUIEntry[3];
    public CanvasGroup RankImage;
    public Text Rank;

    #region singleton
    private static LeaderboardUI _instance;
    public static LeaderboardUI Instance
    {
        get { return _instance; }

        private set { _instance = value; }
    }

    public LeaderboardUI()
    {
        Instance = this;
    }

    #endregion

    public static void AddHighscore(float time)
    {
        _instance.AddHighscoreLocal(time);
    }

    public static void Save()
    {
        if (_instance != null)
        {
            _instance.Leaderboard.Save();
        }
    }

    public static bool NameFinished()
    {
        return _instance.NameFinishedLocal();
    }

    public static void PressA()
    {
        _instance.PressALocal();
    }

    private const string AlphaNumRegex = "^[a-zA-Z0-9_]*$";
    private const int NameLength = 3;

    private bool _hasNewHighscore = false;
    private LeaderboardEntry _editableEntry;
    private int _rank;
    private void AddHighscoreLocal(float time)
    {
        _editableEntry = new LeaderboardEntry();
        _editableEntry.Time = time;
        _hasNewHighscore = Leaderboard.AddEntry(ref _editableEntry);
        _rank = Leaderboard.GetRank(ref _editableEntry);
        //Debug.Log("Rank: " + _rank + " ==> " + _hasNewHighscore);

        if (_hasNewHighscore)
        {
            Rank.text = _rank.ToString();
        }
        else
        {
            RankImage.alpha = 0;
        }

        for (int i=0; i< LeaderboardUIEntries.Length; i++)
        {
            if (Leaderboard.Data.EntryList.Count > i)
            {
                LeaderboardUIEntries[i].Name.text = Leaderboard.Data.EntryList[i].Name;
                LeaderboardUIEntries[i].Time.SetTime(Leaderboard.Data.EntryList[i].Time);
            }
            else
            {
                LeaderboardUIEntries[i].Name.text = string.Empty;
                LeaderboardUIEntries[i].Time.SetTime(0);
            }
        }
    }

    private bool NameFinishedLocal()
    {
        if (_hasNewHighscore)
        {
            return _editableEntry.Name.Length >= NameLength;
        }

        return true;
    }

    private void PressALocal()
    {
        if (_editableEntry.Name.Length < NameLength)
        {
            _editableEntry.Name += "A".ToUpper();
        }
    }

    private void Update()
    {
        if (_hasNewHighscore)
        {
            if (Input.anyKeyDown)
            {
                string pressed = Input.inputString;

                bool alphaNumericPressed = System.Text.RegularExpressions.Regex.IsMatch(pressed, AlphaNumRegex);

                if (_editableEntry.Name.Length < NameLength)
                {
                    _editableEntry.Name += pressed.ToUpper();
                }
            }

            if (string.IsNullOrEmpty(_editableEntry.Name))
            {
                LeaderboardUIEntries[_rank - 1].Name.text = "_";
            }
            else
            {
                LeaderboardUIEntries[_rank - 1].Name.text = _editableEntry.Name;
            }
        }
    }
}

[System.Serializable]
public class LeaderboardUIEntry
{
    public Text Name;
    public LeaderboardUITime Time;
}

[System.Serializable]
public class LeaderboardUITime
{
    public Text TimeDisplay;

    public void SetTime(float time)
    {
        if (time > 0)
        {
            int mins = Mathf.FloorToInt(time/60);
            int sec = Mathf.FloorToInt(time) - (mins*60);
            int mil = (Mathf.FloorToInt(time*60)) - ((mins*60*60) + (sec*60));

            string minsText = mins.ToString();
            string secText = sec.ToString();
            if (sec < 10)
            {
                secText = "0" + secText;
            }
            string milisecText = mil.ToString();
            if (mil < 10)
            {
                milisecText = "0" + milisecText;
            }

            TimeDisplay.text = minsText + "." + secText + "." + milisecText;
        }
        else
        {
            TimeDisplay.text = "0.00.00";
        }
    }
}