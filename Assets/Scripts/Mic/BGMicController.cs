using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Billygoat
{
    public class BGMicController : MonoBehaviour
    {
        #region singleton
        private static BGMicController _instance;
        public static BGMicController Instance
        {
            get { return _instance; }

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
            _instance = this;
        }
        #endregion

        public bool LogsEnabled = false;
        public const int NumberOfPlayers = 2;

        public static float Volume
        {
            get { return GetVolume(1); }
        }

        public static float GetVolume(int id)
        {
            return _instance.GetPlayerVolume(id-1);
        }

        public static int GetSensitivity(int id)
        {
            return _instance.GetPlayerSensitivity(id-1);
        }

        public static void SetSensitivity(int id, int level)
        {
            _instance.SetPlayerSensitivity(id-1, level);
        }

        private MicDetector _micDetector;
        private BGMicrophone[] _players = new BGMicrophone[NumberOfPlayers];
        private Dictionary<int, string> _playerDevices = new Dictionary<int, string>();

        private float GetPlayerVolume(int id)
        {
            if (id < NumberOfPlayers)
            {
                if (_players[id] != null)
                {
                    return _players[id].Volume;
                }
            }
            else
            {
                Debug.LogWarning("Trying to get microphone for player outside of range, currently setup for " + NumberOfPlayers + " players");
            }

            //fallback result
            return 0;
        }

        private int GetPlayerSensitivity(int id)
        {
            if (id < NumberOfPlayers)
            {
                if (_players[id] != null)
                {
                    return _players[id].SensitivityLevel;
                }
            }
            else
            {
                Debug.LogWarning("Trying to get microphone for player outside of range, currently setup for " + NumberOfPlayers + " players");
            }

            //fallback result
            return 0;
        }

        private void SetPlayerSensitivity(int id, int level)
        {
            if (id < NumberOfPlayers)
            {
                if (_players[id] != null)
                {
                    _players[id].SensitivityLevel = level;
                }
            }
            else
            {
                Debug.LogWarning("Trying to get microphone for player outside of range, currently setup for " + NumberOfPlayers + " players");
            }
        }

        private bool _initialized;
        private void Start()
        {
            if (!_initialized)
            {
                _micDetector = GetComponent<MicDetector>();
                _micDetector.OnMicrophoneConnected += DeviceConnected;
                _micDetector.OnMicrophoneDisconnected += DeviceDisconnected;
                _micDetector.Init();
                _initialized = true;
            }
        }

        private void OnDestroyed()
        {
            _micDetector.OnMicrophoneConnected = (deviceName) => { };
            _micDetector.OnMicrophoneDisconnected = (deviceName) => { };
        }

        private void DeviceConnected(string deviceName)
        {
            if (_playerDevices.ContainsValue(deviceName))
            {
                int id = 0;
                foreach (var kvp in _playerDevices)
                {
                    if (kvp.Value.Equals(deviceName))
                    {
                        id = kvp.Key;
                        break;
                    }
                }

                _playerDevices[id] = deviceName;
                _players[id] = _micDetector.GetDevice(deviceName);
                Log("Reconnected microphone for player " + (id + 1) + " : " + deviceName);
            }
            else if (_playerDevices.Count < NumberOfPlayers)
            {
                int playerId = _playerDevices.Count;
                _playerDevices.Add(playerId, deviceName);
                _players[playerId] = _micDetector.GetDevice(deviceName);
                Log("Adding microphone for player " + (playerId + 1) + " : " + deviceName);
            }
            else
            {
                for (int i = 0; i < NumberOfPlayers; i++)
                {
                    if (!_players[i].IsPlaying)
                    {
                        _playerDevices[i] = deviceName;
                        _players[i] = _micDetector.GetDevice(deviceName);
                        Log("Picked up new microphone for player " + (i+1) + " : " + deviceName);
                        break;
                    }
                }
            }
        }

        private void DeviceDisconnected(string deviceName)
        {
            if (_playerDevices.ContainsValue(deviceName))
            {
                int id = 0;
                foreach (var kvp in _playerDevices)
                {
                    if (kvp.Value.Equals(deviceName))
                    {
                        id = kvp.Key;
                        break;
                    }
                }
                foreach (var newDevice in _micDetector.GetDeviceList())
                {
                    if (!_playerDevices.ContainsValue(newDevice))
                    {
                        _playerDevices[id] = newDevice;
                        _players[id] = _micDetector.GetDevice(newDevice);
                        Log("Microphone disconncted: " + deviceName + ", player " + id + " reconnected to " + newDevice);
                        return;
                    }
                }

                Log("Microphone disconncted: " + deviceName + ", player " + id + " lost device.");
            }
            else
            {
                Log("Microphone disconncted: " + deviceName + " (not associated to any player)");
            }
        }

        private void Log(string message)
        {
            if (LogsEnabled)
            {
                Debug.Log(message);
            }
        }
    }
}