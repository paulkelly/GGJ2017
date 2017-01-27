using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    private const float OneStarThreshold = 5*60;
    private const float TwoStarThreshold = 3*60;
    private const float ThreeStarThreshold = 1.5f*60;

    public Image Star1;
    public Image Star2;
    public Image Star3;

    public Text MinDisplay;
    public Text SecDisplay;
    public Text MilisecDisplay;

    private void Update()
    {
        float time = Timer.GetTime();

        if (time > 0)
        {
            int mins = Mathf.FloorToInt(time / 60);
            int sec = Mathf.FloorToInt(time) - (mins * 60);
            int mil = (Mathf.FloorToInt(time * 60)) - ((mins * 60 * 60) + (sec * 60));

            MinDisplay.text = mins.ToString();
            SecDisplay.text = sec.ToString();
            if (sec < 10)
            {
                SecDisplay.text = "0" + SecDisplay.text;
            }
            MilisecDisplay.text = mil.ToString();
            if (mil < 10)
            {
                MilisecDisplay.text = "0" + MilisecDisplay.text;
            }

            if (time > ThreeStarThreshold)
            {
                Star3.color = Color.black;
            }
            if (time > TwoStarThreshold)
            {
                Star3.color = Color.black;
            }
            if (time > OneStarThreshold)
            {
                Star3.color = Color.black;
            }

        }
        else
        {
            MinDisplay.text = 0.ToString();
            SecDisplay.text = 0.ToString();
            MilisecDisplay.text = 0.ToString();
        }
    }
}
