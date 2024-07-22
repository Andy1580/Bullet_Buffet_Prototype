using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ControlSystem : MonoBehaviour
{
    [SerializeField] private Animator c_Animator;
    public int equipo = 0;
    private string selectedCharacter;

    Transform objetoPadre;
    Transform primerHijo;
    Transform segundoHijo;

    private Image control;
    private Image puntero;

    public bool selectCh = false;

    private void Start()
    {
        objetoPadre = c_Animator.transform;

        primerHijo = objetoPadre.GetChild(0);
        segundoHijo = objetoPadre.GetChild(1);

        control = primerHijo.GetComponent<Image>();
        puntero = segundoHijo.GetComponent<Image>();

    }

    private void Update()
    {
        Update_Puntero();
    }

    #region INPUT
    public void Input_ControlMovimiento(InputAction.CallbackContext context)
    {
        Vector2 v2 = context.ReadValue<Vector2>();

        if (v2.x < -0.5f) //Izq
        {
            c_Animator.SetInteger("Posicion", -1);
            equipo = 1; //Equipo Rojo;

            if(!selectCh)
            {
                selectCh = true;
            }
        }
        else if (v2.x > 0.5f) //Der
        {
            c_Animator.SetInteger("Posicion", 1);
            equipo = 2; //Equipo Azul

            if (!selectCh)
            {
                selectCh = true;
            }
        }

    }

    public void Input_PunteroMovimiento(InputAction.CallbackContext context)
    {
        axis = context.ReadValue<Vector2>();
    }

    public void Input_Aceptar(InputAction.CallbackContext context)
    {
        //Aqui agregar la logica para aceptar el equipo que escogieron para posteriormente pasar a escoger el personaje.

        //En el espacio if(selectCh), debe mandar la informacion al Lobby Manager para que prenda el panel de seleccion de personaje y apague el de seleccion de equipo,
        //posterior debe indicarle en este mismo script que el control(Image) debe apagarse para prender el puntero(Image).

        //Cuando se escoja un personaje, puede haber un metodo aparte que se encargue de mandar la informacion al Lobby Manager de cual jugador escogio este personaje, tambien puedes agregar la informacion de que equipo son para mandarle toda esa info junta

        if(selectCh)
        {

        }
    }
    #endregion INPUT

    #region PUNTERO

    [Header("Puntero Stats")]
    [SerializeField] private float velocidadPuntero = 2.0f;
    private Vector3 axis = Vector2.zero;

    void Update_Puntero()
    {
        //Si puedes agregale limite de pantalla para que el puntero no sobrepase ese limite.

        puntero.rectTransform.localPosition += axis * (Time.deltaTime * velocidadPuntero);

        RaycastCharacter();
    }

    void RaycastCharacter()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = puntero.rectTransform.anchoredPosition,
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Character"))
            {
                selectedCharacter = result.gameObject.name;
                Debug.Log("Selected Character: " + selectedCharacter);
                break;
            }
        }
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


