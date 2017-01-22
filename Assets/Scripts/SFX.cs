using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public int Player = 1;

    public bool ManYelling;
    public bool GoatYelling;

    public float ManVolume;
    public float GoatVolume;

    public List<AudioClip> _clips = new List<AudioClip>();

    private AudioSource _audioSource;
    private bool _wasYelling = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
        ManYelling = GlobalMics.Instance.Player1Yelling;
        ManVolume = GlobalMics.Instance.Player1Volume;

        GoatYelling = GlobalMics.Instance.Player2Yelling;
        GoatVolume = GlobalMics.Instance.Player2Volume;

        if(Player == 1)
        {
            if(ManYelling && !_wasYelling)
            {
                PickNewClip();
            }

            if(ManYelling)
            {
                _audioSource.volume = ManVolume;
            }
            else
            {
                _wasYelling = false;
                _audioSource.Stop();
            }
        }
        else
        {
            if(GoatYelling && !_wasYelling)
            {
                PickNewClip();
            }

            if (GoatYelling)
            {
                _audioSource.volume = GoatVolume;
            }
            else
            {
                _wasYelling = false;
                _audioSource.Stop();
            }
        }
    }

    private void PickNewClip()
    {
        _wasYelling = true;
        _audioSource.clip = _clips[Random.Range(0, _clips.Count)];
        _audioSource.pitch = Random.Range(0.9f, 1.1f);

        _audioSource.Play();
    }
}
