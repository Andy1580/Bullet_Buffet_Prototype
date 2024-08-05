using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager self;

    private PlayerInput[] inputs;
    [HideInInspector] public Dictionary<Gamepad, PlayerInput> dicControles = new Dictionary<Gamepad, PlayerInput>();

    [SerializeField] private Canvas canvas;
    public static Canvas Canvas => self.canvas;

    [HideInInspector] public Dictionary<int, int> equipo = new Dictionary<int, int>(); // gamepadId -> equipo
    [HideInInspector] public Dictionary<int, string> personaje = new Dictionary<int, string>(); // gamepadId -> personaje

    [SerializeField] private GameObject panelSelectTeam;
    [SerializeField] private GameObject panelSelectCh;
    [SerializeField] private GameObject botonJugar;

    public static bool escogiendoEquipo = true;

    public static int[] equipoControles = new[] { 0, 0 };

    // Nuevas variables para imágenes
    //[SerializeField] private Image[] teamImages;
    //[SerializeField] private Image[] characterImages;

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
        botonJugar.SetActive(false);
    }

    private void MakeSingleton()
    {
        if (self != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            self = this;
            DontDestroyOnLoad(this.gameObject);
        }
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
                    RegistrarGamepad(gamepad);
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
        if(Gamepad.all.Count == 2)
        {
            if (dicControles.ContainsKey(gamepad))
            {
                int gamepadId = gamepad.deviceId;
                this.equipo[gamepadId] = equipoSeleccionado;
                Debug.Log($"Gamepad {gamepad.deviceId} seleccionó el equipo {equipoSeleccionado}");
            }

            if(dicControles.Count == 2)
            {
                ActivarPanelSeleccionarPersonajes();
            }
        }
        else if(Gamepad.all.Count == 4)
        {
            if (dicControles.ContainsKey(gamepad))
            {
                int gamepadId = gamepad.deviceId;
                this.equipo[gamepadId] = equipoSeleccionado;
                Debug.Log($"Gamepad {gamepad.deviceId} seleccionó el equipo {equipoSeleccionado}");
            }

            if (dicControles.Count == 4)
            {
                ActivarPanelSeleccionarPersonajes();
            }
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

        if(Gamepad.all.Count == 2)
        {
            if(personaje.Count == 2)
            {
                botonJugar.SetActive(true);
            }
            else
            {
                botonJugar.SetActive(false);
            }
        }
        else if(Gamepad.all.Count == 4)
        {
            if(personaje.Count == 4)
            {
                botonJugar.SetActive(true);
            }
            else
            {
                botonJugar.SetActive(false);
            }
        }
    }

    private void ActivarPanelSeleccionarPersonajes()
    {
        if (dicControles.Count >= 2 && equipo.Count >= 2)
        {
            panelSelectTeam.SetActive(false);
            panelSelectCh.SetActive(true);
        }
    }

    public void ActivarPanelSeleccionarEquipo()
    {
        panelSelectTeam.SetActive(true);
        panelSelectCh.SetActive(false);
    }

    [HideInInspector] public Dictionary<int, Gamepad> idToGamepad = new Dictionary<int, Gamepad>();

    public void RegistrarGamepad(Gamepad gamepad)
    {
        int gamepadId = gamepad.deviceId;
        if (!idToGamepad.ContainsKey(gamepadId))
        {
            idToGamepad[gamepadId] = gamepad;
            Debug.Log($"Gamepad registrado: {gamepadId}");
        }
    }

    public Gamepad GetGamepadById(int gamepadId)
    {
        if (idToGamepad.ContainsKey(gamepadId))
        {
            return idToGamepad[gamepadId];
        }

        Debug.LogWarning($"No se encontró el Gamepad con id {gamepadId}");
        return null;
    }

    public void RecopilarInformacion()
    {
        Debug.Log("Iniciando recopilación de información...");

        if (dicControles.Count < 2 || equipo.Count < 2 || personaje.Count < 2)
        {
            Debug.LogWarning("No hay suficientes datos para iniciar la partida.");
            return;
        }

        InfoLobby infoLobby = new InfoLobby();

        foreach (var control in dicControles)
        {
            Gamepad gamepad = control.Key;
            int gamepadId = gamepad.deviceId;

            if (equipo.ContainsKey(gamepadId) && personaje.ContainsKey(gamepadId))
            {
                Debug.Log($"Agregando información del jugador {gamepadId} con equipo {equipo[gamepadId]} y personaje {personaje[gamepadId]}");
                infoLobby.AddPlayerInfo(gamepadId, equipo[gamepadId], personaje[gamepadId]);
            }
            else
            {
                Debug.LogWarning($"Falta información para el gamepad {gamepadId}");
            }
        }

        string json = JsonUtility.ToJson(infoLobby);
        Debug.Log("JSON generado: " + json);

        GameManager.Instance.RecibirInformacionLobby(json);

        SceneManager.LoadScene("ANDYINGAME");
    }

    public void Input_Rechazar(Gamepad gamepad)
    {
        if (dicControles.ContainsKey(gamepad))
        {
            int gamepadId = gamepad.deviceId;
            if (equipo.ContainsKey(gamepadId))
            {
                equipo.Remove(gamepadId);
                //teamImages[gamepadId - 1].gameObject.SetActive(false); // Desactivar imagen del equipo
            }
            if (personaje.ContainsKey(gamepadId))
            {
                personaje.Remove(gamepadId);
                //characterImages[gamepadId - 1].gameObject.SetActive(false); // Desactivar imagen del personaje
            }
            Debug.Log($"Gamepad {gamepad.deviceId} ha rechazado el equipo/personaje");
        }
    }
}
