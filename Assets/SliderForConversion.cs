using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderForConversion : MonoBehaviour
{
    public Slider timerSlider;

    public void ShowFraction(float fraction)
    {
        timerSlider.value = fraction;
    }
}