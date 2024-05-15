using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
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

    //#region Look

    //[SerializeField] private GameObject objetivo;
    //public bool lookObj;

    //void Update_Find_Objetivo()
    //{
    //    if (objetivo == null)
    //    {
    //        objetivo = GameObject.FindGameObjectWithTag("Player2");
    //    }
    //}

    //void FixUpdate_Look_Objetivo()
    //{
    //    if (objetivo != null)
    //    {
    //        lookObj = true;
    //        transform.LookAt(objetivo.transform.position);
    //    }
    //}

    //#endregion

    //#region Movement
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
    //#endregion

    //#region Movimiento Op 2
    //[SerializeField] private float velocidadNormal = 5f;
    //[SerializeField] private float velocidadCorrer = 10f;
    //[SerializeField] private float potenciaSalto = 8f;

    //private Vector3 axis;
    //private float velocidadActual;
    //private bool corriendo;
    //private CharacterController pC;

    //private float playerSpeed = 2.0f;
    //private float jumpHeight = 1.0f;
    //private float gravityValue = -9.81f;

    //private Vector3 movementInput = Vector3.zero;

    //private Vector3 playerVelocity;

    //void Start_Movimiento()
    //{
    //    velocidadActual = velocidadNormal;

    //}

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    //axis = context.ReadValue<Vector2>();
    //    movementInput = context.ReadValue<Vector2>();
    //}

    //private void LeerAxis()
    //{

    //    //Nos va a regresar un valor entre -1 y 1 (Izq y Der)
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");

    //    //Vector3 es una estructura y no un objeto
    //    axis = new Vector3(x, 0, z);

    //}

    //public float speed = 5f;
    //Vector3 movement = Vector3.zero;
    //private void Update_Movimiento()
    //{

    //    //Rotar();
    //    //if (move != Vector3.zero)
    //    //    {
    //    //        gameObject.transform.forward = move;
    //    //    }

    //    //if (axis != Vector3.zero && axis.magnitude > velocidadActual)
    //    //{
    //    //    axis = axis.normalized;
    //    //}

    //    //axis *= velocidadActual;

    //    //pC.Move(axis * Time.deltaTime);




    //    Vector3 move = new Vector3(movementInput.x, 0, movementInput.z);
    //    pC.Move(move * Time.deltaTime * playerSpeed);

    //    if (move != Vector3.zero)
    //    {
    //        gameObject.transform.forward = move;
    //    }


    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    pC.Move(playerVelocity * Time.deltaTime);


    //}

    //private void Rotar()
    //{
    //    ActualizarRotacion();
    //    ////Creamos un rayo con origen en la camara y direccion hacia el cursor
    //    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    ////Obtenemos la LayerMask de la Layer Default
    //    //LayerMask capa = LayerMask.GetMask("Default");

    //    //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, capa))
    //    //{
    //    //    //Guardamos donde colisiona el rayo
    //    //    Vector3 punto = hit.point;

    //    //    //Le ponemos la misma altura que el personaje
    //    //    punto.y = transform.position.y;

    //    //    //Rotamos hacia ese punto
    //    //    transform.LookAt(punto);
    //    //}


    //    ////////////OP 2///////////
    //    //// Obtenemos la posición del mouse en la pantalla
    //    //Vector3 posicionMouse = Input.mousePosition;

    //    //// Convertimos la posición del mouse en la pantalla a un rayo en el mundo 3D
    //    //Ray rayo = Camera.main.ScreenPointToRay(posicionMouse);

    //    //// Variable para almacenar el punto de intersección del rayo con un objeto en la escena
    //    //Vector3 puntoInterseccion;

    //    //// Si el rayo golpea un objeto en la escena
    //    //if (Physics.Raycast(rayo, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default")))
    //    //{
    //    //    // Guardamos el punto de intersección con el objeto
    //    //    puntoInterseccion = hit.point;
    //    //}
    //    //else
    //    //{
    //    //    // Si el rayo no golpea ningún objeto, simplemente elegimos un punto en la misma dirección que el rayo
    //    //    puntoInterseccion = rayo.GetPoint(10f); // Aquí puedes ajustar la distancia si es necesario
    //    //}

    //    //// Convertimos el punto de intersección a un vector relativo a la posición actual del personaje
    //    //Vector3 direccion = puntoInterseccion - transform.position;
    //    //direccion.y = 0f; // Mantenemos la misma altura que el personaje para rotar solo en el plano horizontal

    //    //// Calculamos la rotación necesaria para que el personaje mire hacia la dirección del mouse
    //    //Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

    //    //// Aplicamos la rotación al personaje
    //    //transform.rotation = rotacionObjetivo;

    //}

    //private Vector3 posicionMousePresionado;

    //// Función para manejar el movimiento y la rotación del personaje
    //void ActualizarRotacion()
    //{
    //    // Si se presiona el botón (o se hace click)
    //    if (Input.GetMouseButtonDown(0)) // Cambia el valor 0 por el botón que desees
    //    {
    //        // Almacenar la posición del mouse en ese momento
    //        posicionMousePresionado = Input.mousePosition;
    //    }

    //    // Convertir la posición del mouse en la pantalla a un rayo en el mundo 3D
    //    Ray rayo = Camera.main.ScreenPointToRay(posicionMousePresionado);

    //    // Variable para almacenar el punto de intersección del rayo con un objeto en la escena
    //    Vector3 puntoInterseccion;

    //    // Si el rayo golpea un objeto en la escena
    //    if (Physics.Raycast(rayo, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default")))
    //    {
    //        // Guardamos el punto de intersección con el objeto
    //        puntoInterseccion = hit.point;
    //    }
    //    else
    //    {
    //        // Si el rayo no golpea ningún objeto, simplemente elegimos un punto en la misma dirección que el rayo
    //        puntoInterseccion = rayo.GetPoint(10f); // Aquí puedes ajustar la distancia si es necesario
    //    }

    //    // Convertir el punto de intersección a un vector relativo a la posición actual del personaje
    //    Vector3 direccion = puntoInterseccion - transform.position;
    //    direccion.y = 0f; // Mantenemos la misma altura que el personaje para rotar solo en el plano horizontal

    //    // Calculamos la rotación necesaria para que el personaje mire hacia la dirección del mouse
    //    Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

    //    // Aplicamos la rotación al personaje
    //    transform.rotation = rotacionObjetivo;
    //}
    //#endregion

    #region Movimiento

    [Header("Movement Stats")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private Vector2 movementInput = Vector2.zero;

    private Vector3 playerVelocity;
    private CharacterController controller;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Start_Movimiento()
    {
        controller = gameObject.GetComponent<CharacterController>();
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
            gameObject.transform.forward = move;
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

    #region Shoot

    [Header("Shoot stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float cooldown;

    [Header("Objects")]
    [SerializeField] private Transform bulletprefab;
    [SerializeField] private Transform bulletSpawn;

    private float cont = 0;

    //public delegate void Shoot(in Shoot input);

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
            Transform clon = Instantiate(bulletprefab, bulletSpawn.position, bulletSpawn.rotation);
            clon.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            Destroy(clon.gameObject, 3);
            cont = cooldown;
        }

    }

    #endregion

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
    #endregion

    #region Dash

    #endregion
}
