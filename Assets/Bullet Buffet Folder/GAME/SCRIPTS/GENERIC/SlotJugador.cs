using UnityEngine;
using UnityEngine.InputSystem;

public class SlotJugador : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputDevice control;

    public GameObject pfJugador;

    public InputDevice Control
    {
        get => control;
        set => control = value;
    }
}
