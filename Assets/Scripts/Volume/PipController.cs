using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipController : MonoBehaviour
{
    public int Player = 1;

    [Range(0, 1)]
    public float Volume;

    private List<VolumePip> _pips = new List<VolumePip>();
	void Start ()
    {
        foreach(VolumePip p in GetComponentsInChildren<VolumePip>())
        {
            _pips.Add(p);
        }

        for(int i=0; i<_pips.Count; i++)
        {
            SetPipColor(_pips[i], 1- ((float)i / (float)_pips.Count));
        }
    }

    private void SetPipColor(VolumePip pip, float normalPosition)
    {
        Color c = Color.yellow;
        float pos = normalPosition;
        if(normalPosition < 0.5f)
        {
            pos = normalPosition / 0.5f;
            c = Color.Lerp(Color.green, Color.yellow, normalPosition/0.5f);
        }
        else if(normalPosition > 0.5f)
        {
            pos = (normalPosition / 0.5f) - 0.5f;
            c = Color.Lerp(Color.yellow, Color.red, (normalPosition/0.5f)-1);
        }
        pip.SetColor(c);
    }

    private void Update()
    {
        if (Player == 1)
        {
            Volume = GlobalMics.Instance.Player1Volume;
        }
        else
        {
            Volume = GlobalMics.Instance.Player2Volume;
        }

        for (int i = 0; i < _pips.Count; i++)
        {
            //1-Volume because scripts are in back to front order
            bool on = ((float)i / (float)_pips.Count) >= (1 - Volume);
            if(on)
            {
                if (GlobalMics.Instance.MicControlled)
                {
                    _pips[i].SetAlpha(1);
                }
                else
                {
                    if (Player == 1)
                    {
                        if(GlobalMics.Instance.Player1Yelling)
                        {
                            _pips[i].SetAlpha(1);
                        }
                        else
                        {
                            _pips[i].SetAlpha(0.2f);
                        }
                    }
                    else
                    {
                        if (GlobalMics.Instance.Player2Yelling)
                        {
                            _pips[i].SetAlpha(1);
                        }
                        else
                        {
                            _pips[i].SetAlpha(0.2f);
                        }
                    }
                }
            }
            else
            {
                _pips[i].SetAlpha(0);
            }
        }
    }

}
