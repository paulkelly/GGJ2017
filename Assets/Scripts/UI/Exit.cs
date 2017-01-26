using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : ClickableButton
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
        GlobalMics.Instance.State = GameState.Finished;
        PauseMenu.Unpause();
    }
}
