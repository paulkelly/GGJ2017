using Rewired;
using System.Collections;
using System.Collections.Generic;
using Billygoat;
using UnityEngine;

public enum GameState
{
    Intro,
    NotStarted,
    Starting,
    Started,
    Finished,
    Win
}

public enum ControlScheme
{
    SinglePlayer,
    TwoPlayer
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

    private GameState _state = GameState.Intro;
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

    private ControlScheme _controlScheme = ControlScheme.TwoPlayer;

    public ControlScheme ControlScheme
    {
        get { return _controlScheme; }
        set
        {
            _controlScheme = value;

            if (_controlScheme == ControlScheme.SinglePlayer)
            {
                Player1.controllers.maps.SetMapsEnabled(true, "SinglePlayer");
                Player1.controllers.maps.SetMapsEnabled(false, "MultiPlayer");
                Player2.controllers.maps.SetMapsEnabled(false, "MultiPlayer");
            }
            else
            {
                Player1.controllers.maps.SetMapsEnabled(false, "SinglePlayer");
                Player1.controllers.maps.SetMapsEnabled(true, "MultiPlayer");
                Player2.controllers.maps.SetMapsEnabled(true, "MultiPlayer");
            }
        }
    }

    private bool _usingControllers1;
    private bool _usingControllers2;
    public bool Player1UsingController
    {
        get { return _usingControllers1; }
    }
    public bool Player2UsingController
    {
        get { return _usingControllers2; }
    }

    public float Player1Stamina
    {
        get { return _player1YellTimer/MaxYellTime; }
    }

    public float Player2Stamina
    {
        get { return _player2YellTimer / MaxYellTime; }
    }

    private bool _player1YellingController = false;
    private bool _player2YellingController = false;
    private float _player1ControllerVolume;
    private float _player2ControllerVolume;
    private float _player1ControllerVolumePaused;
    private float _player2ControllerVolumePaused;
    private void Update()
    {
        float player1MicVolume = BGMicController.GetVolume(1);
        float player2MicVolume = BGMicController.GetVolume(2);

        if (Time.timeScale == 0)
        {
            bool player1Yelling = Player1.GetButton(RewiredConsts.Action.ManYell);
            bool player2Yelling = Player1.GetButton(RewiredConsts.Action.GoatYell) || Player2.GetButton(RewiredConsts.Action.GoatYell);

            if (player1Yelling)
            {
                _player1ControllerVolumePaused = Mathf.Clamp01(_player1ControllerVolumePaused + Time.unscaledDeltaTime*2);
            }
            else
            {
                _player1ControllerVolumePaused = Mathf.Clamp01(_player1ControllerVolumePaused - Time.unscaledDeltaTime * 4);
            }
            if (player2Yelling)
            {
                _player2ControllerVolumePaused = Mathf.Clamp01(_player2ControllerVolumePaused + Time.unscaledDeltaTime * 2);
            }
            else
            {
                _player2ControllerVolumePaused = Mathf.Clamp01(_player2ControllerVolumePaused - Time.unscaledDeltaTime * 4);
            }

            _player1Volume = Mathf.Max(_player1ControllerVolumePaused, player1MicVolume);
            _player2Volume = Mathf.Max(_player2ControllerVolumePaused, player2MicVolume);

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

            return;
        }

        if (_player1ControllerCD)
        {
            _player1ControllerCD = !(_player1ControllerVolume < 0.1f && Player1.GetButton(RewiredConsts.Action.ManYell));
        }
        if (_player2ControllerCD)
        {
            _player2ControllerCD = !(_player2ControllerVolume < 0.1f && (Player1.GetButton(RewiredConsts.Action.GoatYell) || Player2.GetButton(RewiredConsts.Action.GoatYell)));
        }

        _player1YellingController = !_player1ControllerCD && Player1.GetButton(RewiredConsts.Action.ManYell);

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

        _player2YellingController = !_player2ControllerCD && (Player1.GetButton(RewiredConsts.Action.GoatYell) || Player2.GetButton(RewiredConsts.Action.GoatYell));

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
            if (_player1Volume > 0.1)
            {
                _usingControllers1 = true;
            }
        }
        else
        {
            _player1Yelling = _player1Volume > MicThreshold;
            if (_player1Volume > 0.1)
            {
                _usingControllers1 = false;
            }
        }

        if (_player2ControllerVolume > player2MicVolume)
        {
            _player2Yelling = _player2YellingController;
            if (_player2Volume > 0.1)
            {
                _usingControllers2 = true;
            }
        }
        else
        {
            _player2Yelling = _player2Volume > MicThreshold;
            if (_player2Volume > 0.1)
            {
                _usingControllers2 = false;
            }
        }
    }
}
