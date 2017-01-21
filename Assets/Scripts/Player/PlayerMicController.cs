using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMicController : MonoBehaviour
{
    ManGoat _manGoat;
    private void Awake()
    {
        _manGoat = GetComponent<ManGoat>();
    }

	void Update ()
    {
        _manGoat.ManYelling = GlobalMics.Instance.Player1Yelling;
        _manGoat.ManVolume = GlobalMics.Instance.Player1Volume;

        _manGoat.GoatYelling = GlobalMics.Instance.Player2Yelling;
        _manGoat.GoatVolume = GlobalMics.Instance.Player2Volume;
    }
}
