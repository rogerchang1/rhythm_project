using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void setMaxFever(float fever)
    {
        slider.maxValue = fever;
    }
    public void setFever(float fever)
    {
        slider.value = fever;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public float getBarValue()
    {
        return (float)slider.value;
    }

}
