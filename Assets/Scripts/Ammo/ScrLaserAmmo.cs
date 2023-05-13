using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrLaserAmmo : ScrParentAmmo
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.speed = 26f;

        this.damage = 15;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
