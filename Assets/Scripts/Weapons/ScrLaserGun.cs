using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrLaserGun : ScrParentWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        this.chargerSize = 20;
        this.timeToRecharge = 5f;
        this.shootingCadence = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
