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
        Start_Escudo();
        Start_Pausa();
    }

    private void Update()
    {
        Update_Movimiento();
        Update_Shoot();
        Update_Vida();
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
            canEscudo = false;
            Vector3 posicion = transform.position + transform.forward;
            escudo = Instantiate(escudoPrefab, posicion, transform.rotation);
            diferenciaEscudo = escudo.position - transform.position;

            Invoke("DesactivarEscudo", tiempoEscudo);
        }
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        BulletShoot();
    }

    public void Input_Pausa(InputAction.CallbackContext context)
    {
        if(tiempoDeJuego == 1f)
        {
            tiempoDeJuego = 0f;
        }
        else
        {
            tiempoDeJuego = 1f;
        }
    }

    #endregion INPUT

    #region Movimiento & Rotacion

    [Header("Movement Stats")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float smoothRotacion;
    private CharacterController controller;
    private Vector3 movement = Vector3.zero;
    private Vector3 axis1 = Vector3.zero;
    private Vector3 axis2 = Vector3.zero;

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update_Movimiento()
    {
        Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
        movement.x = moveXZ.x;
        movement.z = moveXZ.z;

        Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
        transform.LookAt(rotation);

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
    public int salud;
    [SerializeField] private Slider BarraSalud;
    public bool isInvulnerable = false;

    void Update_Vida()
    {
        BarraSalud.GetComponent<Slider>().value = salud;

        if (salud <= 0)
        {
            Debug.Log("Muerto" + this.gameObject.name);
        }

    }
    #endregion Vida

    #region Escudo

    [Header("Shield Stats")]
    [SerializeField] private bool canEscudo;
    [SerializeField] private float tiempoEscudo;
    [SerializeField] private float cooldownEscudo;
    private Transform escudo;
    private Vector3 diferenciaEscudo;

    [Header("Shield Objects")]
    [SerializeField] private Transform escudoPrefab;

    void Start_Escudo()
    {
        canEscudo = true;
    }
    
    void Update_Escudo()
    {
        escudo.position = transform.position + diferenciaEscudo;
    }

    void DesactivarEscudo()
    {
        Destroy(escudoPrefab, 3);
        StartCoroutine(CooldownEscudo());
    }

    IEnumerator CooldownEscudo()
    {
        yield return new WaitForSeconds(cooldownEscudo);
        canEscudo = true;
    }
    #endregion Escudo

    #region PAUSA
    private float tiempoDeJuego;

    private void Start_Pausa()
    {
        tiempoDeJuego = Time.timeScale;
    }
    #endregion PAUSA
}
