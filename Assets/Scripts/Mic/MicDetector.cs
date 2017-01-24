using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Billygoat
{
    public class MicDetector : MonoBehaviour
    {
        public GameObject MicrophoneSource;

        public Action<string> OnMicrophoneConnected = (deviceName) => { };
        public Action<string> OnMicrophoneDisconnected = (deviceName) => { };

        private Dictionary<string, BGMicrophone> _devices = new Dictionary<string, BGMicrophone>();

        private bool _initialized = false;
        internal void Init()
        {
            SearchForDevices();
            _initialized = true;
        }

        private float _timer = 0;
        private float _searchTime = 1;
        private void Update()
        {
            if (_initialized)
            {
                _timer += Time.deltaTime;
                if (_timer > _searchTime)
                {
                    _timer = 0;
                    SearchForDevices();
                }
            }
        }

        public string[] GetDeviceList()
        {
            return _devices.Keys.ToArray();
        }

        public BGMicrophone GetDevice(string deviceName)
        {
            BGMicrophone mic;
            _devices.TryGetValue(deviceName, out mic);
            return mic;
        }

        private void SearchForDevices()
        {
            foreach (var device in Microphone.devices)
            {
                if (_devices.ContainsKey(device))
                {
                    BGMicrophone mic;
                    _devices.TryGetValue(device, out mic);
                    if (!mic.IsPlaying)
                    {
                        ReconnectDevice(device);
                    }
                }
                else
                {
                    ConnectNewDevice(device);
                }
            }

            foreach (var device in _devices.Keys)
            {
                if (!Microphone.devices.Contains(device))
                {
                    BGMicrophone mic;
                    _devices.TryGetValue(device, out mic);
                    if (mic.IsPlaying)
                    {
                        DisconnectDevice(device);
                    }
                }
            }
        }

        private void ConnectNewDevice(string deviceName)
        {
            GameObject instance = GameObject.Instantiate(MicrophoneSource);
            instance.transform.parent = transform;
            instance.name = deviceName;
            BGMicSource source = instance.GetComponent<BGMicSource>();
            source.Setup(deviceName);

            BGMicrophone newMic = new BGMicrophone(source);

            _devices.Add(deviceName, newMic);

            //Debug.Log("Device Connected: " + deviceName);
            OnMicrophoneConnected.Invoke(deviceName);
        }

        private void ReconnectDevice(string deviceName)
        {
            BGMicrophone mic;
            _devices.TryGetValue(deviceName, out mic);

            if (mic != null)
            {
                mic.Start();
            }

            //Debug.Log("Device Reconnected: " + deviceName);
            OnMicrophoneConnected.Invoke(deviceName);
        }

        private void DisconnectDevice(string deviceName)
        {
            BGMicrophone mic;
            _devices.TryGetValue(deviceName, out mic);

            if (mic != null)
            {
                mic.Stop();
            }

           //Debug.Log("Device Disconnected: " + deviceName);
            OnMicrophoneDisconnected.Invoke(deviceName);
        }

    }
}