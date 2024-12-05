using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region PLAYERCONTROLLER CORE

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
        //Start_Disparo();
        personaje = this.gameObject.name;

        GameObject clone = Instantiate(vfxRespanPlayer, transform.position, transform.rotation);
        Destroy(clone, 1.5f);
    }

    private void Update()
    {
        Update_Movimiento();
        //Update_Shoot();
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
        if (!enDash && canDash && !escudo.gameObject.activeSelf && !muerto)
        {
            vfxDash.Play();
            direccionDash = axis1.normalized;
            animator.SetTrigger("dash");
            animator.SetFloat("xdash", movement.x);
            animator.SetFloat("zdash", movement.z);
            enDash = true;
            canDash = false;
            playerHUD.dashIcon.enabled = false;

            AudioManager.instance.PlaySound("dash");

            Invoke("FinalizarDash", 0.1f);
            StartCoroutine(HabilitarDash());
        }

    }

    public void Input_Escudo(InputAction.CallbackContext context)
    {
        if (canEscudo && !muerto)
        {
            ActivarEscudo();
        }
    }

    //public void Input_Disparo(InputAction.CallbackContext context)
    //{
    //    if (canShoot)
    //    {
    //        BulletShoot();
    //    }
    //}

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
        if (context.performed && !muerto)
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
            if (habilidadDisponible && !escudo.gameObject.activeSelf && !muerto)
            {
                //ExplosiveBullet();
                ActivarHabilidad();
                habilidadDisponible = false;
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

    private PlayerHUD playerHUD;

    public PlayerHUD PlayerHUD
    {
        set
        {
            playerHUD = value;
            playerHUD.gameObject.SetActive(true);
        }
    }
    #endregion SLOT JUGADOR

    #region EQUIPOS
    [SerializeField] internal int equipo;
    [SerializeField] private Image circuloEquipo;

    void Start_Equipos()
    {
        if (equipo == 1)
        {
            circuloEquipo.color = Color.magenta;
        }
        else if (equipo == 2)
        {
            circuloEquipo.color = Color.green;
        }
    }

    #endregion EQUIPOS

    #region Movimiento & Rotacion

    [Header("Movement Stats")]
    [SerializeField] internal float playerSpeed = 5f;
    [SerializeField] private float smoothRotacion = 5f;
    private bool groundedPlayer;
    private CharacterController controller;
    private Vector3 movement = Vector3.zero;
    private Vector3 axis1 = Vector3.zero;
    private Vector3 axis2 = Vector3.zero;
    private Vector3 direccionDash;
    private Vector3 ultimaDireccion = Vector3.forward;

    [SerializeField] internal bool BloquearMovimiento = false;
    [SerializeField] internal bool BloquearRotacion = false;

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update_Movimiento()
    {


        if (BloquearMovimiento)
        {
            movement = Vector3.zero;
            animator.SetFloat("xmov", movement.x);
            animator.SetFloat("zmov", movement.z);
            return;
        }

        if (!enDash)
        {

            Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
            movement.x = moveXZ.x;
            movement.z = moveXZ.z;

            animator.SetFloat("xmov", movement.x);
            animator.SetFloat("zmov", movement.z);

            if (!BloquearRotacion)
            {
                if (axis2 != Vector3.zero)
                {
                    ultimaDireccion = axis2.normalized;
                }
                Quaternion targetRotation = Quaternion.LookRotation(ultimaDireccion);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothRotacion * Time.deltaTime);
                Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
                circuloEquipo.transform.position = rotation;
            }

            if (escudo.gameObject.activeSelf)
            {
                animator.SetFloat("xescudo", movement.x);
                animator.SetFloat("zescudo", movement.z);
            }


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
    /*
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
            Da�oEscopeta bullet = clon.GetComponent<Da�oEscopeta>();
            bullet.Inicializar(this);
            Destroy(clon.gameObject, 3);
            cont = cooldown;
        }

    }
    */
    #endregion Disparo

    #region ANIMATOR

    internal Animator animator;

    void Start_Animator()
    {
        animator = GetComponent<Animator>();
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

    private void AsignarGamepad(int gamepadId)
    {
        // Encontrar y asignar el Gamepad
        _gamepad = Gamepad.all.FirstOrDefault(gp => gp.deviceId == gamepadId);
        print("Este personaje " + this.name + " lo controla el gamepad: " + _gamepad);
        if (_gamepad != null)
        {
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput != null)
            {
                _playerInput.user.UnpairDevices();  // Eliminar dispositivos previamente emparejados
                InputUser.PerformPairingWithDevice(_gamepad, _playerInput.user);
                Debug.Log($"Gamepad emparejado correctamente con el jugador {gameObject.name}");
            }
        }
    }

    public void MovePositionToRespawn(Transform respawn)
    {
        transform.position = respawn.position;
        transform.rotation = respawn.rotation;
        Debug.Log($"{name} movido a respawn en {respawn.position}");
    }

    public void SetGamepad(Gamepad gamepad)
    {
        if (gamepad == null)
        {
            Debug.LogWarning("SetGamepad: No se ha asignado un gamepad válido para este jugador.");
            return;
        }

        _gamepad = gamepad; // Asignamos el Gamepad recibido

        // Obtener el componente PlayerInput y asegurarnos de que esté inicializado
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput != null)
        {
            // Desvinculamos cualquier dispositivo actual emparejado con el usuario
            if (_playerInput.user.valid)
            {
                _playerInput.user.UnpairDevices();
            }

            // Emparejamos el Gamepad actual con un nuevo InputUser usando el método estático
            InputUser.PerformPairingWithDevice(_gamepad, _playerInput.user);
            Debug.Log($"SetGamepad: Gamepad {gamepad.deviceId} emparejado con el usuario del PlayerInput.");

            // Cambiamos al esquema de control "Controller" (asegúrate de que esté configurado en tu InputActions)
            var controlScheme = _playerInput.actions.FindControlScheme("Controller");
            if (controlScheme != null)
            {
                _playerInput.SwitchCurrentControlScheme("Controller", new[] { gamepad });
                Debug.Log($"SetGamepad: Control scheme cambiado a 'Controller' para Gamepad {gamepad.deviceId}");
            }
            else
            {
                Debug.LogWarning("SetGamepad: El esquema de control 'Controller' no fue encontrado.");
            }
        }
        else
        {
            Debug.LogError("SetGamepad: PlayerInput no encontrado en el objeto de PlayerController.");
        }
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
    [SerializeField] private ParticleSystem vfxDash;

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
    [SerializeField] private ParticleSystem vfxMuerte;
    internal int salud;
    internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        playerHUD.BarraDeVida = (float)salud / maxSalud;
        //GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);
        muerto = false;

        renderer = GetComponentInChildren<SkinnedMeshRenderer>();

        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    void DeadEvent()
    {
        Debug.Log("Murio: " + this.gameObject.name);
        muerto = true;
        DeshabilitarMovimiento();
        animator.SetTrigger("muerto");
        AudioManager.instance.PlaySound("muertejugador");
        GameManager.Instance.DeadPlayerEventMHS(this);
        vfxMuerte.Play();
        DesactivarSprite();

        if(escudo.gameObject.activeSelf)
        {
            escudo.gameObject.SetActive(false);
        }
    }

    public int Vida
    {
        get => salud;
        set
        {
            if (value < salud)
            {
                StartCoroutine(DañoEmisivo());
                //DamageVibration(frecuenciaMinimaDa�o, frecuenciaMaximaDa�o);
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
        //if (Vida! <= 0)
        //{
        //    animator.SetTrigger("daño");

        //}
        yield return new WaitForSeconds(0.1f);
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    public void Revivir()
    {
        //Verificar se la vida vuelve a 100(visualmente), sino agregar: playerHUD.BarraDeVida = (float)salud / maxSalud;
        Vida = maxSalud;
        animator.SetTrigger("spawn");
        muerto = false;
        isInvulnerable = false;
        habilidadProgreso = 0f;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
        StartCoroutine(CargarHabilidad());

        canEscudo = true;
        hability = null;
        actualHability = hability;
        habilidadDisponible = false;
        Start_Dash();
        Start_Escudo();

        GameObject clone = Instantiate(vfxRespanPlayer, transform.position, transform.rotation);
        Destroy(clone, 1.5f);
    }
    #endregion Vida

    #region Escudo

    [Header("Shield Stats")]
    [SerializeField] private float tiempoEscudo = 0.45f;
    [SerializeField] private float cooldownEscudo = 7;
    [SerializeField] public Transform escudo;
    [SerializeField] private int contadorEscudo = 7;
    private Vector3 diferenciaEscudo;
    private Quaternion rotacionEscudo;
    private bool canEscudo = true;

    void Start_Escudo()
    {
        //GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
        playerHUD.shieldIcon.enabled = true;
        contadorEscudo = 7;
        playerHUD.shieldCounter.text = contadorEscudo.ToString();
    }

    void ActivarEscudo()
    {
        canEscudo = false;
        animator.SetTrigger("escudo");
        escudo.gameObject.SetActive(true);
        playerHUD.shieldIcon.enabled = false;
        diferenciaEscudo = transform.forward * 2;
        escudo.position = transform.position + diferenciaEscudo;
        rotacionEscudo = transform.rotation;

        AudioManager.instance.PlaySound("escudo");

        Invoke("DesactivarEscudo", tiempoEscudo);
    }

    void DesactivarEscudo()
    {
        //canShoot = true;
        animator.SetTrigger("mov");
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

        //yield return new WaitForSeconds(cooldownEscudo);
        //GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
        canEscudo = true;
        contadorEscudo = 7;
        playerHUD.shieldCounter.text = contadorEscudo.ToString();
        playerHUD.shieldIcon.enabled = true;
    }

    #endregion Escudo

    #region Habilidad

    [Header("Ability Stats")]
    [SerializeField] private string personaje;
    [SerializeField] private float cargaHabilidad;
    public float habilidadProgreso;
    public bool habilidadDisponible;
    public HabilidadEnArea habilidadEnArea;
    public HabilidadRayo habilidadRayo;
    public HabilidadEscopeta habilidadEscopeta;
    public HabilidadSub habilidadSub;

    public void Start_Habilidad()
    {
        habilidadProgreso = 0;
        playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
        StartCoroutine(CargarHabilidad());

        habilidadDisponible = false;
    }

    void FixedUpdate_Habilidad()
    {
        //if (habilidadProgreso == 1f)
        //{
        //    habilidadDisponible = true;
        //    //_gamepad.SetMotorSpeeds(frecuenciaMinimaHabilidad, frecuenciaMaximaHabilidad);
        //    //Invoke("StopVibration", tiempoDeVibracionHabilidad);
        //    //playerHUD.explosiveShotIcon.enabled = true;
        //}
        //else
        //{
        //    habilidadDisponible = false;
        //}
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

            if (habilidadProgreso >= 1f)
            {
                habilidadProgreso = 1f;
                playerHUD.BarraDeHabilidad = (float)habilidadProgreso;
                habilidadDisponible = true;
            }

            yield return null;
        }


    }

    void ActivarHabilidad()
    {
        switch (personaje)
        {
            case "SKYIE":
                HabilidadSKYIE();
                habilidadRayo = null;
                habilidadEscopeta = null;
                habilidadSub = null;
                break;
            case "NOVA":
                HabilidadNOVA();
                habilidadEnArea = null;
                habilidadEscopeta = null;
                habilidadRayo = null;
                break;
            case "KAI":
                HabilidadKAI();
                habilidadSub = null;
                habilidadRayo = null;
                habilidadEnArea = null;
                break;
            case "CRIM":
                HabilidadCRIM();
                habilidadEscopeta = null;
                habilidadEnArea = null;
                habilidadSub = null;
                break;
        }
    }

    void HabilidadSKYIE()
    {
        Debug.Log(this.gameObject.name + "Activo la habilidad");
        habilidadEnArea.ActivarHabilidad();
        DeshabilitarMovimiento();

        AudioManager.instance.PlaySound("habilidadSKYIE");

        Invoke("HabilitarMovimiento", 0.5f);
    }

    void HabilidadNOVA()
    {
        Debug.Log(this.gameObject.name + "Activo la habilidad");
        habilidadSub.ActivarHabilidad();
        DeshabilitarMovimiento();

        AudioManager.instance.PlaySound("habilidadNOVA");
    }

    void HabilidadCRIM()
    {
        Debug.Log(this.gameObject.name + "Activo la habilidad");
        habilidadRayo.ActivarHabilidad();
        DeshabilitarMovimiento();

        AudioManager.instance.PlaySound("habilidadCRIM");
    }

    void HabilidadKAI()
    {
        Debug.Log(this.gameObject.name + "Activo la habilidad");
        habilidadEscopeta.ActivarHabilidad();
        DeshabilitarMovimiento();

        AudioManager.instance.PlaySound("habilidadKAI");

        Invoke("HabilitarMovimiento", 1f);
    }

    public void HabilitarMovimiento()
    {
        BloquearMovimiento = false;
        BloquearRotacion = false;
    }

    public void DeshabilitarMovimiento()
    {
        BloquearMovimiento = true;
        BloquearRotacion = true;
    }
    #endregion Habilidad

    #region POWER UP
    [Header("Power Up Core")]
    [SerializeField] private string actualHability = null;
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

    public void ResetearVariablesCambioRonda()
    {
        hability = null;
        actualHability = hability;
        playerSpeed = actualSpeed;
        isInvulnerable = false;
        inSuperSpeed = false;
        DesactivarSprite();
    }

    #region INVULNERABILIDAD
    [Header("Invulnerabilidad")]
    [SerializeField] private float isInvunerableTime = 5f;
    [SerializeField] internal bool isInvulnerable = false;
    [SerializeField] private GameObject vfxInmune;
    [SerializeField] private GameObject vfxVelocidad;
    //[SerializeField] private bool invulnerable;

    private void ActivarInvulnerabilidad()
    {
        isInvulnerable = true;
        vfxInmune.SetActive(true);
        AudioManager.instance.PlaySound("powerUpActive");
        Debug.Log("Se activo la invulnerabilidad");
        Invoke("DesactivarInvulnerabilidad", isInvunerableTime);
    }

    private void DesactivarInvulnerabilidad()
    {
        Debug.Log("Si se desactivo Invulnerabilidad");
        vfxInmune.SetActive(false);
        hability = null;
        actualHability = hability;
        isInvulnerable = false;
        DesactivarSprite();
    }
    #endregion INVULNERABILIDAD

    #region SUPER SPEED
    [Header("Super Speed")]
    [SerializeField] private float superSpeed = 10f;
    [SerializeField] private float actualSpeed;
    [SerializeField] private float superSpeedTime = 5f;
    public bool inSuperSpeed = false;

    void InicializarSSD()
    {
        actualSpeed = playerSpeed;
    }

    private void SuperSpeed()
    {
        playerSpeed = superSpeed;
        vfxVelocidad.SetActive(true);
        AudioManager.instance.PlaySound("powerUpActive");
        //superSpeed = playerSpeed;
        Invoke("DesactivarSSD", superSpeedTime);
    }

    private void DesactivarSSD()
    {
        vfxVelocidad.SetActive(false);
        hability = null;
        actualHability = hability;
        playerSpeed = actualSpeed;
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

    //void OnEnable()
    //{
    //    // Reiniciamos el progreso de la habilidad
    //    habilidadProgreso = 0f;
    //    playerHUD.BarraDeHabilidad = (float)habilidadProgreso;

    //    canEscudo = true;
    //    hability = null;
    //    actualHability = hability;
    //    muerto = false;
    //    habilidadDisponible = false;

    //    //canShoot = true;
    //    // Iniciamos la coroutine para cargar la habilidad
    //    StartCoroutine(CargarHabilidad());

    //    Start_Dash();
    //    Start_Escudo();

    //    BloquearMovimiento = false;
    //    BloquearRotacion = false;

    //    GameObject clone = Instantiate(vfxRespanPlayer, transform.position, transform.rotation);
    //    Destroy(clone, 1.5f);

    //}

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

    private Jugador _jugador;

    public Jugador Jugador
    {
        get => _jugador;
        set
        {
            _jugador = value;
            equipo = _jugador.equipo;
            playerHUD.name = _jugador.personaje;
            //gameObject.name = equipo + " - " + _jugador.personaje;

            // Llama a AsignarGamepad y fuerza el emparejamiento aislado
            AsignarGamepad(_jugador.gamepadId);
        }
    }
}
