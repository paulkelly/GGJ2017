using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSFX : MonoBehaviour
{
    private AudioSource _audioSource;

    public AudioClip[] Clips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Crash(float volume = 1)
    {
        _audioSource.clip = Clips[Random.Range(0, Clips.Length)];
        _audioSource.pitch = Random.Range(0.92f, 1.08f);
        _audioSource.volume = Mathf.Clamp01(Random.Range(volume - 0.05f, volume + 0.05f));
        _audioSource.Play();
    }

}
