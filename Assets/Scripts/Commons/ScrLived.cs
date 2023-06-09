using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrLived : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool showHealthBarInMaxHealth;
    public Vector3 healthBarOffset;
    public int typeDamageVulnerability = TypeDamageConstant.none;

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

    public void takeDamage(float damage, int typeDamage)
    {
        float finalDamage;
        if(this.typeDamageVulnerability != TypeDamageConstant.none)
        {
            if(this.typeDamageVulnerability == typeDamage) // Si el tipo de da�o recibido es igual al de vulnerabilidad, recibe el da�o completo
            {
                finalDamage = damage;
            }
            else
            {
                finalDamage = damage * 0.70f; // Sino, recibe como un 70% del da�o
            }
        }
        else
        {
            finalDamage = damage;
        }


        this.currentHealth -= finalDamage;
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

        StartCoroutine(this.blinkEffect());
    }

    private IEnumerator blinkEffect()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        //for(int i =1;i<=3;i+=1)
        //{
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                float alpha = 0.1f;//i%2==0?1f:0.1f;
                // Obtener el color actual del SpriteRenderer
                Color color = spriteRenderer.color;

                // Establecer el nuevo alfa en el color
                color.a = alpha;

                // Establecer el nuevo color en el SpriteRenderer
                spriteRenderer.color = color;
            }
            yield return new WaitForSeconds(0.3f);
        //}
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Obtener el color actual del SpriteRenderer
            Color color = spriteRenderer.color;

            // Establecer el nuevo alfa en el color
            color.a = 1;

            spriteRenderer.color = color;
        }
    }

    public void restoreLinearDrag()
    {
        StartCoroutine(this.corroutineRestoreLinearDrag());
    }

    private IEnumerator corroutineRestoreLinearDrag()
    {
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<Rigidbody2D>().drag = 1000000;
        this.GetComponent<Rigidbody2D>().mass = 1000000;
    }

    public abstract void lifeGotZero();
}
