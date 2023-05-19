using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrParentAmmo : MonoBehaviour
{
    public float damage;
    public float speed;
    public int damageType;

    public GameObject ExplosionEffect;
    public Transform Corner;

    public bool destroyOnCollision = true;

    public Rigidbody2D rb;


    // Start is called before the first frame update
    protected virtual  void Start()
    {
        this.damageType = TypeDamageConstant.normal;
        this.destroyOnCollision = true;
        this.rb = GetComponent<Rigidbody2D>();    
        this.rb.AddForce(this.transform.up * this.speed , ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 movement = transform.forward * speed;

        //this.rb.MovePosition( this.rb.position + (movement * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            ScrLived livedItem = collision.gameObject.GetComponent<ScrLived>();
            if(livedItem != null)
            {
                livedItem.takeDamage(this.damage,this.damageType);

                this.extraColitionEvent(livedItem);
                if(this.destroyOnCollision)
                {
                    this.destroyEvent();
                }
            }
        }
    }

    private void extraColitionEvent(ScrLived livedItem)
    {

    }

    private void destroyEvent()
    {
        if(this.ExplosionEffect != null)
        {
            Vector3 position = this.Corner!=null?this.Corner.position : this.transform.position;
            GameObject explosion = Instantiate(this.ExplosionEffect,position,Quaternion.identity);
            Object.Destroy(explosion,1);
        }
        Object.Destroy(this.gameObject);
    }

}
