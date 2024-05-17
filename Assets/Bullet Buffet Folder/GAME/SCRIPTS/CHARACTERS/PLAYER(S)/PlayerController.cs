using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Vector3 movementInput = Vector2.zero;
    private Vector3 playerVelocity;
    private CharacterController controller;
    private Vector3 axis;

    [Header("Shoot Stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float cooldown;
    private float cont = 0;

    [Header("Shoot Objects")]
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    //public delegate void Shoot(in Shoot input);

    [Header("Life Stats")]
    public int salud;
    [SerializeField] private Slider BarraSalud;
    public bool isInvulnerable = false;


    private void Start()
    {
        Start_Movimiento();
    }

    private void Update()
    {
        Update_Movimiento();
        Update_Shoot();
        Update_Vida();
    }

    #region Movimiento

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
        axis = new Vector3(movementInput.x, 0, movementInput.y);
    }

    void Update_Movimiento()
    {
        Movimiento();
        Gravedad();
    }

    void Movimiento()
    {
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.position = move;
        }
    }

    void Gravedad()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    #endregion

    #region Dash

    [Header("Dash Stats")]
    [SerializeField] private bool enDash;
    [SerializeField] private float fuerzaDash;

    public void OnDash(InputAction.CallbackContext context)
    {
        Dash();
    }

    void Dash()
    {

        if (!enDash)
        {
            enDash = true;

            Invoke("DesactivarDash", 0.25f);
        }

        if (!enDash)
        {
            Vector3 m = movementInput * playerSpeed;
            axis.x = m.x;
            axis.z = m.y;
        }
        else
        {
            Debug.Log("Dash");

            Vector3 m = movementInput != Vector3.zero ? movementInput * fuerzaDash : transform.forward * fuerzaDash;
            axis.x = m.x;
            axis.z = m.y;
            controller.Move(axis * fuerzaDash * Time.deltaTime);

        }
        
       

    }

    void DesactivarDash()
    {
        enDash = false;
    }

    #endregion

    #region Shoot

    public void OnShoot(InputAction.CallbackContext context)
    {
        BulletShoot();
    }

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

    #endregion

    #region Vida

    void Update_Vida()
    {
        BarraSalud.GetComponent<Slider>().value = salud;

        if (salud <= 0)
        {
            Debug.Log("Muerto" + this.gameObject.name);
        }

    }
    #endregion

}
