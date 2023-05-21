using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayer : ScrLived
{
    #region Atributos
    public float speed = 9;

    public float timeToMeleeAttack = 1f;
    public float distanceToMeleeAttack = 3.4f;
    public float meleeDamage = 1;

    #endregion
    #region Variables de control
    private float vSpeed = 0;
    private float hSpeed = 0;
    public int currentNumberWeapon = 0;
    public bool isMeleeAtacking = false;
    private float coldDownTimeMeleeAttack = 10f;
    private bool enableMeleeAttack = true;

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

        #region Acciones sin ataques melee
        if (!this.isMeleeAtacking)
        {
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
        }
        #endregion


        if (Input.GetKeyDown(KeyCode.V) && this.enableMeleeAttack)
        {
            this.startMeleeAttack();
        }



        
        this.updateWeaponPosition();
    }

    private void FixedUpdate()
    {
        base.checkLife();

        this.movement();
        if(!this.isMeleeAtacking)
        {
            this.chageAngle();                
        }
        
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

    private void startMeleeAttack()
    {
        if(!this.isMeleeAtacking)
        {
            this.isMeleeAtacking = true;
            this.enableMeleeAttack = false;
            StartCoroutine(this.meleeAttack());
        }
    }

    private IEnumerator meleeAttack()
    {
        int iterations = 36;
        float grades = 360 / iterations;
        float time = 1 / iterations;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanceToMeleeAttack);

        // Iterar sobre los colliders y hacer algo con ellos
        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject != this.gameObject)
            {
                ScrLived liveObject = collider.GetComponent<ScrLived>();
                if(liveObject != null)
                {
                    liveObject.takeDamage(meleeDamage,TypeDamageConstant.none);

                    Vector2 direction =  collider.transform.position - this.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Debug.Log(direction);
                    collider.GetComponent<Rigidbody2D>().drag = 0.05f;
                    collider.GetComponent<Rigidbody2D>().mass = 3;

                    collider.GetComponent<Rigidbody2D>().AddForce(direction * 20 , ForceMode2D.Impulse);

                    liveObject.restoreLinearDrag();
                }
            }
        }

        for(int i = 0; i < iterations; i++)
        {

            Quaternion currentRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles + new Vector3(0f, 0f, 360/iterations));
            transform.rotation = newRotation;
            yield return new WaitForSecondsRealtime(0.005f);
        }

        this.isMeleeAtacking = false;

        StartCoroutine(this.startColdownMeleeAttack());
    }

    private IEnumerator startColdownMeleeAttack()
    {
        yield return new WaitForSeconds(this.coldDownTimeMeleeAttack);

        this.enableMeleeAttack = true;
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

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToMeleeAttack);
    }
    #endregion
}
