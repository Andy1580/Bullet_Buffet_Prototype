using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private Animator c_Animator;
    [SerializeField] private LobbyManager loby;
    public int equipo = 0;
    public string selectedCharacter;

    [SerializeField] private RectTransform puntero;
    [SerializeField] private RectTransform slot;
    [SerializeField] private Image spritePersonaje;
    [SerializeField] private Image controlImg;
    private bool equipoBloqueado = false;

    public bool selectTm;
    public bool selectCh;

    private void Start()
    {
        selectTm = true;
        selectCh = false;

        puntero.gameObject.SetActive(false);
        puntero.gameObject.transform.position = slot.position;

        c_Animator.SetInteger("Posicion", 0);

        controlImg.color = new Color(1, 1, 1, 0.6f);
        equipoBloqueado = false;
        equipo = 0;


    }

    private void Update()
    {
        Update_Puntero();
    }

    private void FixedUpdate()
    {
        spritePersonaje.sprite = CheckSprite(selectedCharacter);
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
                equipo = 1; //Equipo Rojo;

            }
            else if (v2.x > 0.5f) //Der
            {
                c_Animator.SetInteger("Posicion", 1);
                equipo = 2; //Equipo Azul

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
            if (LobbyManager.escogiendoEquipo)
            {
                if (!equipoBloqueado)
                {
                    if (selectTm && equipo != 0)
                    {
                        AudioManager.instance.PlaySound("botonmenu");

                        if (Gamepad.all.Count == 2)
                        {
                            if (LobbyManager.equipoControles[equipo - 1] <= 0)
                            {
                                Debug.Log("1");
                                equipoBloqueado = true;
                                selectTm = false;
                                selectCh = true;
                                Gamepad currentGamepad = context.control.device as Gamepad;
                                puntero.gameObject.SetActive(true);
                                controlImg.color = Color.white;

                                if (equipo == 1 && LobbyManager.equipo1 < Gamepad.all.Count / 2)
                                {
                                    LobbyManager.SeleccionarEquipo(currentGamepad, equipo);
                                    LobbyManager.equipo1++;
                                    Debug.Log("Te unes al equipo 1");
                                }
                                else if ((equipo == 2) && LobbyManager.equipo2 < Gamepad.all.Count / 2)
                                {
                                    LobbyManager.SeleccionarEquipo(currentGamepad, equipo);
                                    LobbyManager.equipo2++;
                                    Debug.Log("Te unes al equipo 2");
                                }
                                else
                                {
                                    Debug.Log("No hay espacio en el equipo " + equipo);
                                }
                            }

                        }
                        else if (Gamepad.all.Count == 4)
                        {
                            if (LobbyManager.equipoControles[equipo - 1] <= 1)
                            {
                                Debug.Log("2");
                                equipoBloqueado = true;
                                selectTm = false;
                                selectCh = true;
                                Gamepad currentGamepad = context.control.device as Gamepad;
                                //LobbyManager.SeleccionarEquipo(currentGamepad, equipo);
                                puntero.gameObject.SetActive(true);
                                controlImg.color = Color.white;
                                if (equipo == 1 && LobbyManager.equipo1 < Gamepad.all.Count / 2)
                                {
                                    LobbyManager.SeleccionarEquipo(currentGamepad, equipo);
                                    LobbyManager.equipo1++;
                                    Debug.Log("Te unes al equipo 1");
                                }                                   // 2                // 4/2 = 2
                                else if ((equipo == 2) && LobbyManager.equipo2 < Gamepad.all.Count / 2)
                                {
                                    LobbyManager.SeleccionarEquipo(currentGamepad, equipo);
                                    LobbyManager.equipo2++;
                                    Debug.Log("Te unes al equipo 2");
                                }
                                else
                                {
                                    Debug.Log("No hay espacio en el equipo " + equipo);
                                }
                            }

                        }
                        else
                        {
                            Debug.Log("3");
                        }
                    }

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
                    AudioManager.instance.PlaySound("seleccionpersonaje");
                    selectedCharacter = col.gameObject.name;
                    CheckSprite(selectedCharacter);
                    Gamepad currentGamepad = context.control.device as Gamepad;
                    loby.SeleccionarPersonaje(currentGamepad, selectedCharacter);
                    Debug.Log("Escojio el personaje: " + selectedCharacter);
                }
            }


        }
    }

    public void Input_RechazarEquipo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Gamepad.all.Count == 2 || Gamepad.all.Count == 4)
            {
                if (LobbyManager.equipoControles[equipo - 1] > 0)
                {
                    Gamepad currentGamepad = context.control.device as Gamepad;
                    if (equipo == 1)
                    {
                        LobbyManager.RechazarEquipo(currentGamepad, equipo);
                        LobbyManager.equipo1--;
                        Debug.Log("Te sales del equipo 1 y ahora queda " + LobbyManager.equipo1 + " espacios en el equipo 1");
                    }
                    else if (equipo == 2)
                    {
                        LobbyManager.RechazarEquipo(currentGamepad, equipo);
                        LobbyManager.equipo2--;
                        Debug.Log("Te sales del equipo 2 y ahora queda " + LobbyManager.equipo2 + " espacios en el equipo 2");
                    }
                    else
                    {
                        // No estas en ningun equipo
                        Debug.Log("No estas en ningun equipo");
                    }
                    equipo = 0;
                    equipoBloqueado = false;
                    selectTm = true;
                    selectCh = false;
                    puntero.gameObject.SetActive(false);
                    puntero.gameObject.transform.position = slot.position;
                    selectedCharacter = null;
                    c_Animator.SetInteger("Posicion", 0);
                    controlImg.color = new Color(1, 1, 1, 0.6f);
                    //equipo = 0;
                    LobbyManager.ActivarPanelSeleccionarEquipo();
                }
            }

        }
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
        //if (puntero.localPosition.x > -928f)//Falto terminar...
        puntero.localPosition += axis * (Time.deltaTime * velocidadPuntero);
        limon = puntero.localPosition;
        aguacate = puntero.anchoredPosition;
        canvasTamaño = LobbyManager.Canvas.GetComponent<RectTransform>().sizeDelta;
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


