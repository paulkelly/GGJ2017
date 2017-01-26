using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Billygoat
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMicSource : MonoBehaviour
    {
        public int Position;

        private AudioSource _audioSource;
        private string _deviceName;
        private bool _isPlaying;
        private bool _hasStartedPlaying;

        private const int SampleSize = 44100;

        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        public string DeviceName
        {
            get { return _deviceName; }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Setup(string microphoneName)
        {
            _deviceName = microphoneName;
            _audioSource.clip = Microphone.Start(_deviceName, true, 1, SampleSize);
            _isPlaying = true;
            StartCoroutine(StartClip());
        }

        public void StartRecording()
        {
            _audioSource.clip = Microphone.Start(_deviceName, true, 1, SampleSize);
            _isPlaying = true;
            StartCoroutine(StartClip());
        }

        public void StopRecording()
        {
            //Debug.Log(DeviceName + " recording stopped.");
            _isPlaying = false;
            _hasStartedPlaying = false;
            _audioSource.clip = null;
        }

        private void Restart()
        {
            StopRecording();
            StartRecording();
        }

        private IEnumerator StartClip()
        {
            while (!(Microphone.GetPosition(_deviceName) > 0))
            {
                yield return null;
            }
            _audioSource.Play();
            _hasStartedPlaying = true;
            //Debug.Log(DeviceName + " recording started.");
        }

        [Range(0, 1)]
        public float _volume;

        public float Volume
        {
            get
            {
                if (_isPlaying && _audioSource.clip != null)
                {
                    return _volume;
                }
                return 0;
            }
        }

        void Update()
        {
            if (_hasStartedPlaying)
            {
                float nextVolume = ReadAudioSourceVolume();
                if (nextVolume >= 0)
                {
                    nextVolume = Mathf.Clamp01(nextVolume);
                    _volume = Mathf.Clamp01(_volume + (nextVolume - _volume)*(Time.unscaledDeltaTime*30));
                }
            }
            else
            {
                _volume = 0;
            }
        }

        private int _readHead = 0;
      //  private int _failedAttempts;
      //  private const int FailsBeforeRestart = 30;
        private float ReadAudioSourceVolume()
        {
            int writeHead = Microphone.GetPosition(_deviceName);
            Position = writeHead;
            int nFloatsToGet = (_audioSource.clip.samples + writeHead - _readHead)%_audioSource.clip.samples;
            if (nFloatsToGet < 1)
            {
                //nothing to read

                if (!Microphone.IsRecording(_deviceName))
                {
                    Restart();
                }

                //_failedAttempts++;
                //if (_failedAttempts > FailsBeforeRestart)
                //{
                //    _failedAttempts = 0;
                //    Restart();
                //}

                return -1;
            }
          //  _failedAttempts = 0;
            float[] _buffer = new float[nFloatsToGet];

            _audioSource.clip.GetData(_buffer, _readHead);
            _readHead = (_readHead + nFloatsToGet)%_audioSource.clip.samples;

            //float min = Mathf.Infinity;
            //float max = Mathf.NegativeInfinity;
            //for(int i=0; i< nFloatsToGet; i++)
            //{
            //    min = Mathf.Min(min, _buffer[i]);
            //    max = Mathf.Max(max, _buffer[i]);
            //}
            //return max - min;

            float sum = 0;
            for (int i = 0; i < nFloatsToGet; i++)
            {
                sum += _buffer[i]*_buffer[i];
            }
            return Mathf.Sqrt(sum/(float) nFloatsToGet);
        }
    }
}