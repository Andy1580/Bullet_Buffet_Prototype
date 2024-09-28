using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region PLAYERCONTROLLER CORE

    private void Awake()
    {
        InicializarGamepad();
    }

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
        Start_Disparo();

        BloquearMovimiento = false;

        GameObject clone = Instantiate(vfxRespanPlayer, transform.position, transform.rotation);
        Destroy(clone, 1.5f);
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
    #endregion PLAYERCONTROLLER CORE

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
            direccionDash = axis1.normalized;
            animator.SetTrigger("dash");
            animator.SetFloat("xdash", movement.x);
            animator.SetFloat("zdash", movement.z);
            enDash = true;
            canDash = false;
            playerHUD.dashIcon.enabled = false;
            Invoke("FinalizarDash", 0.1f);
            StartCoroutine(HabilitarDash());
        }

    }

    public void Input_Escudo(InputAction.CallbackContext context)
    {
        if (canEscudo)
        {
            animator.SetTrigger("escudo");
            ActivarEscudo();
        }
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (canShoot)
        {
            BulletShoot();
        }
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
            if (habilidadProgreso >= 1f)
            {
                //ExplosiveBullet();
                habilidadProgreso = 0;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
                StartCoroutine(CargarHabilidad());
            }
            /*
            else if (habilidadProgreso >= 0.47f)
            {
                //SuperShoot();
                habilidadProgreso = 0;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
                StartCoroutine(CargarHabilidad());
            }
            */
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
    private Vector3 direccionDash;

    [SerializeField] internal bool BloquearMovimiento = false;

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update_Movimiento()
    {


        if (BloquearMovimiento)
            return;

        if (!enDash)
        {

            Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
            circuloEquipo.transform.position = rotation;
            transform.LookAt(rotation);


            Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
            movement.x = moveXZ.x;
            movement.z = moveXZ.z;

            animator.SetFloat("xmov", movement.x);
            animator.SetFloat("zmov", movement.z);

        }
        else
        {
            movement = direccionDash * fuerzaDash;
        }


        //if (GameManager.EnPausa)
        //    return;


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
    private bool canShoot;

    [Header("Shoot Objects")]
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawn;

    void Start_Disparo()
    {
        canShoot = true;
    }

    void Update_Shoot()
    {
        cont -= Time.deltaTime;
    }

    void BulletShoot()
    {
        if (cont <= 0)
        {
            AudioManager.instance.PlaySound("disparojugador");
            Transform clon = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            clon.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            DañoVida bullet = clon.GetComponent<DañoVida>();
            bullet.Inicializar(this);
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
    /*
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
    */
    #endregion SUPER SHOOT

    #region EXPLOSIVE BULLET
    /*
    [Header("Explosive Bullet")]
    [SerializeField] private GameObject balaExplosiva;
    [SerializeField] private Transform spawnBalaExplosiva;
    [SerializeField] private bool boolEB = false;

    void ExplosiveBullet()
    {
        GameObject bala = Instantiate(balaExplosiva, spawnBalaExplosiva.position, spawnBalaExplosiva.rotation);

        Invoke("ResetearHabilidad", 0.25f);
    }

    private void ResetearHabilidad()
    {
        hability = null;
        actualHability = hability;
    }
    */
    #endregion EXPLOSIVE BULLET

    #endregion FUNCIONAMIENTO HABILIDADES

    #region GAMEPAD
    
    [SerializeField] internal Gamepad _gamepad;

    [Header("Gamepad Core")]
    [SerializeField] private float frecuenciaMaximaDaño = 0.5f;
    [SerializeField] private float frecuenciaMinimaDaño = 0.5f;
    [SerializeField] private float frecuenciaMaximaHabilidad = 0.2f;
    [SerializeField] private float frecuenciaMinimaHabilidad = 0.2f;
    [SerializeField] private float tiempoDeVibracionDaño = 0.5f;
    [SerializeField] private float tiempoDeVibracionHabilidad = 0.1f;
    private PlayerInput _playerInput;

    private void InicializarGamepad()
    {
        _playerInput = GetComponent<PlayerInput>();
        _gamepad = _playerInput.devices.OfType<Gamepad>().FirstOrDefault();
    }
    #endregion GAMEPAD

    #region Dash

    [Header("Dash Stats")]
    [SerializeField] private bool enDash;
    [SerializeField] private bool canDash;
    [SerializeField] private float fuerzaDash = 30;
    [SerializeField] private float cooldownDash = 5;
    [SerializeField] private float tiempoEnDash = 0.25f;
    [SerializeField] private float contadorDash = 5;

    void Start_Dash()
    {
        canDash = true;
        playerHUD.dashIcon.enabled = true;
        contadorDash = 5;
        playerHUD.dashCounter.text = contadorDash.ToString();
    }

    void FinalizarDash()
    {
        enDash = false;
    }

    IEnumerator HabilitarDash()
    {
        while (contadorDash > 0)
        {
            yield return new WaitForSeconds(1);
            contadorDash--;
            playerHUD.dashCounter.text = contadorDash.ToString();
        }

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
    internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        playerHUD.BarraDeVida = (float)salud / maxSalud;
        //GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);
        muerto = false;
        Debug.Log("Nombre:" + this.gameObject.name + Vida);

        renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    void DeadEvent()
    {
        animator.SetTrigger("muerto");
        AudioManager.instance.PlaySound("muertejugador");
        muerto = true;
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
                DamageVibration(frecuenciaMinimaDaño, frecuenciaMaximaDaño);
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
        canShoot = false;
        escudo.gameObject.SetActive(true);
        playerHUD.shieldIcon.enabled = false;
        diferenciaEscudo = transform.forward * 2;
        escudo.position = transform.position + diferenciaEscudo;
        rotacionEscudo = transform.rotation;

        Invoke("DesactivarEscudo", tiempoEscudo);
    }

    void DesactivarEscudo()
    {
        canShoot = true;
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


    public void Start_Habilidad()
    {
        habilidadProgreso = 0;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
        StartCoroutine(CargarHabilidad());
    }

    void FixedUpdate_Habilidad()
    {
        if (habilidadProgreso >= 1f)
        {
            _gamepad.SetMotorSpeeds(frecuenciaMinimaHabilidad, frecuenciaMaximaHabilidad);
            Invoke("StopVibration", tiempoDeVibracionHabilidad);
            //playerHUD.explosiveShotIcon.enabled = true;
        }
        /*
        else
        {
            playerHUD.explosiveShotIcon.enabled = false;
        }

        if (habilidadProgreso >= 0.47f)
        {
            playerHUD.coneShotIcon.enabled = true;

        }
        else
        {
            playerHUD.coneShotIcon.enabled = false;
        }
        */
    }

    private IEnumerator CargarHabilidad()
    {

        while (habilidadProgreso < 1f)
        {

            if (muerto)
            {
                yield break;
            }

            habilidadProgreso += Time.deltaTime / cargaHabilidad;
            playerHUD.BarraDeHabilidad = (float)habilidadProgreso;

            if (habilidadProgreso == 1f)
            {
                habilidadProgreso = 1f;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso / cargaHabilidad;
            }

            yield return null;
        }


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

    #region VFX RESPAWN

    [Header("VFX RESPAWN")]
    [SerializeField] GameObject vfxRespanPlayer;
    #endregion VFX RESPAWN

    #region EXTRAS

    void OnEnable()
    {
        // Reiniciamos el progreso de la habilidad
        habilidadProgreso = 0f;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;

        canEscudo = true;
        actualHability = hability;
        muerto = false;

        canShoot = true;
        // Iniciamos la coroutine para cargar la habilidad
        StartCoroutine(CargarHabilidad());

        Start_Dash();
        Start_Escudo();

        BloquearMovimiento = false;

        GameObject clone = Instantiate(vfxRespanPlayer, transform.position, transform.rotation);
        Destroy(clone, 1.5f);

    }

    private void OnDestroy()
    {
        Debug.Log("El objeto " + gameObject.name + " ha sido destruido.");
    }

    private void DamageVibration(float min, float max)
    {
        _gamepad.SetMotorSpeeds(min, max);
        Invoke("StopVibration", 0.5f);
    }

    private void StopVibration()
    {
        _gamepad.SetMotorSpeeds(0f, 0f);
    }
    #endregion EXTRAS
}
