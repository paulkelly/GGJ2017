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

        public static string GetMicNName(int id)
        {
            return _instance.GetPlayerMicName(id - 1);
        }

        public static bool HasMic(int id)
        {
            return _instance.PlayerHasMic(id - 1);
        }

        public static bool HasExtraMics()
        {
            return _instance.MicsAvailable();
        }

        public static void SearchMicsUp(int player)
        {
            _instance.FindNewMic(player-1, true);
        }

        public static void SearchMicsDown(int player)
        {
            _instance.FindNewMic(player - 1, false);
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

        private string GetPlayerMicName(int id)
        {
            if (id < NumberOfPlayers)
            {
                if (_players[id] != null)
                {
                    return _players[id].DeviceName;
                }
            }

            return "No Mic Detected";
        }

        private bool PlayerHasMic(int id)
        {
            if (id < NumberOfPlayers)
            {
                return _players[id] != null;
            }
            else
            {
                Debug.LogWarning("Trying to get microphone for player outside of range, currently setup for " + NumberOfPlayers + " players");
            }

            return false;
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
                        Log("Microphone disconncted: " + deviceName + ", player " + (id+1) + " reconnected to " + newDevice);
                        return;
                    }
                }

                _players[id] = null;
                Log("Microphone disconncted: " + deviceName + ", player " + (id+1) + " lost device.");
            }
            else
            {
                Log("Microphone disconncted: " + deviceName + " (not associated to any player)");
            }
        }

        private void FindNewMic(int playerId, bool ascending)
        {
            if (playerId >= NumberOfPlayers) return;
            if (_players[playerId] != null)
            {
                string current = _players[playerId].DeviceName;
                int currentDevice = 0;
                for (int i = 0; i < _micDetector.GetDeviceList().Length; i++)
                {
                    if (_micDetector.GetDeviceList()[i].Equals(_players[playerId].DeviceName))
                    {
                        currentDevice = i;
                        break;
                    }
                }

                for (int i = 0; i < _micDetector.GetDeviceList().Length; i++)
                {
                    if (ascending)
                    {
                        currentDevice = (currentDevice + 1)%_micDetector.GetDeviceList().Length;
                    }
                    else
                    {
                        currentDevice = (currentDevice - 1);
                        if (currentDevice < 0)
                        {
                            currentDevice = _micDetector.GetDeviceList().Length - 1;
                        }
                    }

                    bool _micAlreadyClaimed = false;
                    string newMic = _micDetector.GetDeviceList()[currentDevice];
                    foreach (var kvp in _playerDevices)
                    {
                        if (kvp.Value.Equals(_micDetector.GetDeviceList()[currentDevice]))
                        {
                            _micAlreadyClaimed = true;
                            newMic = _micDetector.GetDeviceList()[currentDevice];
                            break;
                        }
                    }

                    if (!_micAlreadyClaimed)
                    {
                        _playerDevices[playerId] = newMic;

                        _players[playerId].StopAfterDelay();
                        _players[playerId] = _micDetector.GetDevice(newMic);
                        _players[playerId].Start();


                        Log("Microphone changed: " + current + ", player " + (playerId + 1) + " reconnected to " + newMic);
                        return;
                    }

                }

            }
        }

        private bool MicsAvailable()
        {
            return _micDetector.GetDeviceList().Length > NumberOfPlayers;
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