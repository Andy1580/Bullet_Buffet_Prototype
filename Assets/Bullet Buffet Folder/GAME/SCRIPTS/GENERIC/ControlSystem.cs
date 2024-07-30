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

    public bool selectTm;
    public bool selectCh;

    private void Start()
    {
        selectTm = true;
        selectCh = false;

        puntero.gameObject.SetActive(false);
    }

    private void Update()
    {
        Update_Puntero();
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
            if (selectTm && equipo != 0)
            {
                selectTm = false;
                selectCh = true;
                Gamepad currentGamepad = context.control.device as Gamepad;
                loby.SeleccionarEquipo(currentGamepad, equipo);
                puntero.gameObject.SetActive(true);
                c_Animator.gameObject.SetActive(false);
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
            Gamepad currentGamepad = context.control.device as Gamepad;
            loby.Input_Rechazar(currentGamepad);
            selectTm = true;
            selectCh = false;
            puntero.gameObject.SetActive(false);
            c_Animator.gameObject.SetActive(true);
            loby.ActivarPanelSeleccionarEquipo();
        }
    }

    private bool TodosEquiposSeleccionados()
    {
        return loby.equipo.Count >= 2 && loby.equipo.Count == loby.dicControles.Count;
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
        if (puntero.localPosition.x > -928f)//Falto terminar...
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


