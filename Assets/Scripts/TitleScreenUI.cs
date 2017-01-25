using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    //  public CanvasGroup Main;
    // public CanvasGroup UI;
    //  public CanvasGroup Black;
    //  public GameObject Win;
    public CanvasGroup TitleCanvas;
    public Animator GameUIAnimator;
    public CanvasGroup GameUICanvas;
    public Camera TitleScreenCamera;
    public Animator TitleAnimator;

    public CanvasGroup GameBlack;

    private bool _started;
    private bool _finished;
    private bool _win;

    private float _blackTime = 0.4f;
    private float _blackTimer = 0;

    private float _endTimer = 0;
    private float _endWaitTime = 1f;

    private void Awake()
    {
    }

    private void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private GameState _lastGameState = GameState.Intro;
    private void Update()
    {
        if(GlobalMics.Instance.State != _lastGameState)
        {
            if (GlobalMics.Instance.State == GameState.Starting)
            {
                GameStarting();
            }
            else if (GlobalMics.Instance.State == GameState.Started)
            {
                GameStarted();
            }
            else if (GlobalMics.Instance.State == GameState.Finished)
            {
                GameFinished();
            }
            if (GlobalMics.Instance.State == GameState.Win)
            {
                GameWon();
            }
        }
        _lastGameState = GlobalMics.Instance.State;

        //if (GlobalMics.Instance.State == GameState.NotStarted)
        //{


        //    if (GlobalMics.Instance.GameStarted)
        //    {
        //        _started = true;
        //        TitleScreenCamera.gameObject.SetActive(false);
        //    }
        //}
        //else if (GlobalMics.Instance.State == GameState.Starting)
        //{
        //    Black.alpha = Mathf.Clamp01(Black.alpha + 4 * Mathf.Min(Time.deltaTime, 1f / 30f));

        //    if (_blackTimer < _blackTime)
        //    {
        //        if (Black.alpha > 0.98f)
        //        {
        //            _blackTimer += Time.deltaTime;
        //        }
        //    }
        //    else if (Main.alpha > 0)
        //    {
        //        UI.alpha = 0;
        //        Main.alpha = Mathf.Clamp01(Main.alpha - Time.deltaTime);
        //    }
        //    else
        //    {
        //        TitleScreenCamera.gameObject.SetActive(false);
        //        GlobalMics.Instance.HideTitleScreen();
        //    }
        //}
        //else if (GlobalMics.Instance.State == GameState.Finished)
        //{
        //    if (!_finished)
        //    {
        //        _finished = true;
        //        StartCoroutine(RestartGame());
        //    }

        //    if (_endTimer < _endWaitTime)
        //    {
        //        _endTimer += Time.deltaTime;
        //    }
        //    else
        //    {
        //        Main.alpha = Mathf.Clamp01(Main.alpha + Time.deltaTime);
        //    }
        //}
        //else if(GlobalMics.Instance.State == GameState.Win)
        //{
        //    if (!_finished)
        //    {
        //        _finished = true;
        //       // Win.SetActive(true);
        //        StartCoroutine(Finish());
        //    }

        //    if (_endTimer < _endWaitTime)
        //    {
        //        _endTimer += Time.deltaTime;
        //    }
        //    else
        //    {
        //        _finished = true;
        //        Main.alpha = Mathf.Clamp01(Main.alpha + Time.deltaTime);
        //    }
        //}

        if ((GlobalMics.Instance.State != GameState.Finished && GlobalMics.Instance.State != GameState.Win) && Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalMics.Instance.State = GameState.Finished;
        }

        if(GlobalMics.Instance.State == GameState.Win && _win && (Input.anyKeyDown || GlobalMics.Instance.Player1Yelling || GlobalMics.Instance.Player2Yelling))
        {
            GlobalMics.Instance.State = GameState.NotStarted;
            SceneManager.LoadScene(1);
        }
    }

    private void GameStarting()
    {
        TitleAnimator.SetTrigger("Start");
        StartCoroutine(WaitForOutAnimation());
    }

    private void GameStarted()
    {
        TitleCanvas.alpha = 0;
        GameUICanvas.alpha = 1;
        GameUIAnimator.SetTrigger("Go");
        TitleScreenCamera.gameObject.SetActive(false);
    }

    private void GameFinished()
    {
        _finished = true;
        StartCoroutine(RestartGame());
    }

    private void GameWon()
    {
        _finished = true;
        StartCoroutine(RestartGame());
    }

    private IEnumerator WaitForIntro()
    {
        while (!TitleAnimator.GetCurrentAnimatorStateInfo(0).IsName("In"))
        {
            yield return null;
        }

        while (TitleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            yield return null;
        }

        GlobalMics.Instance.State = GameState.NotStarted;
    }

    private IEnumerator WaitForOutAnimation()
    {
        while (!TitleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Out"))
        {
            yield return null;
        }

        while (TitleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        GlobalMics.Instance.State = GameState.Started;
    }

    private float _fadeOutTimer = 0;
    private float _fadeOutTime = 3;
    private IEnumerator RestartGame()
    {
        Image blackImage = GameBlack.GetComponent<Image>();
        blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, 1);
        GameBlack.alpha = 0;
        while (_fadeOutTimer < _fadeOutTime)
        {
            _fadeOutTimer += Time.deltaTime;
            GameBlack.alpha = Mathf.Lerp(0, 1, _fadeOutTimer / _fadeOutTime);
            yield return null;
        }
        GameBlack.alpha = 1;

        yield return new WaitForSeconds(1f);
        GlobalMics.Instance.State = GameState.NotStarted;
        SceneManager.LoadScene(1);
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(3f);
        _win = true;
    }
}
