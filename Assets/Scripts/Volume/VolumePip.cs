using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumePip : MonoBehaviour
{
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetColor(Color c)
    {
        _image.color = c;
        Hide();
    }

    public void Show()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }

    public void Hide()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }
}
