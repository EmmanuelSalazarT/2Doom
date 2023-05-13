using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrParentAmmo : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    public float damage;

    // Start is called before the first frame update
    protected virtual  void Start()
    {
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

}
