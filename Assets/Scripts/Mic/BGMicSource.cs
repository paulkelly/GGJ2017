using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            StartRecording();
        }

        public void StartRecording()
        {
            _isPlaying = true;
            _readHead = 0;
            try
            {
                _audioSource.clip = Microphone.Start(_deviceName, true, 1, SampleSize);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to start recording " + DeviceName + " : " + e.Message + "\n" + e.StackTrace);
            }
            StopAllCoroutines();
            StartCoroutine(StartClip());
        }

        public void StopRecording()
        {
            //Debug.Log(DeviceName + " recording stopped.");
            Microphone.End(_deviceName);
            _isPlaying = false;
            _hasStartedPlaying = false;
            _audioSource.clip = null;
        }

        private void Restart()
        {
            if (Microphone.devices.Contains(_deviceName))
            {
                StopRecording();
                StartRecording();
            }
        }

        private float startTime = 0;
        private float timer = 1;
        private IEnumerator StartClip()
        {
            startTime = 0;
            while (!(Microphone.IsRecording(_deviceName)) && startTime < timer)
            {
                startTime += Time.unscaledDeltaTime;
                yield return null;
            }

            //Try to start recording again after waiting 1 sec without any progress
            if (startTime >= timer)
            {
                StartRecording();
                yield break;
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
            //_isRecording = Microphone.IsRecording(_deviceName);
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