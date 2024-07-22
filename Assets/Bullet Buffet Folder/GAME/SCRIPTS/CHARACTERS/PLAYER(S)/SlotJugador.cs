using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SlotJugador : MonoBehaviour
{
    #region CORE SLOT
    [Header("Slot Core")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Image img;
    [SerializeField] private TMP_Text nombreTexto;
    private string selectedCharacter;
    public string personaje;
    private CharacterController controlador;
    private Gamepad control;

    private void Awake()
    {
        if (img != null)
        {
            img = Image;
            img.enabled = false;
        }
    }

    private void Start()
    {
        //Movimiento
        controlador = img.GetComponent<CharacterController>();

        //Revision de boton
        ray = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        nombreTexto.text = personaje;
    }

    private void Update()
    {
        img.transform.position += axis * (Time.deltaTime * speed);

        ChecarBoton();
    }

    public Image Image
    {
        get => Image;
        set
        {
            Image = value;
            img.enabled = Image != null;
        }
    }
    #endregion CORE SLOT

    #region MOVIMIENTO
    [Header("Movement Stats")]
    [SerializeField] private float speed = 2.0f;
    private Vector3 axis = Vector2.zero;

    public void Input_MovimientoImg(InputAction.CallbackContext context)
    {
        axis = context.ReadValue<Vector2>();
    }

    void RaycastCharacter()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Character"))
            {
                selectedCharacter = result.gameObject.name;
                Debug.Log("Selected Character: " + selectedCharacter);
                // Handle character selection logic
                break;
            }
        }
    }
    #endregion MOVIMIENTO

    #region CHECAR BOTON
    [Header("Check Button")]
    [SerializeField] private GraphicRaycaster ray;
    [SerializeField] private EventSystem eventSystem;
    private Button botonActual;

    public void Input_SeleccionarPersonaje(InputAction.CallbackContext context)
    {
        if(botonActual != null)
        {
            botonActual.onClick.Invoke();
        }
    }

    void ChecarBoton()
    {
        botonActual = null;

        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = img.transform.position;

        List<RaycastResult> results = new List<RaycastResult>();
        ray.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();

            if (button != null)
            {
                botonActual = button;
                personaje = button.name;
                break;
            }
        }
    }
    #endregion CHECAR BOTON
    private bool blanco = true;

    public void CambiarColor(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        blanco = !blanco;
        img.color = blanco ? Color.white : Color.red;
    }
}
