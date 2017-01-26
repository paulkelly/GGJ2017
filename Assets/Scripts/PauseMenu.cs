using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public CanvasGroup GameUI;
    private bool _paused = false;

    private void Update()
    {
        bool pausePressed = false;
        foreach(Player player in ReInput.players.AllPlayers)
        {
            pausePressed |= player.GetButtonDown(RewiredConsts.Action.Pause);
        }

        pausePressed |= ReInput.players.SystemPlayer.GetButtonDown(RewiredConsts.Action.Pause);

        if (pausePressed)
        {
            if(_paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        if(GlobalMics.Instance.State == GameState.Started)
        {
            GameUI.alpha = 0;
            _paused = true;
            Time.timeScale = 0;
            PauseMenuObject.SetActive(true);
        }
    }

    public void Unpause()
    {
        GameUI.alpha = 1;
        _paused = false;
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
    }
}
