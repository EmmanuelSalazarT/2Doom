using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrLived : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool showHealthBarInMaxHealth;
    public Vector3 healthBarOffset;

    public GameObject healthBar;

    protected void OnEnable()
    {

        this.checkInitialPositionHealthBar();
    }

    protected void LateUpdate()
    {
        this.setHealthBarPosition();
    }

    public void checkInitialPositionHealthBar()
    {
        if(this.healthBar != null)
        {
            this.healthBarOffset =  this.transform.InverseTransformPoint(this.healthBar.transform.position);//this.healthBar.transform.position;
        }
        
    }
    public void setHealthBarPosition()
    {
        if(this.healthBar != null)
        {
            this.healthBar.transform.position = this.transform.position + this.healthBarOffset;
        }
    }
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
        if(this.healthBar != null)
        {
            ScrHealthBar healthBarScript = this.healthBar.GetComponent<ScrHealthBar>();
            if(healthBarScript != null)
            {
                healthBarScript.setMaxHealth((int)this.maxHealth);
                
                healthBarScript.setHealth((int)this.maxHealth);
            }
        }
    }

    public void takeDamage(float damage)
    {
        this.currentHealth -= damage;
        if(this.healthBar != null)
        {
            ScrHealthBar healthBarScript = this.healthBar.GetComponent<ScrHealthBar>();
            if(healthBarScript != null)
            {   
                healthBarScript.setHealth((int)this.currentHealth);
            }
        }
        if(this.currentHealth <= 0)
        {
            this.lifeGotZero();
        }
    }

    public abstract void lifeGotZero();
}
