using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrSimpleAmmo : ScrParentAmmo
{
    // Start is called before the first frame update
    protected override void Start()
    {
        this.speed = 20f;
        base.Start();

        this.damage = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
