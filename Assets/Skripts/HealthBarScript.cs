using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public HealthComponent health;

    public Gradient gradient;
    public RawImage fill;

    public void UpdateMaxHealth()
    {
        slider.maxValue = health.maxHealth;
        slider.value = health.health;

        fill.color = gradient.Evaluate(1f);
    }
    public void UpdateHealth()
    {
        slider.value = health.health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
