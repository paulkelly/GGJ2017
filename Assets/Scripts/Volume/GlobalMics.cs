using Rewired;
using System.Collections;
using System.Collections.Generic;
using Billygoat;
using UnityEngine;

public enum GameState
{
    NotStarted,
    Starting,
    Intro,
    Started,
    Finished,
    Win
}

public class GlobalMics : MonoBehaviour
{
    private static GlobalMics _instance;
    public AnimationCurve ThrustCurve;
    public AnimationCurve TorqueCurve;

    private Player Player1;
    private Player Player2;

    private const float MaxYellTime = 3;

    private bool _player1ControllerCD = false;
    private float _player1YellTimer;

    private bool _player2ControllerCD = false;
    private float _player2YellTimer;

    public static GlobalMics Instance
    {
        get
        {
            return _instance;
        }

        private set { _instance = value; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Player1 = ReInput.players.GetPlayer(0);
        Player2 = ReInput.players.GetPlayer(1);

        _instance = this;
    }

    public void StartGame()
    {
        State = GameState.Starting;
    }

    public void HideTitleScreen()
    {
        State = GameState.Started;
    }

    public void EndGame()
    {
        State = GameState.Finished;
    }

    public bool GameStarted
    {
        get
        {
            return State == GameState.Started;
        }
    }

    private GameState _state;
    public GameState State
    {
        get { return _state; }

        set
        {
            _state = value;
            //Debug.Log("StateSet: " + _state);
        }
    }

    public bool MicControlled = true;
    public float MicThreshold = 0.7f;

    private float _player1Volume;
    public float Player1Volume
    {
        get
        {
            return _player1Volume;
        }
    }

    private bool _player1Yelling;
    public bool Player1Yelling
    {
        get
        {
            return _player1Yelling;
        }
    }

    private float _player2Volume;
    public float Player2Volume
    {
        get
        {
            return _player2Volume;
        }
    }

    private bool _player2Yelling;
    public bool Player2Yelling
    {
        get
        {
            return _player2Yelling;
        }
    }

    private bool _player1YellingController = false;
    private bool _player2YellingController = false;
    private float _player1ControllerVolume;
    private float _player2ControllerVolume;
    private void Update()
    {
        float player1MicVolume = BGMicController.GetVolume(1);
        float player2MicVolume = BGMicController.GetVolume(2);

        if (_player1ControllerCD)
        {
            _player1ControllerCD = !(_player1ControllerVolume < 0.1f && Player1.GetButtonDown(RewiredConsts.Action.Yell));
        }
        if (_player2ControllerCD)
        {
            _player2ControllerCD = !(_player2ControllerVolume < 0.1f && Player2.GetButtonDown(RewiredConsts.Action.Yell));
        }

        _player1YellingController = !_player1ControllerCD && Player1.GetButton(RewiredConsts.Action.Yell);

        if (_player1YellingController)
        {
            _player1ControllerVolume = Mathf.Clamp01(_player1ControllerVolume + Time.deltaTime * 2);
            _player1YellTimer = Mathf.Clamp(_player1YellTimer + (Time.deltaTime * _player1ControllerVolume), 0, MaxYellTime);

            if (_player1YellTimer >= MaxYellTime)
            {
                _player1ControllerCD = true;
                _player1YellingController = false;
            }
        }
        else
        {
            if (_player1ControllerCD)
            {
                _player1ControllerVolume = Mathf.Clamp01(_player1ControllerVolume - Time.deltaTime * 3);
            }
            else
            {
                _player1ControllerVolume = Mathf.Clamp01(_player1ControllerVolume - Time.deltaTime * 4);
            }
            _player1YellTimer = Mathf.Clamp(_player1YellTimer - (Time.deltaTime * 3f), 0, MaxYellTime);
        }

        _player2YellingController = !_player2ControllerCD && Player2.GetButton(RewiredConsts.Action.Yell);

        if (_player2YellingController)
        {
            _player2ControllerVolume = Mathf.Clamp01(_player2ControllerVolume + Time.deltaTime * 2);
            _player2YellTimer = Mathf.Clamp(_player2YellTimer + (Time.deltaTime * _player2ControllerVolume), 0, MaxYellTime);

            if (_player2YellTimer >= MaxYellTime)
            {
                _player2ControllerCD = true;
                _player2YellingController = false;
            }
        }
        else
        {
            if (_player2ControllerCD)
            {
                _player2ControllerVolume = Mathf.Clamp01(_player2ControllerVolume - Time.deltaTime * 3);
            }
            else
            {
                _player2ControllerVolume = Mathf.Clamp01(_player2ControllerVolume - Time.deltaTime * 4);
            }
            _player2YellTimer = Mathf.Clamp(_player2YellTimer - (Time.deltaTime * 3f), 0, MaxYellTime);
        }


        _player1Volume = Mathf.Max(_player1ControllerVolume, player1MicVolume);
        _player2Volume = Mathf.Max(_player2ControllerVolume, player2MicVolume);

        if (_player1ControllerVolume > player1MicVolume)
        {
            _player1Yelling = _player1YellingController;
        }
        else
        {
            _player1Yelling = _player1Volume > MicThreshold;
        }

        if (_player2ControllerVolume > player2MicVolume)
        {
            _player2Yelling = _player2YellingController;
        }
        else
        {
            _player2Yelling = _player2Volume > MicThreshold;
        }
    }
}
