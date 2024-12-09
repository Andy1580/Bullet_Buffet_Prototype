using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] public Animator c_Animator;
    [SerializeField] private LobbyManager loby;
    public int equipoJugador = 0;
    public string selectedCharacter;
    public int gamepadID;
    public int originalID;

    [SerializeField] public RectTransform puntero;
    [SerializeField] public Image spritePersonaje;
    [SerializeField] public Sprite spritePersonajeDefault;
    [SerializeField] public Image controlImg;
    [SerializeField] public Sprite centro;
    [SerializeField] public Sprite izquierda;
    [SerializeField] public Sprite derecha;
    [SerializeField] public Sprite aceptarEquipo1;
    [SerializeField] public Sprite aceptarEquipo2;
    public bool equipoBloqueado = false;

    public bool selectTm;
    public bool selectCh;

    public List<RectTransform> slotsEquipo1;
    public List<RectTransform> slotsEquipo2;

    public RectTransform[] posisionesSlotsEquipo1;
    public RectTransform[] posisionesSlotsEquipo2;

    public string jugadorNickName;

    // Agregar una lista de TMP_Text para los textos de los slots
    [SerializeField] private List<TMP_Text> textosEquipo1; // Textos para el equipo 1
    [SerializeField] private List<TMP_Text> textosEquipo2; // Textos para el equipo 2

    private bool equipoRechazado = false;
    public bool escogiendoEquipoCS = true;
    private bool escogiendoPersonaje = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        c_Animator.SetInteger("Posicion", 0);
        controlImg.sprite = centro;
        Debug.Log("Ya se ejecuto el Awake de: " + gameObject.name);
    }

    private void Start()
    {
        selectTm = true;
        selectCh = false;

        //puntero.gameObject.SetActive(false);
        //puntero.gameObject.transform.position = slot.position;

        equipoBloqueado = false;
        equipoJugador = 0;
        //spritePersonaje.sprite = spritePersonaje.sprite;

        equipoRechazado = false;
        escogiendoEquipoCS = true;
        escogiendoPersonaje = false;
    }

    private void Update()
    {
        Update_Puntero();
    }

    private void FixedUpdate()
    {
        //spritePersonaje.sprite = CheckSprite(selectedCharacter);
    }

    #region INPUT
    public void Input_ControlMovimiento(InputAction.CallbackContext context)
    {
        Vector2 v2 = context.ReadValue<Vector2>();

        if (selectTm)
        {
            if (v2.x < -0.5f) //Izq
            {
                c_Animator.SetInteger("Posicion", -1);
                equipoJugador = 1; //Equipo Rojo;
                controlImg.sprite = izquierda;
                controlImg.color = Color.blue;
            }
            else if (v2.x > 0.5f) //Der
            {
                c_Animator.SetInteger("Posicion", 1);
                equipoJugador = 2; //Equipo Azul
                controlImg.sprite = derecha;
                controlImg.color= Color.yellow;
            }
        }
    }

    public void Input_PunteroMovimiento(InputAction.CallbackContext context)
    {
        if (!selectCh || LobbyManager.escogiendoEquipo) return;
        Vector2 v2 = context.ReadValue<Vector2>();
        axis = new Vector3(v2.x, v2.y, 0);
    }

    public void Input_Continuar(InputAction.CallbackContext context)
    {
        if (LobbyManager.self.continuar.activeSelf && LobbyManager.self.panelSelectTeam.activeSelf && escogiendoEquipoCS)
        {
            escogiendoEquipoCS = false;
            escogiendoPersonaje = true;
            LobbyManager.self.ActivarPanelPersonajesConDelay();
            //AudioManager.instance.PlaySound("botonJugar");
        }
        else if (LobbyManager.self.continuar.activeSelf && LobbyManager.self.panelSelectCh.activeSelf && !LobbyManager.partidaComenzada)
        {
            LobbyManager.self.RecopilarInformacion();
            //AudioManager.instance.PlaySound("botonJugar");
        }
    }

    public void Input_AceptarEquipo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (LobbyManager.escogiendoEquipo && !equipoBloqueado && selectTm && equipoJugador != 0)
            {
                int jugadoresPermitidos = (Gamepad.all.Count == 2) ? 1 : 2;

                if (LobbyManager.equipoControles[equipoJugador - 1] < jugadoresPermitidos)
                {
                    equipoBloqueado = true;
                    selectTm = false;
                    selectCh = true;

                    Gamepad currentGamepad = context.control.device as Gamepad;
                    puntero.gameObject.SetActive(true);
                    //controlImg.sprite = aceptar;

                    if(equipoJugador == 1)
                    {
                        controlImg.sprite = aceptarEquipo1;
                        controlImg.color = Color.white;   
                    }
                    else
                    {
                        controlImg.sprite = aceptarEquipo2;
                        controlImg.color = Color.white;
                    }

                    // Asignar al equipo y actualizar contadores
                    LobbyManager.SeleccionarEquipo(currentGamepad, equipoJugador);
                    Debug.Log($"El jugador {this.name} seleccionó el equipo {equipoJugador}");

                    //AudioManager.instance.PlaySound("botonmenu");
                }
                else
                {
                    Debug.Log("No hay espacio en el equipo " + equipoJugador);
                }
            }

            if (selectCh && selectedCharacter != null)
            {
                Vector2 origen = puntero.position;
                float radio = 5;
                LayerMask capa = LayerMask.GetMask("Character");

                Collider2D col = Physics2D.OverlapCircle(origen, radio, capa);

                if (!col) return;

                selectedCharacter = col.gameObject.name;
                spritePersonaje.sprite = CheckSprite(selectedCharacter);
                Gamepad currentGamepad = context.control.device as Gamepad;
                loby.SeleccionarPersonaje(currentGamepad, selectedCharacter);
                selectCh = false;
                axis = Vector2.zero;
                //AudioManager.instance.PlaySound("botonmenu");
                Debug.Log($"El {this.gameObject.name} a escogido al personaje {selectedCharacter}");
            }

        }
    }

    public void Input_RechazarEquipo(InputAction.CallbackContext context)
    {
        if (!context.performed || equipoRechazado) return;

        equipoRechazado = true; // Bloqueo para evitar múltiples llamadas simultáneas

        if (LobbyManager.self.panelSelectTeam.activeSelf || LobbyManager.self.panelSelectCh.activeSelf)
        {
            // Pasar el control al LobbyManager según el panel activo
            LobbyManager.RechazarEquipo(equipoJugador, this);
            Gamepad currentGamepad = context.control.device as Gamepad;
            LobbyManager.RechazarPersonaje(currentGamepad);
        }

        StartCoroutine(ResetEquipoRechazado());
    }

    private IEnumerator ResetEquipoRechazado()
    {
        yield return new WaitForSeconds(0.3f);
        equipoRechazado = false;
    }

    public void ResetearVariables()
    {
        //if (LobbyManager.self.panelSelectTeam.activeSelf)
        //{
        //    // Solo resetear el estado interno de este ControlSystem
        //    equipoJugador = 0;
        //    equipoBloqueado = false;
        //    selectTm = true;
        //    c_Animator.SetInteger("Posicion", 0);
        //    controlImg.color = new Color(1, 1, 1, 0.6f);
        //    Debug.Log($"ControlSystem {gameObject.name} reseteó sus variables en selección de equipo.");
        //}
        //else if (LobbyManager.self.panelSelectCh.activeSelf)
        //{
        //    // Solo resetear el estado de personaje si aplica
        //    if (selectedCharacter == null || selectedCharacter == "")
        //    {
        //        // Si no tiene personaje seleccionado, regresar al panel de selección de equipo
        //        LobbyManager.RecetearVariablesLobby();
        //        puntero.gameObject.SetActive(false);
        //        equipoJugador = 0;
        //        equipoBloqueado = false;
        //        selectTm = true;
        //        c_Animator.SetInteger("Posicion", 0);
        //        controlImg.color = new Color(1, 1, 1, 0.6f);
        //        Debug.Log($"ControlSystem {gameObject.name} reseteó al volver a selección de equipo.");
        //    }
        //    else
        //    {
        //        // Si ya tiene personaje seleccionado, resetear solo eso
        //        selectedCharacter = "";
        //        spritePersonaje.sprite = spritePersonajeDefault;
        //        selectCh = true;
        //        Debug.Log($"ControlSystem {gameObject.name} reseteó solo el personaje.");
        //    }
        //}

        selectedCharacter = "";
        spritePersonaje.sprite = spritePersonajeDefault;
        selectCh = true;


    }

    private Sprite CheckSprite(string personaje)
    {
        switch (personaje)
        {
            case "CRIM":
                return Resources.Load<Sprite>("CRIM");
            case "KAI":
                return Resources.Load<Sprite>("KAI");
            case "NOVA":
                return Resources.Load<Sprite>("NOVA");
            case "SKYIE":
                return Resources.Load<Sprite>("SKYIE");
            default:
                return null;
        }
    }

    #endregion INPUT

    #region PUNTERO

    [Header("Puntero Stats")]
    [SerializeField] private float velocidadPuntero;
    private Vector3 axis = Vector2.zero;

    public Vector2 limon;
    public Vector2 aguacate;

    public Vector2 canvasTamaño;
    void Update_Puntero()
    {
        // Calcular los límites del Canvas
        RectTransform canvasRect = LobbyManager.Canvas.GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Definir un margen adicional para limitar el área de movimiento
        float margenX = 50f; // Margen horizontal (en unidades del Canvas)
        float margenY = 50f; // Margen vertical

        // Calcular los límites del puntero considerando el margen
        float minX = -canvasSize.x / 2f + margenX;
        float maxX = canvasSize.x / 2f - margenX;
        float minY = -canvasSize.y / 2f + margenY;
        float maxY = canvasSize.y / 2f - margenY;

        // Mover el puntero
        Vector3 nuevaPosicion = puntero.localPosition + axis * (Time.deltaTime * velocidadPuntero);

        // Limitar la posición dentro del área visible del Canvas
        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, minX, maxX);
        nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, minY, maxY);

        // Asignar la posición clamped al puntero
        puntero.localPosition = nuevaPosicion;
    }

    #endregion PUNTERO

    

    private void OnEnable()
    {
        c_Animator.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        c_Animator.gameObject.SetActive(false);
    }
}


