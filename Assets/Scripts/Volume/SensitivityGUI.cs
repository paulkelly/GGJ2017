using System.Collections;
using System.Collections.Generic;
using Billygoat;
using Billygoat.InputManager.GUI;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityGUI : MonoBehaviour
{
    public int Player;
    public ClickableArrowGUI LeftArrow;
    public ClickableArrowGUI RightArrow;
    public CanvasGroupFader NoMicGroup;
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    //Should really do this with events
    private void Update()
    {
        if (BGMicController.Instance != null)
        {
            if (BGMicController.HasMic(Player))
            {
                if (NoMicGroup.Visible)
                {
                    NoMicGroup.Visible = false;
                }

                int sensitivity = BGMicController.GetSensitivity(Player);
                LeftArrow.SetEnabled(sensitivity > 0);
                RightArrow.SetEnabled(sensitivity < 5);
                _text.text = sensitivity.ToString();
            }
            else
            {
                if (!NoMicGroup.Visible)
                {
                    NoMicGroup.Visible = true;
                }

                LeftArrow.SetEnabled(false);
                RightArrow.SetEnabled(false);
                _text.text = "";
            }
        }

    }

    public void OnEnable()
    {
        if (!Application.isPlaying) return;
        if (LeftArrow != null)
        {
            LeftArrow.OnClick += LeftClicked;
            if (BGMicController.Instance != null)
            {
                int sensitivity = BGMicController.GetSensitivity(Player);
                LeftArrow.SetEnabled(sensitivity > 0);
            }
        }
        if (RightArrow != null)
        {
            RightArrow.OnClick += RightClicked;
            if (BGMicController.Instance != null)
            {
                int sensitivity = BGMicController.GetSensitivity(Player);
                RightArrow.SetEnabled(sensitivity < 5);
            }
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
        BGMicController.SetSensitivity(Player, BGMicController.GetSensitivity(Player)-1);
        int sensitivity = BGMicController.GetSensitivity(Player);
        LeftArrow.SetEnabled(sensitivity > 0);
        RightArrow.SetEnabled(sensitivity < 5);
    }

    private void RightClicked()
    {
        BGMicController.SetSensitivity(Player, BGMicController.GetSensitivity(Player)+1);
        int sensitivity = BGMicController.GetSensitivity(Player);
        LeftArrow.SetEnabled(sensitivity > 0);
        RightArrow.SetEnabled(sensitivity < 5);
    }
}
