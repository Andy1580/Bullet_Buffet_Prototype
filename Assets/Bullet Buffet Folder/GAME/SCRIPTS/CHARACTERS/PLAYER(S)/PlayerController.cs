using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private Transform playerTransform;

    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        playerTransform = this.transform;

        Start_Movimiento();
        ch = GetComponent<CharacterController>();
    }

    private void Update()
    {
        LeerAxis();
        Update_Movimiento();

        cont -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
    }


    #region Movement
    //[Header("Movement stats")]
    //[SerializeField] private float playerSpeed = 2.0f;
    //[SerializeField] private float gravityValue = -9.81f;


    //private CharacterController controller;
    //private Vector2 playerVelocity;
    //private bool groundedPlayer;

    //private Vector2 movementInput = Vector2.zero;

    //public float velocidadRotacion = 5f; // Velocidad de rotación del jugador

    //Quaternion q;

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    movementInput = context.ReadValue<Vector2>();
    //}

    //void Update()
    //{
    //    //BulletShoot();

    //    //CallbackContext contexto = playerInput.actions["Shoot"].ReadValue<CallbackContext>();



    //    cont -= Time.deltaTime;

    //    groundedPlayer = controller.isGrounded;
    //    if (groundedPlayer && playerVelocity.y < 0)
    //    {
    //        playerVelocity.y = 0f;
    //    }

    //    Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
    //    controller.Move(move * Time.deltaTime * playerSpeed);

    //    if (move != Vector3.zero)
    //    {
    //        gameObject.transform.forward = move;
    //    }

    //    // Changes the height position of the player..
    //    //if (Input.GetButtonDown("Jump") && groundedPlayer)
    //    //{
    //    //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    //    //}

    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    controller.Move(playerVelocity * Time.deltaTime);

    //    // Obtener la posición del mouse en pantalla
    //    //Vector3 posicionMouse = Input.mousePosition;

    //        //Mouse.current.position.ReadValue();

    //    //posicionMouse = Camera.main.ScreenToWorldPoint(posicionMouse);


    //    //Vector2 direccion = new Vector2(posicionMouse.x - transform.rotation.x, posicionMouse.y - transform.rotation.y);

    //    //transform.right = direccion;

    //    ///////////////////////////////////

    //    Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

    //    float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

    //    Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    //    playerTransform.rotation = rotation;

    //}
    #endregion

    #region Movimiento Op 2
    [SerializeField] private float velocidadNormal = 5f;
    [SerializeField] private float velocidadCorrer = 10f;
    [SerializeField] private float potenciaSalto = 8f;

    private Vector3 axis;
    private float velocidadActual;
    private bool corriendo;
    private CharacterController ch;

    private Vector3 movementInput = Vector3.zero;

    void Start_Movimiento()
    {
        velocidadActual = velocidadNormal;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void LeerAxis()
    {

        //Nos va a regresar un valor entre -1 y 1 (Izq y Der)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector3 es una estructura y no un objeto
        axis = new Vector3(movementInput.x, 0, movementInput.z);

    }

    private void Update_Movimiento()
    {

        Rotar();
        

        if(axis.magnitude > velocidadActual)
        {
            axis = axis.normalized;
        }
        
        axis *= velocidadActual;

        ch.Move(axis * Time.deltaTime);


    }

    private void Rotar()
    {

        //Creamos un rayo con origen en la camara y direccion hacia el cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Obtenemos la LayerMask de la Layer Default
        LayerMask capa = LayerMask.GetMask("Default");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, capa))
        {
            //Guardamos donde colisiona el rayo
            Vector3 punto = hit.point;

            //Le ponemos la misma altura que el personaje
            punto.y = transform.position.y;

            //Rotamos hacia ese punto
            transform.LookAt(punto);
        }

    }
    #endregion

    #region Skills

    #endregion

    #region Handgun

    [Header("Handgun stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float cooldown;

    [SerializeField] private Transform bulletprefab;
    [SerializeField] private Transform bulletSpawn;

    private float cont = 0;

    private ControlInput input;

    //public InputAction shoot;

    private InputControl shoot2;

    public delegate void Shoot(in Shoot input);

    public void OnShoot(InputAction.CallbackContext context)
    {
        //shoot2 = playerInput.actions["Fire"].triggered.
        //input = context.
        //isShooting = context.ReadValue<bool>();
        //isShooting = context.action.triggered;
        //BulletShoot() = context.ReadValue;
        //Debug.Log("Pressed");
        BulletShoot();
    }

    void BulletShoot()
    {
        if (cont <= 0)
        {
            Transform clon = Instantiate(bulletprefab, bulletSpawn.position, bulletSpawn.rotation);
            clon.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            Destroy(clon.gameObject, 3);
            cont = cooldown;
        }

        //if (playerInput.actions["Shoot"].triggered && cont <= 0)
        //{
        //}
    }

    #endregion
}
