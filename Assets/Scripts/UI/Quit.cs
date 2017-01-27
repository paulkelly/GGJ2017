
using System.Net.Mime;
using UnityEngine;

public class Quit : ClickableButton
{
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
        Application.Quit();
    }
}
