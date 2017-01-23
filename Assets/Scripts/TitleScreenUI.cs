using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public CanvasGroup Main;
    public CanvasGroup UI;
    public CanvasGroup Black;
    public GameObject Win;

    private bool _started;
    private bool _finished;
    private bool _win;

    private float _blackTime = 0.4f;
    private float _blackTimer = 0;

    private float _endTimer = 0;
    private float _endWaitTime = 1f;

    private void Awake()
    {
        Main.alpha = 1;
    }

    private void Update()
    {
        if (GlobalMics.Instance.State == GameState.NotStarted)
        {
            if (Black.alpha > 0)
            {
                Black.alpha = Mathf.Clamp01(Black.alpha - 2 * Mathf.Min(Time.deltaTime, 1f / 30f));
            }

            if (GlobalMics.Instance.GameStarted)
            {
                _started = true;
            }
        }
        else if (GlobalMics.Instance.State == GameState.Starting)
        {
            Black.alpha = Mathf.Clamp01(Black.alpha + 4 * Mathf.Min(Time.deltaTime, 1f / 30f));

            if (_blackTimer < _blackTime)
            {
                if (Black.alpha > 0.98f)
                {
                    _blackTimer += Time.deltaTime;
                }
            }
            else if (Main.alpha > 0)
            {
                UI.alpha = 0;
                Main.alpha = Mathf.Clamp01(Main.alpha - Time.deltaTime);
            }
            else
            {
                GlobalMics.Instance.HideTitleScreen();
            }
        }
        else if (GlobalMics.Instance.State == GameState.Finished)
        {
            if (!_finished)
            {
                _finished = true;
                StartCoroutine(RestartGame());
            }

            if (_endTimer < _endWaitTime)
            {
                _endTimer += Time.deltaTime;
            }
            else
            {
                Main.alpha = Mathf.Clamp01(Main.alpha + Time.deltaTime);
            }
        }
        else if(GlobalMics.Instance.State == GameState.Win)
        {
            if (!_finished)
            {
                _finished = true;
                Win.SetActive(true);
                StartCoroutine(Finish());
            }

            if (_endTimer < _endWaitTime)
            {
                _endTimer += Time.deltaTime;
            }
            else
            {
                _finished = true;
                Main.alpha = Mathf.Clamp01(Main.alpha + Time.deltaTime);
            }
        }

        if((GlobalMics.Instance.State != GameState.Finished && GlobalMics.Instance.State != GameState.Win) && Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalMics.Instance.State = GameState.Finished;
        }

        if(GlobalMics.Instance.State == GameState.Win && _win && (Input.anyKeyDown || GlobalMics.Instance.Player1Yelling || GlobalMics.Instance.Player2Yelling))
        {
            GlobalMics.Instance.State = GameState.NotStarted;
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f);
        GlobalMics.Instance.State = GameState.NotStarted;
        SceneManager.LoadScene(1);
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(3f);
        _win = true;
    }
}
