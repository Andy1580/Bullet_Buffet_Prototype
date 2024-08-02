using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        //// Obtener información del GameManager
        //GameManager.Instance.SetupPlayer(this);

        //// Configurar HUD en el GameManager
        //hudSlot = GameManager.Instance.AssignHUDSlot(this);
        //playerHUD = hudSlot.GetComponent<PlayerHUD>();

        // Configurar equipo
        //SetupEquipo();
    }

    private void Update()
    {
        Update_Movimiento();
        Update_Shoot();
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
            enDash = true;
            canDash = false;
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
            }
            else if (habilidadProgreso >= 0.47f)
            {
                ExplosiveBullet();
                habilidadProgreso = 0;
            }

            playerHUD.UpdateHability(habilidadProgreso);
        }
    }

    #endregion INPUT

    #region UI References
    [SerializeField] private Image equipoRojo;
    [SerializeField] private Image equipoAzul;
    [SerializeField] private TMP_Text jugadorText;
    public GameObject hudSlot;
    private PlayerHUD playerHUD;
    #endregion UI References

    #region EQUIPOS
    [SerializeField] internal int equipo;
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
        Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
        transform.LookAt(rotation);

        if (BloquearMovimiento)
            return;

        Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
        movement.x = moveXZ.x;
        movement.z = moveXZ.z;

        //animator.SetFloat("x", movement.x);
        //animator.SetFloat("z", movement.z);


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
    [SerializeField] private TMP_Text contadorDashText;

    void Start_Dash()
    {
        canDash = true;
        //GameManager.Instance.UpdateDashStatus(this, true, contadorDash);
    }

    IEnumerator DesactivarDash()
    {
        Debug.Log("Dashiando");
        while (contadorDash > 0)
        {
            yield return new WaitForSeconds(1);
            //contadorDashText.text = contadorDash.ToString();          
            enDash = false;
            contadorDash--;
        }
        //contadorDashText.text = contadorDash.ToString();
        //GameManager.Instance.UpdateDashStatus(this, true, contadorDash);
        canDash = true;
        contadorDash = 5;
    }

    #endregion Dash

    #region Vida

    [Header("Life Stats")]
    [SerializeField] private int maxSalud = 300;
    [SerializeField] private SkinnedMeshRenderer renderer;
    internal int salud;
    //internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);

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

            GameManager.Instance.UpdatePlayerHealth(this, salud, maxSalud);
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
    [SerializeField] private TMP_Text contadorEscudoText;
    private Vector3 diferenciaEscudo;
    private Quaternion rotacionEscudo;
    private bool canEscudo = true;

    void Start_Escudo()
    {
        GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
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
        StartCoroutine(CooldawnEscudo());
    }

    IEnumerator CooldawnEscudo()
    {
        while (contadorEscudo >= 0)
        {
            contadorEscudo--;
            contadorEscudoText.text = contadorEscudo.ToString();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(cooldownEscudo);
        GameManager.Instance.UpdateShieldStatus(this, true, contadorEscudo);
        canEscudo = true;
    }

    #endregion Escudo

    #region Habilidad

    [Header("Ability Stats")]
    private float habilidadProgreso;

    void Start_Habilidad()
    {
        habilidadProgreso = 0;
        StartCoroutine(CargarHabilidad());
    }

    private IEnumerator CargarHabilidad()
    {
        while (true)
        {
            habilidadProgreso += Time.deltaTime / 10f; // Ajusta el tiempo de carga según sea necesario
            GameManager.Instance.UpdatePlayerAbility(this, habilidadProgreso);

            if (habilidadProgreso >= 1)
            {
                habilidadProgreso = 1;
                // Habilidad está completamente cargada
            }
            yield return null;
        }
    }


    #endregion Habilidad

    #region POWER UP
    [Header("Power Up Core")]
    [SerializeField] private string actualHability = "None";

    [SerializeField] private GameObject[] spritesPowerUp;
    internal string hability;

    public Dictionary<string, GameObject> spritesPowerUpDictionary = new Dictionary<string, GameObject>();

    void InicializarPowerUps()
    {

        actualHability = hability;

        foreach (GameObject go in spritesPowerUp)
        {
            spritesPowerUpDictionary.Add(go.name, go);
        }

        foreach (GameObject go in spritesPowerUp)
        {
            go.SetActive(false);
        }
    }

    public void SetHability(string newHability)
    {
        hability = newHability;
        actualHability = hability;

        GameManager.Instance.UpdatePlayerPowerUp(this, actualHability);
    }

    void DesactivarSprite()
    {
        GameManager.Instance.UpdatePlayerPowerUp(this, null);
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
