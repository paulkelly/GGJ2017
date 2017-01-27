using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFromOptions : ClickableButton
{
    public TitleScreenUI TitleScreen;
    private void OnEnable()
    {
        OnClick += OnSubmit;
    }

    private void OnDisable()
    {
        OnClick = () => { };
    }

    private void OnSubmit()
    {
        TitleScreen.HideOptions();
    }
}
