using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private Animator c_Animator;
    [SerializeField] private LobbyManager loby;
    public int equipoJugador = 0;
    public string selectedCharacter;
    public int gamepadID;
    public int originalID;

    [SerializeField] public RectTransform puntero;
    [SerializeField] public Image spritePersonaje;
    [SerializeField] private Image controlImg;
    private bool equipoBloqueado = false;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        /*
        foreach (RectTransform slot in slotsEquipo1)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in slotsEquipo2)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in posisionesSlotsEquipo1)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in posisionesSlotsEquipo2)
        {
            slot.gameObject.SetActive(false);
        }
        */
    }

    private void Start()
    {
        selectTm = true;
        selectCh = false;

        //puntero.gameObject.SetActive(false);
        //puntero.gameObject.transform.position = slot.position;

        c_Animator.SetInteger("Posicion", 0);

        controlImg.color = new Color(1, 1, 1, 0.6f);
        equipoBloqueado = false;
        equipoJugador = 0;
        //spritePersonaje.sprite = spritePersonaje.sprite;

    }

    private void Update()
    {
        Update_Puntero();
    }

    private void FixedUpdate()
    {
        //spritePersonaje.sprite = CheckSprite(selectedCharacter);
    }
    /*
    public void AsignarEquipo(int gamepadId, int equipo)
    {
        equipoJugador = equipo;

        int equipo1Index = 0;
        int equipo2Index = 0;

        int gamepadCount = Gamepad.all.Count;

        if (gamepadCount == 2)
        {
            slotsEquipo1[0].gameObject.SetActive(true);
            slotsEquipo2[0].gameObject.SetActive(true);
            AcomodarSlots();
        }
        else
        {
            foreach (RectTransform slot in slotsEquipo1)
            {
                slot.gameObject.SetActive(true);
            }

            foreach (RectTransform slot in slotsEquipo2)
            {
                slot.gameObject.SetActive(true);
            }
            AcomodarSlots();
        }

        // Mover el puntero a la posici�n del slot correspondiente
        if (equipoJugador == 1 && equipo1Index < slotsEquipo1.Count)
        {
            //RectTransform posicionSlot = slotsEquipo1[equipo1Index].GetChild(2).GetComponent<RectTransform>();
            MoverPuntero(slotsEquipo1[equipo1Index], textosEquipo1[equipo1Index], jugadorNickName);
            equipo1Index++;
            spritePersonaje = slotsEquipo1[equipo1Index].GetChild(1).GetComponent<Image>();
            
        }
        else if (equipoJugador == 2 && equipo2Index < slotsEquipo2.Count)
        {
            MoverPuntero(slotsEquipo2[equipo2Index], textosEquipo2[equipo2Index], jugadorNickName);
            equipo2Index++;
            spritePersonaje = slotsEquipo2[equipo2Index].GetChild(1).GetComponent<Image>();
            
        }
    }

    void AcomodarSlots()
    {
        int gamepadsCount = Gamepad.all.Count;

        if (gamepadsCount == 2)
        {
            posisionesSlotsEquipo1[1].gameObject.SetActive(true);
            posisionesSlotsEquipo2[0].gameObject.SetActive(true);

            slotsEquipo1[0].position = posisionesSlotsEquipo1[1].position;
            slotsEquipo2[0].position = posisionesSlotsEquipo2[0].position;
        }
        else
        {
            posisionesSlotsEquipo1[0].gameObject.SetActive(true);
            posisionesSlotsEquipo1[1].gameObject.SetActive(true);
            posisionesSlotsEquipo2[0].gameObject.SetActive(true);
            posisionesSlotsEquipo2[1].gameObject.SetActive(true);

            slotsEquipo1[0].position = posisionesSlotsEquipo1[0].position;
            slotsEquipo1[1].position = posisionesSlotsEquipo1[1].position;

            slotsEquipo2[0].position = posisionesSlotsEquipo2[0].position;
            slotsEquipo2[1].position = posisionesSlotsEquipo2[1].position;
        }
    }

    private void MoverPuntero(RectTransform slot, TMP_Text textoSlot, string nombreJugador)
    {
        puntero.gameObject.SetActive(true);
        puntero.gameObject.transform.position = slot.position;

        textoSlot.text = nombreJugador;
        Debug.Log($"Puntero {puntero.name} se movio a la posici�n del slot {slot.name}");
    }
    */
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

            }
            else if (v2.x > 0.5f) //Der
            {
                c_Animator.SetInteger("Posicion", 1);
                equipoJugador = 2; //Equipo Azul

            }
        }
    }

    public void Input_PunteroMovimiento(InputAction.CallbackContext context)
    {
        if (!selectCh) return;
        Vector2 v2 = context.ReadValue<Vector2>();
        axis = new Vector3(v2.x, v2.y, 0);
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
                    controlImg.color = Color.white;

                    // Asignar al equipo y actualizar contadores
                    LobbyManager.SeleccionarEquipo(currentGamepad, equipoJugador);
                    Debug.Log($"El jugador {this.name} seleccionó el equipo {equipoJugador}");

                    AudioManager.instance.PlaySound("botonmenu");
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

                if (col.CompareTag("BotonJugar"))
                {
                    var boton = col.GetComponent<Button>();

                    if (boton != null)
                    {
                        boton.onClick.Invoke();
                    }
                }
                else
                {
                    selectedCharacter = col.gameObject.name;
                    spritePersonaje.sprite = CheckSprite(selectedCharacter);
                    Gamepad currentGamepad = context.control.device as Gamepad;
                    loby.SeleccionarPersonaje(currentGamepad, selectedCharacter);
                    AudioManager.instance.PlaySound("botonmenu");
                    Debug.Log($"El {this.gameObject.name} a escogido al personaje {selectedCharacter}");
                }
            }

        }
    }

    public void Input_RechazarEquipo(InputAction.CallbackContext context)
    {
        if (context.performed && !equipoRechazado)
        {
            equipoRechazado = true; // Bloquea más llamadas
            Debug.Log("Se rechazó equipo");
            LobbyManager.RechazarEquipo();
            StartCoroutine(ResetEquipoRechazado());

            AudioManager.instance.PlaySound("botonBack");
        }
    }

    private IEnumerator ResetEquipoRechazado()
    {
        yield return new WaitForSeconds(0.1f);
        equipoRechazado = false;
    }

    public void ResetearVariables()
    {
        equipoJugador = 0;
        equipoBloqueado = false;
        selectTm = true;
        selectCh = false;
        puntero.gameObject.SetActive(false);
        selectedCharacter = "";

        if (spritePersonaje != null)
        {
            spritePersonaje.sprite = null;
        }
        else
        {
            Debug.LogWarning($"El 'spritePersonaje' en {gameObject.name} no está asignado todavía.");
        }

        c_Animator.SetInteger("Posicion", 0);
        controlImg.color = new Color(1, 1, 1, 0.6f);
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


