using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Timer _instance;

    private void Awake()
    {
        _instance = this;
    }

    private float _time = -1f;
    public static float GetTime()
    {
        return _instance._time;
    }

    public Text MinDisplay;
    public Text SecDisplay;
    public Text MilisecDisplay;
    private bool _started = false;

    private GameState _lastGameState = GameState.Intro;
    private void Update()
    {
        if (GlobalMics.Instance.State != _lastGameState)
        {
            if (GlobalMics.Instance.State == GameState.Started)
            {
                GameStarted();
            }
            else if (GlobalMics.Instance.State == GameState.Finished)
            {
                GameFinished();
            }
            if (GlobalMics.Instance.State == GameState.Win)
            {
                GameFinished();
            }
        }

        if (_started)
        {
            _time += Time.deltaTime;

            if (_time > 0)
            {
                int mins = Mathf.FloorToInt(_time/60);
                int sec = Mathf.FloorToInt(_time) - (mins*60);
                int mil = (Mathf.FloorToInt(_time*60)) - ((mins*60*60) + (sec*60));

                MinDisplay.text = mins.ToString();
                SecDisplay.text = sec.ToString();
                if (sec < 10)
                {
                    SecDisplay.text = "0" + SecDisplay.text;
                }
                MilisecDisplay.text = mil.ToString();
                if (mil < 10)
                {
                    MilisecDisplay.text = "0" + MilisecDisplay.text;
                }
            }
            else
            {
                MinDisplay.text = 0.ToString();
                SecDisplay.text = 0.ToString();
                MilisecDisplay.text = 0.ToString();
            }
        }
    }

    private void GameFinished()
    {
        _started = false;
    }

    private void GameStarted()
    {
        _started = true;
    }
}
