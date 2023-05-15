using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrTestEnemy : ScrLived
{
    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = 100;
        this.maxHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void lifeGotZero()
    {
        Object.Destroy(this.gameObject);
    }
}
