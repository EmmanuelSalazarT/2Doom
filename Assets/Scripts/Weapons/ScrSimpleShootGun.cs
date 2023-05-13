using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrSimpleShootGun : ScrParentWeapon
{
    // Start is called before the first frame update
    void Awake()
    {
        this.chargerSize = 8;
        this.timeToRecharge = 1.5f;
        this.shootingCadence = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
