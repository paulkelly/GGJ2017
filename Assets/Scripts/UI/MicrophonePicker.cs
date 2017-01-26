using System.Collections;
using System.Collections.Generic;
using Billygoat;
using Billygoat.InputManager.GUI;
using UnityEngine;
using UnityEngine.UI;

public class MicrophonePicker : MonoBehaviour
{
    public int Player;
    public ClickableArrowGUI LeftArrow;
    public ClickableArrowGUI RightArrow;
    public Text _text;

    //Should really do this with events
    private void Update()
    {
        if (BGMicController.Instance != null)
        {
            if (BGMicController.HasMic(Player))
            {
                _text.text = BGMicController.GetMicNName(Player);

                LeftArrow.SetEnabled(BGMicController.HasExtraMics());
                RightArrow.SetEnabled(BGMicController.HasExtraMics());
            }
            else
            {
                _text.text = "No Mic Detected";
                LeftArrow.SetEnabled(false);
                RightArrow.SetEnabled(false);
            }
        }

    }

    public void OnEnable()
    {
        if (!Application.isPlaying) return;
        if (LeftArrow != null)
        {
            LeftArrow.OnClick += LeftClicked;
        }
        if (RightArrow != null)
        {
            RightArrow.OnClick += RightClicked;
        }
    }

    public void OnDisable()
    {
        if (LeftArrow != null)
        {
            LeftArrow.OnClick = () => { };
        }
        if (RightArrow != null)
        {
            RightArrow.OnClick = () => { };
        }
    }

    private void LeftClicked()
    {
        BGMicController.SearchMicsDown(Player);
    }

    private void RightClicked()
    {
        BGMicController.SearchMicsUp(Player);
    }
}
