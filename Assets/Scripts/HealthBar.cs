using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Class to handle HealthBar
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Declaring varliables
    /// </summary>
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthCounter;

    /// <summary>
    /// Sets health bar value
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        //Sets slider value to health paramater
        slider.value = health;

        healthCounter.text = health.ToString();

        // Evaluates gradient at current health, normalizedValue converts health to a scale from 0 to 1
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Sets max value for health bar
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(int health)
    {
        // Sets slider's max value to the paramaterized health value
        slider.maxValue = health;
    }
}
