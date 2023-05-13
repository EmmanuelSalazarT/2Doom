using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrParentWeapon : MonoBehaviour
{
    public float timeToRecharge;
    public float shootingCadence;
    public int chargerSize;
    public int currentAmmoInCharger;

    public GameObject ammoType;
    public Transform pipe;


    public bool isRechargin = false;
    public bool cooldownToShot = false;

    public bool shot(Quaternion rotation)
    {
        if(this.cooldownToShot)
        {
            return true;
        }
        if(this.currentAmmoInCharger <= 0)
        {
            return false;
        }
        if(this.ammoType == null)
        {
            Debug.LogError("No hay tipo de munición");

            return false;
        }

        GameObject bullet = Instantiate(ammoType,this.pipe.position,rotation);
        this.currentAmmoInCharger -=1;

        this.cooldownToShot = true;
        StartCoroutine(this.startCooldownToShot());

        return true;
    }

    public IEnumerator startCooldownToShot()
    {
        yield return new WaitForSeconds(this.shootingCadence);

        this.cooldownToShot = false;
    }

    public int getChargerSize()
    {
        return this.chargerSize; 
    }

    public int getCurrentAmmoInCharger()
    {
        return this.currentAmmoInCharger;
    }

    public bool recharge(int ammo)
    {
        if(this.isRechargin)
        {
            return false;
        }
        this.currentAmmoInCharger = ammo;
        this.isRechargin = true;
        StartCoroutine(this.rechargingTime());
        return true;
    }

    private IEnumerator rechargingTime()
    {
        yield return new WaitForSeconds(this.timeToRecharge);

        this.isRechargin=false;
    }
}
