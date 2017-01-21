using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMics : MonoBehaviour
{
    private static GlobalMics _instance;
    public MicControl mic1;
    public MicControl mic2;

    private Player Player1;
    private Player Player2;

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

        Player1 = ReInput.players.GetPlayer(0);
        Player2 = ReInput.players.GetPlayer(1);

        _instance = this;
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
            float player1Stick = Player1.GetAxis(RewiredConsts.Action.Volume);
            if(Mathf.Abs(player1Stick) > 0.2f)
            {
                _player1Volume = Mathf.Clamp(_player1Volume + (player1Stick * Time.deltaTime), 0, 1);
            }
            _player1Yelling = Player1.GetButton(RewiredConsts.Action.Yell);

            float player2Stick = Player2.GetAxis(RewiredConsts.Action.Volume);
            if (Mathf.Abs(player2Stick) > 0.2f)
            {
                _player2Volume = Mathf.Clamp(_player2Volume + (player2Stick * Time.deltaTime), 0, 1);
            }
            _player2Yelling = Player2.GetButton(RewiredConsts.Action.Yell);
        }
    }
}
