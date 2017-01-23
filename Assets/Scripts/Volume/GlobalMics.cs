using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NotStarted,
    Starting,
    Started,
    Finished,
    Win
}

public class GlobalMics : MonoBehaviour
{
    private static GlobalMics _instance;
    public MicControl mic1;
    public MicControl mic2;
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

    private void Update()
    {
        if (MicControlled)
        {
            bool buttonPress = Player1.GetButton(RewiredConsts.Action.Yell);
            buttonPress |=  Player2.GetButton(RewiredConsts.Action.Yell);
           // buttonPress |= Mathf.Abs(Player1.GetAxis(RewiredConsts.Action.Volume)) > 0.2f;
           // buttonPress |= Mathf.Abs(Player2.GetAxis(RewiredConsts.Action.Volume)) > 0.2f;

            if (buttonPress)
            {
                float lookForMics = Mathf.Min(mic1.loudness, mic2.loudness);
                MicControlled = lookForMics > 0.0005f;
            }

            _player1Yelling = mic1.loudness > MicThreshold;
            if (_player1Yelling)
            {
                _player1Volume = mic1.loudness;
            }
            else
            {
                _player1Volume = 0;
            }
            _player2Yelling = mic2.loudness > MicThreshold;
            if (_player2Yelling)
            {
                _player2Volume = mic2.loudness;
            }
            else
            {
                _player2Volume = 0;
            }
        }
        else
        {

            float lookForMics = Mathf.Min(mic1.loudness, mic2.loudness);
            MicControlled = lookForMics > 0.0005f;

            //float player1Stick = Player1.GetAxis(RewiredConsts.Action.Volume);
            //if(Mathf.Abs(player1Stick) > 0.2f)
            //{
            //    _player1Volume = Mathf.Clamp(_player1Volume + (player1Stick * Time.deltaTime), 0, 1);
            //}

            if(_player1ControllerCD)
            {
                _player1ControllerCD = !(_player1Volume < 0.1f && Player1.GetButtonDown(RewiredConsts.Action.Yell));
            }
            if (_player2ControllerCD)
            {
                _player2ControllerCD = !(_player2Volume < 0.1f && Player2.GetButtonDown(RewiredConsts.Action.Yell));
            }

            _player1Yelling = !_player1ControllerCD && Player1.GetButton(RewiredConsts.Action.Yell);

            if(_player1Yelling)
            {
                _player1Volume = Mathf.Clamp01(_player1Volume + Time.deltaTime * 2);
                _player1YellTimer = Mathf.Clamp(_player1YellTimer + (Time.deltaTime * _player1Volume), 0, MaxYellTime);

                if(_player1YellTimer >= MaxYellTime)
                {
                    _player1ControllerCD = true;
                    _player1Yelling = false;
                }
            }
            else
            {
                if (_player1ControllerCD)
                {
                    _player1Volume = Mathf.Clamp01(_player1Volume - Time.deltaTime * 3);
                }
                else
                {
                    _player1Volume = Mathf.Clamp01(_player1Volume - Time.deltaTime * 4);
                }
                _player1YellTimer = Mathf.Clamp(_player1YellTimer - (Time.deltaTime * 3f), 0, MaxYellTime);
            }

            //float player2Stick = Player2.GetAxis(RewiredConsts.Action.Volume);
            //if (Mathf.Abs(player2Stick) > 0.2f)
            //{
            //    _player2Volume = Mathf.Clamp(_player2Volume + (player2Stick * Time.deltaTime), 0, 1);
            //}
            _player2Yelling = !_player2ControllerCD && Player2.GetButton(RewiredConsts.Action.Yell);

            if (_player2Yelling)
            {
                _player2Volume = Mathf.Clamp01(_player2Volume + Time.deltaTime * 2);
                _player2YellTimer = Mathf.Clamp(_player2YellTimer + (Time.deltaTime * _player2Volume), 0, MaxYellTime);

                if (_player2YellTimer >= MaxYellTime)
                {
                    _player2ControllerCD = true;
                    _player2Yelling = false;
                }
            }
            else
            {
                if (_player2ControllerCD)
                {
                    _player2Volume = Mathf.Clamp01(_player2Volume - Time.deltaTime * 3);
                }
                else
                {
                    _player2Volume = Mathf.Clamp01(_player2Volume - Time.deltaTime * 4);
                }
                _player2YellTimer = Mathf.Clamp(_player2YellTimer - (Time.deltaTime * 3f), 0, MaxYellTime);
            }
        }
    }
}
