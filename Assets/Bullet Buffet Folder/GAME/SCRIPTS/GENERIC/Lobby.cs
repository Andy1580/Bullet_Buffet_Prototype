using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class Lobby : MonoBehaviour
{
    public static Lobby InstanceLobby;

    [SerializeField] private Transform PanelSlots;

    private SlotJugador[] slots;

    private Dictionary<Gamepad, SlotJugador> dicGamepads;

    private void Awake()
    {
        if(InstanceLobby != null)
        {
            Destroy(this);
        }
        else
        {
            InstanceLobby = this;
            DontDestroyOnLoad(this.gameObject);
        }

        dicGamepads = new Dictionary<Gamepad, SlotJugador>();
        slots = PanelSlots.GetComponentsInChildren<SlotJugador>();
        InputSystem.onDeviceChange += CambioEnControl;
        RevisarControlesConectados();
    }

    private void RevisarControlesConectados()
    {
        foreach(Gamepad control in Gamepad.all)
        {
            CambioEnControl(control, InputDeviceChange.Added);
        }
    }

    private void CambioEnControl(InputDevice device, InputDeviceChange cambio)
    {

        if (!(device is Gamepad))
            return;

        Gamepad controlCambio = device as Gamepad;

        if (cambio == InputDeviceChange.Added)
        {
            foreach (SlotJugador slot in slots)
            {
                if (slot.Control == null)
                {
                    slot.Control = controlCambio;

                    dicGamepads.Add(controlCambio, slot);

                    break;
                }

            }

        }

        else if (cambio == InputDeviceChange.Removed)
        {
            if (!dicGamepads.ContainsKey(controlCambio))
                return;

            SlotJugador slotRemovido = dicGamepads[controlCambio];
           
            slotRemovido.Control = null;
        }


    }
}
