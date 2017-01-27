using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Billygoat
{
    public class BGMicrophone
    {
        private string _name;
        private BGMicSource _source;

        public BGMicrophone(BGMicSource source)
        {
            _source = source;
            if (_source != null)
            {
                _name = _source.DeviceName;
            }
        }

        private readonly float[] BaseSensitivity = new float[]
        {
            0.5f,
            1,
            2,
            3,
            5,
            8
        };

        private int _sensitivtyLevel = 3;
        public int SensitivityLevel
        {
            get { return _sensitivtyLevel; }

            set { _sensitivtyLevel = Mathf.Clamp(value, 0, 5); }
        }
        private float Sensitivity
        {
            get { return BaseSensitivity[_sensitivtyLevel]; }
        }

        public string DeviceName { get { return _name; } }

        public float Volume
        {
            get
            {
                if (_source == null)
                {
                    Debug.LogError("Audiosource for microphone " + _name + " is null.");
                    return 0;
                }
                return Mathf.Clamp01(_source.Volume*Sensitivity);
            }
        }

        public bool IsPlaying
        {
            get { return _source.IsPlaying; }
        }

        public void Start()
        {
            _source.StartRecording();
        }

        public void Stop()
        {
            _source.StopRecording();
        }

        public void StopAfterDelay()
        {
            _source.StopRecording(true);
        }
    }
}