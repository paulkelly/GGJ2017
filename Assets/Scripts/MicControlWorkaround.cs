using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MicControlWorkaround : MonoBehaviour
{
    private bool _firstTime = true;
    private void Start()
    {
        if (_firstTime)
        {
            _firstTime = false;
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
