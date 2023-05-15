using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrLived : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool showHealthBarInMaxHealth;

    // Update is called once per frame
    public void checkLife()
    {
        if(this.currentHealth <= 0)
        {
            this.lifeGotZero();
        }
    }

    public void setMaxLife(float newMaxLife)
    {
        this.maxHealth = newMaxLife;
        this.currentHealth = newMaxLife;
    }

    public void takeDamage(float damage)
    {
        this.currentHealth -= damage;
        if(this.currentHealth <= 0)
        {
            this.lifeGotZero();
        }
    }

    public abstract void lifeGotZero();
}
