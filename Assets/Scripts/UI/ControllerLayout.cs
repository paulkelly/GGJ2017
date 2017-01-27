using Billygoat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerLayout : MonoBehaviour
{
    public int Player;
    public ClickableArrowGUI LeftArrow;
    public ClickableArrowGUI RightArrow;
    public Text _text;

    public GameObject AButton;
    public GameObject XButton;

    private const string OnePlayerString = "Gamepad 1";
    private const string TwoPlayerString = "Gamepad 2";

    //Should really do this with events
    private void RefreshUI()
    {
        if (GlobalMics.Instance != null)
        {
            if (GlobalMics.Instance.ControlScheme == ControlScheme.TwoPlayer)
            {
                _text.text = TwoPlayerString;
                AButton.SetActive(true);
                XButton.SetActive(false);

            }
            else
            {
                _text.text = OnePlayerString;
                AButton.SetActive(false);
                XButton.SetActive(true);
            }
        }

    }

    public void OnEnable()
    {
        if (!Application.isPlaying) return;
        if (LeftArrow != null)
        {
            LeftArrow.OnClick += OnClick;
        }
        if (RightArrow != null)
        {
            RightArrow.OnClick += OnClick;
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

    private void OnClick()
    {
        if (GlobalMics.Instance != null)
        {
            if (GlobalMics.Instance.ControlScheme == ControlScheme.TwoPlayer)
            {
                GlobalMics.Instance.ControlScheme = ControlScheme.SinglePlayer;
            }
            else
            {
                GlobalMics.Instance.ControlScheme = ControlScheme.TwoPlayer;
            }
        }

        RefreshUI();
    }
}