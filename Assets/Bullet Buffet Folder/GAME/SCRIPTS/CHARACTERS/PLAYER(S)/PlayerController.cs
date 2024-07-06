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

    #endregion INPUT

    #region Movimiento & Rotacion

    [Header("Movement Stats")]
    [SerializeField] private float playerSpeed = 2.0f;
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
        if (BloquearMovimiento)
            return;

        Vector3 moveXZ = !enDash ? axis1 * playerSpeed : axis1 * fuerzaDash;
        movement.x = moveXZ.x;
        movement.z = moveXZ.z;


        Vector3 rotation = transform.position + axis2 * smoothRotacion * Time.deltaTime;
        transform.LookAt(rotation);

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
    [SerializeField] private Slider BarraSalud;
    private static int salud;
    internal bool isInvulnerable = false;
    //internal bool muerto = false;

    void Start_Vida()
    {
        salud = maxSalud;
        BarraSalud.GetComponent<Slider>().value = salud;
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
                DeadEvent();
            }
            else if(value >= maxSalud)
            {
                salud = maxSalud;
            }
            else
            {
                salud = value;
            }
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

}
