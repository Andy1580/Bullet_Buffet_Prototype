using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotJugador : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Image img;

    private Gamepad control;


    public Gamepad Control
    {
        get => control;
        set
        {
            control = value;
            img.enabled = control != null;
        }
    }

    private bool blanco = true;

    public void CambiarColor(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        blanco = !blanco;
        img.color = blanco ? Color.white : Color.red;
    }
}
