using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager self;

    private PlayerInput[] inputs;
    private Dictionary<Gamepad, PlayerInput> dicControles = new Dictionary<Gamepad, PlayerInput>();

    [SerializeField] private Canvas canvas;
    public static Canvas Canvas => self.canvas;

    private Dictionary<int, int> equipo = new Dictionary<int, int>(); // gamepadId -> equipo
    private Dictionary<int, string> personaje = new Dictionary<int, string>(); // gamepadId -> personaje

    [SerializeField] private GameObject panelSelectTeam;
    [SerializeField] private GameObject panelSelectCh;

    private void Awake()
    {
        self = this;

        inputs = GetComponentsInChildren<PlayerInput>();
        Awake_DesactivarControles();
        Awake_AsignarControles();
        InputSystem.onDeviceChange += CambiosEnControl;
    }

    private void Start()
    {
        panelSelectTeam.SetActive(true);
        panelSelectCh.SetActive(false);
    }

    void Awake_DesactivarControles()
    {
        foreach (PlayerInput input in inputs)
            input.gameObject.SetActive(false);
    }

    void Awake_AsignarControles()
    {
        var gamepads = Gamepad.all;

        for (int i = 0; i < gamepads.Count; i++)
        {
            inputs[i].gameObject.SetActive(true);
            dicControles[gamepads[i]] = inputs[i];
        }
    }

    private void CambiosEnControl(InputDevice device, InputDeviceChange cambio)
    {
        if (!(device is Gamepad))
            return;

        Gamepad gamepad = device as Gamepad;

        if (cambio == InputDeviceChange.Added)
        {
            foreach (PlayerInput input in inputs)
            {
                if (!input.gameObject.activeSelf)
                {
                    input.gameObject.SetActive(true);
                    dicControles[gamepad] = input;
                    break;
                }
            }
        }
        else if (cambio == InputDeviceChange.Removed)
        {
            dicControles[gamepad].gameObject.SetActive(false);
            dicControles.Remove(gamepad);
        }
    }

    public void SeleccionarEquipo(Gamepad gamepad, int equipoSeleccionado)
    {
        if (dicControles.ContainsKey(gamepad))
        {
            int gamepadId = gamepad.deviceId;
            this.equipo[gamepadId] = equipoSeleccionado;
            ActivarPanelSeleccionarPersonajes();
            Debug.Log($"Gamepad {gamepad.deviceId} seleccionó el equipo {equipoSeleccionado}");
        }
    }

    public void SeleccionarPersonaje(Gamepad gamepad, string personajeSeleccionado)
    {
        if (dicControles.ContainsKey(gamepad))
        {
            int gamepadId = gamepad.deviceId;
            this.personaje[gamepadId] = personajeSeleccionado;
            Debug.Log($"Gamepad {gamepad.deviceId} seleccionó el personaje {personajeSeleccionado}");
        }
    }

    private void ActivarPanelSeleccionarPersonajes()
    {
        if (dicControles.Count >= 2 && equipo.Count > 1)
        {
            panelSelectTeam.SetActive(false);
            panelSelectCh.SetActive(true);
        }
    }

    public void RecopilarInformacion()
    {
        if (dicControles.Count < 2 || equipo.Count < 2 || personaje.Count < 2)
            return;


        InfoLobby infoLobby = new InfoLobby();
        infoLobby.equipos = this.equipo;
        infoLobby.personajes = this.personaje;

        string json = JsonUtility.ToJson(infoLobby);
        Debug.Log("Se mando la informacion al Game Manager: " + json);

        GameManager.Instance.RecibirInformacionLobby(json);
    }
}
