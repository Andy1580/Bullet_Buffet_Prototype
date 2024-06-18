using System.Collections.Generic;
using System.IO;
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

    private void Start()
    {
        InicializarPartida();
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

    private void InicializarPartida()
    {
        Partida partida = new Partida();

        int i = 0;

        foreach(var par in dicGamepads)
        {
            partida.controlesId[i] = par.Key.deviceId;
            partida.personaje[i] = par.Value.personaje;

            i++;
        }

        //Se convierte el objeto partida a Json
        string json = JsonUtility.ToJson(partida);
        //Generamos ruta
        string ruta = Path.Combine(Application.persistentDataPath, "Jugadores_Save Data");
        print(ruta);
        //Lo guardamos en el almacenamiento
        File.WriteAllText(ruta, json);
    }

    [SerializeField] private string personaje1;
    [SerializeField] private string personaje2;
    [SerializeField] private string personaje3;
    [SerializeField] private string personaje4;

    //Preguntar al profe si es correcto de esta forma
    public void Personaje1()
    {
        //personaje1 = SlotJugador.personaje;

        foreach(var jugador in dicGamepads)
        {
            personaje1 = jugador.Value.personaje;
        }
    }

}
