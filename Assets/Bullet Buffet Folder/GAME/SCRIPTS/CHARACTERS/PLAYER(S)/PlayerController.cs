using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    private void Start()
    {
        Start_Movimiento();
        Start_Dash();
        Start_Animator();
        Start_Vida();
        InicializarPowerUps();
        InicializarSSD();
    }

    private void Update()
    {
        Update_Movimiento();
        Update_Shoot();
        Update_Escudo();
    }

    #region INPUT

    public void Input_Axis1(InputAction.CallbackContext context)
    {
        Vector2 v2 = context.ReadValue<Vector2>();
        axis1 = new Vector3(v2.x, 0, v2.y);
    }

    public void Input_Axis2(InputAction.CallbackContext context)
    {
        Vector2 v2 = context.ReadValue<Vector2>();
        axis2 = new Vector3(v2.x, 0, v2.y);
    }

    public void Input_Dash(InputAction.CallbackContext context)
    {
        if (!enDash && canDash)
        {
            enDash = true;
            canDash = false;
            Invoke("DesactivarDash", tiempoDash);
        }

    }

    public void Input_Escudo(InputAction.CallbackContext context)
    {
        if (canEscudo)
        {
            ActivarEscudo();
        }
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        BulletShoot();
    }

    public void Input_Pausa(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print(name);
            GameManager.Pausa(this);
        }
    }

    public void Input_PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Se ejecuto el Power Up");

            if (actualHability == "Super Shoot")
            {
                boolSS = true;
                timeToShoot++;
                BloquearMovimiento = true;

                if (timeToShoot > 3 && boolSS)
                {
                    SuperShoot();
                }
            }


            if (actualHability == "Explosive Bullet")
            {
                boolEB = true;
                timeToShoot++;
                BloquearMovimiento = true;

                if (timeToShoot > 3 && boolEB)
                {
                    ExplosiveBullet();
                }
            }


            if (actualHability == "Invulnerability")
            {
                if (!isInvulnerable)
                {
                    ActivarInvulnerabilidad();
                }

            }


            if (actualHability == "Super Speed")
            {
                if(!inSuperSpeed)
                {
                    inSuperSpeed = true;
                    SuperSpeed();
                }
            }

        }
    }

    #endregion INPUT

    #region Movimiento & Rotacion

    [Header("Movement Stats")]
    [SerializeField] internal float playerSpeed = 5f;
    [SerializeField] private float smoothRotacion;
    private bool groundedPlayer;
    private CharacterController controller;
    private Vector3 movement = Vector3.zero;
    private Vector3 axis1 = Vector3.zero;
    private Vector3 axis2 = Vector3.zero;

    internal bool BloquearMovimiento = false;

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update_Movimiento()
    {
        Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
        transform.LookAt(rotation);

        if (BloquearMovimiento)
            return;

        Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
        movement.x = moveXZ.x;
        movement.z = moveXZ.z;



        if (GameManager.EnPausa)
            return;

        if (moveXZ == Vector3.zero)
        {
            animator.SetBool("movimiento", false);
        }
        else if (moveXZ != Vector3.zero)
        {
            animator.SetBool("movimiento", true);
        }

        if (enDash)
        {
            animator.SetBool("dash", true);
        }
        else
        {
            animator.SetBool("dash", false);
        }

        if (controller.isGrounded)
        {
            movement.y = 0f;
            groundedPlayer = true;
        }
        else
        {
            movement.y -= Time.deltaTime;
            groundedPlayer = false;
        }

        controller.Move(movement * Time.deltaTime);
    }

    #endregion Movimiento & Rotacion

    #region Dash

    [Header("Dash Stats")]
    [SerializeField] private bool enDash;
    [SerializeField] private bool canDash;
    [SerializeField] private float fuerzaDash;
    [SerializeField] private float cooldownDash = 5;
    [SerializeField] private float tiempoDash = 0.25f;

    void Start_Dash()
    {
        canDash = true;
    }

    void DesactivarDash()
    {
        StartCoroutine(CooldawnDash());
    }

    IEnumerator CooldawnDash()
    {
        enDash = false;
        yield return new WaitForSeconds(cooldownDash);
        canDash = true;
    }

    #endregion Dash

    #region Disparo

    [Header("Shoot Stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float cooldown;
    private float cont = 0;

    [Header("Shoot Objects")]
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawn;

    void Update_Shoot()
    {
        cont -= Time.deltaTime;
    }

    void BulletShoot()
    {
        if (cont <= 0)
        {
            Transform clon = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            clon.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            Destroy(clon.gameObject, 3);
            cont = cooldown;
        }

    }

    #endregion Disparo

    #region Vida

    [Header("Life Stats")]
    [SerializeField] private int maxSalud = 300;
    [SerializeField] private Image BarraSalud;
    internal int salud;
    //internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        BarraSalud.fillAmount = salud;
    }

    void DeadEvent()
    {
        animator.SetTrigger("muerto");
        GameManager.Instance.DeadPlayerEventMHS(this);
        Debug.Log("Murio: " + this.gameObject.name);
    }

    public int Vida
    {
        get => salud;
        set
        {
            if (value <= 0)
            {
                salud = 0;
                BarraSalud.fillAmount = (float)salud / maxSalud;
                DeadEvent();
            }
            else if (value >= maxSalud)
            {
                salud = maxSalud;
                BarraSalud.fillAmount = (float)salud / maxSalud;
            }
            else
            {
                salud = value;
                BarraSalud.fillAmount = (float)salud / maxSalud;
            }

            BarraSalud.fillAmount = (float)salud / maxSalud;
        }
    }
    #endregion Vida

    #region Escudo
    [Header("Shield Stats")]

    [SerializeField] private float tiempoEscudo;
    [SerializeField] private float cooldownEscudo;
    [SerializeField] private Transform escudo;
    private Vector3 diferenciaEscudo;
    private Quaternion rotacionEscudo;
    private bool canEscudo = true;

    void Update_Escudo()
    {
        escudo.position = transform.position + diferenciaEscudo;
        escudo.rotation = rotacionEscudo;
    }

    void ActivarEscudo()
    {
        canEscudo = false;
        escudo.gameObject.SetActive(true);
        diferenciaEscudo = transform.forward;
        escudo.position = transform.position + diferenciaEscudo;
        rotacionEscudo = transform.rotation;

        Invoke("DesactivarEscudo", tiempoEscudo);
    }

    void DesactivarEscudo()
    {
        escudo.gameObject.SetActive(false);
        Invoke("FinalizarCooldown", cooldownEscudo);
    }

    private void FinalizarCooldown()
    {
        canEscudo = true;
    }
    #endregion Escudo

    #region ANIMATOR
    private Animator animator;

    internal Animator anim;

    void Start_Animator()
    {
        animator = GetComponent<Animator>();
        anim = animator.GetComponent<Animator>();
        animator.SetTrigger("spawn");
    }
    #endregion ANIMATOR

    #region EQUIPOS
    internal int equipo;
    #endregion EQUIPOS

    #region POWER UP
    [Header("Power Up Core")]
    [SerializeField] private string actualHability = "None";
    [SerializeField] private int timeToShoot;
    internal string hability;

    void InicializarPowerUps()
    {
        timeToShoot = 0;
    }

    public void SetHability(string newHability)
    {
        hability = newHability;
        actualHability = hability;
    }

    #region SUPER SHOOT
    [Header("Super Shoot")]
    [SerializeField] private GameObject bulletSSPrefab;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private bool boolSS = false;
    public float spreadSpeed = 10f;

    void SuperShoot()
    {
        foreach (Transform shootPoint in shootPoints)
        {
            GameObject bulletInstance = Instantiate(bulletSSPrefab, shootPoint.position, shootPoint.rotation);
            SuperShoot bulletScript = bulletInstance.GetComponent<SuperShoot>();
            bulletScript.spreadSpeed = spreadSpeed;
        }

        Invoke("DesactivarSS", 0.25f);
    }

    private void DesactivarSS()
    {
        Debug.Log("Si se desactivo Super Shoot");
        hability = null;
        actualHability = hability;
        BloquearMovimiento = false;
        timeToShoot = 0;
        boolSS = false;
    }
    #endregion SUPER SHOOT

    #region EXPLOSIVE BULLET
    [Header("Explosive Bullet")]
    [SerializeField] private GameObject balaExplosiva;
    [SerializeField] private Transform spawnBalaExplosiva;
    [SerializeField] private bool boolEB = false;

    void ExplosiveBullet()
    {
        Debug.Log("Se instancio la bala explosiva");
        GameObject bala = Instantiate(balaExplosiva, spawnBalaExplosiva.position, spawnBalaExplosiva.rotation);

        Invoke("DesactivarES", 0.25f);
    }

    private void DesactivarES()
    {
        Debug.Log("Si se desactivo Explosive Shoot");
        hability = "None";
        BloquearMovimiento = false;
        timeToShoot = 0;
    }
    #endregion EXPLOSIVE BULLET

    #region INVULNERABILIDAD
    [Header("Invulnerabilidad")]
    [SerializeField] private float isInvunerableTime = 5f;
    [SerializeField] internal bool isInvulnerable = false;
    //[SerializeField] private bool invulnerable;

    private void ActivarInvulnerabilidad()
    {
        isInvulnerable = true;
        Debug.Log("Se activo la invulnerabilidad");
        Invoke("DesactivarInvulnerabilidad", isInvunerableTime);
    }

    private void DesactivarInvulnerabilidad()
    {
        Debug.Log("Si se desactivo Invulnerabilidad");
        hability = null;
        actualHability = hability;
        isInvulnerable = false;
    }
    #endregion INVULNERABILIDAD

    #region SUPER SPEED
    [Header("Super Speed")]
    [SerializeField] private float superSpeed;
    [SerializeField] private float superSpeedTime = 5f;
    public bool inSuperSpeed = false;

    void InicializarSSD()
    {
        superSpeed = playerSpeed;
    }
    
    private void SuperSpeed()
    {
        playerSpeed = 100f;
        superSpeed = playerSpeed;
        StartCoroutine("DesactivarSSD", superSpeedTime);
    }

    private void DesactivarSSD()
    {
        Debug.Log("Si se desactivo Super Speed");
        hability = null;
        actualHability = hability;
        playerSpeed = 5f;
        superSpeed = playerSpeed;
        inSuperSpeed = false;
    }
    #endregion SUPER SPEED

    #endregion POWER UP

}
