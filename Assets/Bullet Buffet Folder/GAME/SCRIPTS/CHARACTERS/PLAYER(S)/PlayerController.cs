using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private void Start()
    {
        Start_Movimiento();
        Start_Dash();
        Start_Escudo();
        Start_Animator();
        Start_Vida();
        InicializarPowerUps();
        InicializarSSD();
        Start_Habilidad();
        Start_Equipos();
    }

    private void Update()
    {
        Update_Movimiento();
        Update_Shoot();
    }

    private void FixedUpdate()
    {
        FixedUpdate_Habilidad();
    }

    //private void SetupEquipo()
    //{
    //    jugadorText.text = "J" + gamepadIndex.ToString();
    //    if (equipo == 1)
    //    {
    //        equipoRojo.gameObject.SetActive(true);
    //        equipoAzul.gameObject.SetActive(false);
    //    }
    //    else if (equipo == 2)
    //    {
    //        equipoRojo.gameObject.SetActive(false);
    //        equipoAzul.gameObject.SetActive(true);
    //    }
    //}

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
            animator.SetTrigger("dash");
            //agregar los floats de animator
            enDash = true;
            canDash = false;
            playerHUD.dashIcon.enabled = false;
            StartCoroutine(DesactivarDash());
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
            if (actualHability == "Invulnerability")
            {
                if (!isInvulnerable)
                {
                    ActivarInvulnerabilidad();
                }

            }


            if (actualHability == "Super Speed")
            {
                if (!inSuperSpeed)
                {
                    inSuperSpeed = true;
                    SuperSpeed();
                }
            }

        }
    }

    public void Input_Hability(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Si no funciona correctamente, agregar el timetoshoot y booleanos necesarios
            if (habilidadProgreso >= 0.97f)
            {
                SuperShoot();
                habilidadProgreso = 0;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
                StartCoroutine(CargarHabilidad());
            }
            else if (habilidadProgreso >= 0.47f)
            {
                ExplosiveBullet();
                habilidadProgreso = 0;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
                StartCoroutine(CargarHabilidad());
            }

            //playerHUD.UpdateHability(habilidadProgreso);
        }
    }

    #endregion INPUT

    #region SLOT JUGADOR

    [HideInInspector] public PlayerHUD playerHUD;

    public void AsignarSlot(PlayerHUD playerHUD)
    {
        this.playerHUD = playerHUD;
    }
    #endregion SLOT JUGADOR

    #region EQUIPOS
    [SerializeField] internal int equipo;
    [SerializeField] private Image circuloEquipo;

    void Start_Equipos()
    {
        if (equipo == 1)
        {
            circuloEquipo.color = Color.red;
        }
        else if (equipo == 2)
        {
            circuloEquipo.color = Color.blue;
        }
    }

    #endregion EQUIPOS

    #region GAMEPAD
    [SerializeField] internal Gamepad gamepadIndex;
    #endregion GAMEPAD

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
        if (GameManager.EnPausa)
            return;

        if (BloquearMovimiento)
            return;

        Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
        circuloEquipo.transform.position = rotation;
        transform.LookAt(rotation);


        Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
        movement.x = moveXZ.x;
        movement.z = moveXZ.z;

        //animator.SetFloat("x", movement.x);
        //animator.SetFloat("z", movement.z);


        

        if (moveXZ == Vector3.zero)
        {
            animator.SetBool("movimiento", false);
        }
        else if (moveXZ != Vector3.zero)
        {
            animator.SetBool("movimiento", true);
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

    #region FUNCIONAMIENTO HABILIDADES

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
        hability = null;
        actualHability = hability;
        BloquearMovimiento = false;

    }
    #endregion EXPLOSIVE BULLET

    #endregion FUNCIONAMIENTO HABILIDADES

    #region GAMEPAD
    [SerializeField] internal Gamepad _gamepad;
    #endregion GAMEPAD

    #region Dash

    [Header("Dash Stats")]
    [SerializeField] private bool enDash;
    [SerializeField] private bool canDash;
    [SerializeField] private float fuerzaDash;
    [SerializeField] private float cooldownDash = 5;
    [SerializeField] private float tiempoDash = 0.25f;
    [SerializeField] private float contadorDash = 5;

    void Start_Dash()
    {
        canDash = true;
        playerHUD.dashIcon.enabled = true;
        contadorDash = 5;
        playerHUD.dashCounter.text = contadorDash.ToString();
        //GameManager.Instance.UpdateDashStatus(this, true, contadorDash);
    }

    IEnumerator DesactivarDash()
    {
        while (contadorDash > 0)
        {
            yield return new WaitForSeconds(1);
            //contadorDashText.text = contadorDash.ToString();          
            enDash = false;
            contadorDash--;
            playerHUD.dashCounter.text = contadorDash.ToString();
        }
        //contadorDashText.text = contadorDash.ToString();
        //GameManager.Instance.UpdateDashStatus(this, true, contadorDash);
        canDash = true;
        contadorDash = 5;
        playerHUD.dashCounter.text = contadorDash.ToString();
        playerHUD.dashIcon.enabled = true;
    }

    #endregion Dash

    #region Vida

    [Header("Life Stats")]
    [SerializeField] private int maxSalud = 100;
    [SerializeField] private SkinnedMeshRenderer renderer;
    internal int salud;
    //internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        playerHUD.BarraDeVida = (float)salud / maxSalud;
        //GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);

        Debug.Log("Nombre:" + this.gameObject.name + Vida);

        renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    void DeadEvent()
    {
        animator.SetTrigger("muerto");
        GameManager.Instance.DeadPlayerEventMHS(this);
    }

    public int Vida
    {
        get => salud;
        set
        {
            if (value < salud)
            {
                StartCoroutine(DañoEmisivo());
            }

            if (value <= 0)
            {
                salud = 0;
                playerHUD.BarraDeVida = salud;
                DeadEvent();
            }
            else if (value >= maxSalud)
            {
                salud = maxSalud;
            }
            else
            {
                salud = value;
            }

            //GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);
            playerHUD.BarraDeVida = (float)salud / maxSalud;
        }
    }

    private IEnumerator DañoEmisivo()
    {
        renderer.material.SetColor("_EmissionColor", Color.white * 2);
        yield return new WaitForSeconds(0.1f);
        renderer.material.SetColor("_EmissionColor", Color.black);
    }
    #endregion Vida

    #region Escudo

    [Header("Shield Stats")]
    [SerializeField] private float tiempoEscudo = 0.45f;
    [SerializeField] private float cooldownEscudo = 5;
    [SerializeField] private Transform escudo;
    [SerializeField] private int contadorEscudo = 5;
    private Vector3 diferenciaEscudo;
    private Quaternion rotacionEscudo;
    private bool canEscudo = true;

    void Start_Escudo()
    {
        //GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
        playerHUD.shieldIcon.enabled = true;
        contadorEscudo = 5;
        playerHUD.shieldCounter.text = contadorEscudo.ToString();
    }

    void ActivarEscudo()
    {
        canEscudo = false;
        escudo.gameObject.SetActive(true);
        playerHUD.shieldIcon.enabled = false;
        diferenciaEscudo = transform.forward * 2;
        escudo.position = transform.position + diferenciaEscudo;
        rotacionEscudo = transform.rotation;

        Invoke("DesactivarEscudo", tiempoEscudo);
    }

    void DesactivarEscudo()
    {
        escudo.gameObject.SetActive(false);
        StartCoroutine(CooldawnEscudo());
    }

    IEnumerator CooldawnEscudo()
    {
        while (contadorEscudo > 0)
        {
            contadorEscudo--;
            playerHUD.shieldCounter.text = contadorEscudo.ToString();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(cooldownEscudo);
        //GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
        canEscudo = true;
        contadorEscudo = 5;
        playerHUD.shieldCounter.text = contadorEscudo.ToString();
        playerHUD.shieldIcon.enabled = true;
    }

    #endregion Escudo

    #region Habilidad

    [Header("Ability Stats")]
    [SerializeField] private float cargaHabilidad;
    public float habilidadProgreso;


    void Start_Habilidad()
    {
        habilidadProgreso = 0;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
        StartCoroutine(CargarHabilidad());
    }

    void FixedUpdate_Habilidad()
    {
        if (habilidadProgreso >= 0.97f)
        {
            playerHUD.coneShotIcon.enabled = true;
        }
        else
        {
            playerHUD.coneShotIcon.enabled = false;
        }

        if (habilidadProgreso >= 0.47f)
        {
            playerHUD.explosiveShotIcon.enabled = true;
        }
        else
        {
            playerHUD.explosiveShotIcon.enabled = false;
        }
    }

    private IEnumerator CargarHabilidad()
    {
        while (habilidadProgreso < 1f)
        {
            habilidadProgreso += Time.deltaTime / cargaHabilidad; // Ajusta el tiempo de carga según sea necesario
            playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
            //GameManager.Instance.UpdatePlayerAbility(this, habilidadProgreso);

            if (habilidadProgreso == 1f)
            {
                habilidadProgreso = 1f;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso / cargaHabilidad;
                // Habilidad está completamente cargada
            }

            yield return null;
        }
    }

    void OnEnable()
    {
        // Reiniciamos el progreso de la habilidad
        habilidadProgreso = 0f;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;

        // Iniciamos la coroutine para cargar la habilidad
        StartCoroutine(CargarHabilidad());

        Start_Dash();
        Start_Escudo();
    }

    #endregion Habilidad

    #region POWER UP
    [Header("Power Up Core")]
    [SerializeField] private string actualHability = "None";
    internal string hability;

    void InicializarPowerUps()
    {
        actualHability = hability;
        DesactivarSprite();
    }

    public void SetHability(string newHability)
    {
        hability = newHability;
        actualHability = hability;

        playerHUD.EnablePowerUpIcon(actualHability);
    }

    void DesactivarSprite()
    {
        playerHUD.DisablePowerUpIcons();

    }


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
        DesactivarSprite();
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
        playerSpeed = 10f;
        superSpeed = playerSpeed;
        Invoke("DesactivarSSD", superSpeedTime);
    }

    private void DesactivarSSD()
    {
        hability = null;
        actualHability = hability;
        playerSpeed = 5f;
        superSpeed = playerSpeed;
        inSuperSpeed = false;
        DesactivarSprite();
    }
    #endregion SUPER SPEED

    #endregion POWER UP

}
