using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public int maxHealth;
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        maxHealth = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }
    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public int getHealth()
    {
        return (int)slider.value;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
}
