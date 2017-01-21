using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMics : MonoBehaviour
{
    private static GlobalMics _instance;
    public MicControl mic1;
    public MicControl mic2;

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

        _instance = this;
    }

    private float _player1Volume;
    public float Player1Volume
    {
        get
        {
            return _player1Volume;
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

    private void Update()
    {
        _player1Volume = mic1.loudness;
        _player2Volume = mic2.loudness;
    }
}
