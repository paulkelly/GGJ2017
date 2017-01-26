using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Continue : ClickableButton
{
    public PauseMenu PauseMenu;

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
        PauseMenu.Unpause();
    }
}
