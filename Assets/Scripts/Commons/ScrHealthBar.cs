using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrHealthBar : MonoBehaviour
{
    public Slider slider;

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void setMaxHealth(int maxHealth)
    {
        this.slider.maxValue = maxHealth;
        this.slider.value = maxHealth;
    }

    public void setHealth(int health)
    {
        this.slider.value = health;
    }

}
