using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayer : ScrLived
{
    #region Atributos
    public float speed = 9;

    #endregion
    #region Variables de control
    private float vSpeed = 0;
    private float hSpeed = 0;
    public int currentNumberWeapon = 0;

    #endregion

    #region Objetos
    public Transform body;
    public Rigidbody2D rigidBody;
    
    public Transform mainGun;
    public Transform secondGun;

    public Transform target;

    private GameObject currentWeapon = null;
    #endregion

    #region estructuras
    private enum nameWeapons
    {
        Simple = 1,
        Laser = 2,
    }

    //public int ammoForSimple = 120;
    //public int ammoForLaser = 40;

    public Dictionary<int, GameObject> weapons;
    public Dictionary<int, int> ammo;
    public Dictionary<int, int> ammoInWeaponsCharger;

    #endregion

    #region eventos de unity
    private void Awake()
    {
        this.setMaxLife(50);

        this.weapons = new Dictionary<int, GameObject>()
        {
            { 1, Resources.Load<GameObject>("Prefabs/Weapons/SimpleShootGun") },
            { 2, Resources.Load<GameObject>("Prefabs/Weapons/LaserGun") },
        };
        this.ammo = new Dictionary<int, int>()
        {
            {1, 120 },
            {2, 40 },
        };
        this.ammoInWeaponsCharger = new Dictionary<int, int>();
    }

    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.changeWeapon((int)ScrPlayer.nameWeapons.Simple);


        
        //SOlo para debug
        this.rechargeMainWeapon();

    }

    // Update is called once per frame
    void Update()
    {
        vSpeed = Input.GetAxis("Vertical");
        hSpeed = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Fire1"))
        {
            // Lógica de disparo
            this.shotMainWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.rechargeMainWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            this.changeWeapon((int)ScrPlayer.nameWeapons.Simple);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            this.changeWeapon((int)ScrPlayer.nameWeapons.Laser);
        }



        
        this.updateWeaponPosition();
    }

    private void FixedUpdate()
    {
        base.checkLife();

        this.movement();
        this.chageAngle();
    }
    #endregion

    public override void lifeGotZero()
    {
        Debug.Log("es zero");
    }

    #region funciones del objeto jugador
    private void chageAngle()
    {
        Vector2 direction = this.target.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        this.rigidBody.rotation = angle;
    }

    private void movement()
    {
        Vector2 movement = new Vector2(this.hSpeed,this.vSpeed);

        this.rigidBody.MovePosition( this.rigidBody.position + (movement * this.speed * Time.fixedDeltaTime));
    }

    #endregion

    #region funciones de armas
    private void updateWeaponPosition()
    {
        this.currentWeapon.transform.position = mainGun.position;
        this.currentWeapon.transform.rotation = transform.rotation;
    }

    private void rechargeMainWeapon()
    {

        if(this.currentWeapon == null)
        {
            Debug.LogError("No hay arma principal");
            return;
        }
        ScrParentWeapon weaponScript = this.currentWeapon.GetComponent<ScrParentWeapon>();
        if(weaponScript == null)
        {
            Debug.LogError("No se obtiene el script de la arma");
            return ;
        }
        /**
         * 
         * Faltan validaciones de AMMO
         */
        int remainingAmmo = weaponScript.getChargerSize() - weaponScript.getCurrentAmmoInCharger();
        int ammoToRecharge = this.ammo[this.currentNumberWeapon]>= remainingAmmo? remainingAmmo:this.ammo[this.currentNumberWeapon];
        bool resultRecharge = weaponScript.recharge(ammoToRecharge);
        if(resultRecharge)
        {
            this.ammo[this.currentNumberWeapon] -= ammoToRecharge;
        }
        
    }

    private void shotMainWeapon()
    {
        if(this.currentWeapon == null)
        {
            return;
        }
        ScrParentWeapon weaponScript = this.currentWeapon.GetComponent<ScrParentWeapon>();
        if(weaponScript == null)
        {
            return ;
        }
        bool resultShot = weaponScript.shot(this.transform.rotation);
        if(!resultShot)
        {
            this.rechargeMainWeapon();
        }
    }

    private void changeWeapon(int newWeapon)
    {
        if(this.currentNumberWeapon == newWeapon)
        {
            return;
        }
        if(this.currentWeapon!=null)
        {
            ScrParentWeapon scrCurrentWeapon = this.currentWeapon.GetComponent<ScrParentWeapon>();
            if(scrCurrentWeapon != null)
            {
                this.ammoInWeaponsCharger[this.currentNumberWeapon] = scrCurrentWeapon.getCurrentAmmoInCharger();
            }

            Object.Destroy(this.currentWeapon);
        }
        GameObject weapon = Instantiate(this.weapons[newWeapon]);
        this.currentNumberWeapon = newWeapon;
        this.currentWeapon = weapon;

        if(this.ammoInWeaponsCharger != null)
        {
            if(this.ammoInWeaponsCharger.ContainsKey(newWeapon))
            {
                ScrParentWeapon scrCurrentWeapon = this.currentWeapon.GetComponent<ScrParentWeapon>();
                if(scrCurrentWeapon != null)
                {
                    scrCurrentWeapon.recharge(this.ammoInWeaponsCharger[newWeapon]);
                }
            }
        }

        // Puedes establecer la posición y rotación del prefab si lo deseas
        this.currentWeapon.transform.position = mainGun.position;
        this.currentWeapon.transform.rotation = transform.rotation;
        int playerIndex = this.transform.GetSiblingIndex();
    
        // Establecer el índice del objeto "weapon" para que esté por encima del "Player"
        weapon.transform.SetSiblingIndex(playerIndex + 1);
    }

    #endregion
}
