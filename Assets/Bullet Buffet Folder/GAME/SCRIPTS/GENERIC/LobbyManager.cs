using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LobbyManager : MonoBehaviour
{


    public static List<ControlSystem> listaCS = new List<ControlSystem>();
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public static LobbyManager self;

    private PlayerInput[] inputs;
    [HideInInspector] public static Dictionary<Gamepad, PlayerInput> dicControles = new Dictionary<Gamepad, PlayerInput>();
    private static List<ControlSystem> listaCsEquipo1 = new List<ControlSystem>();
    private static List<ControlSystem> listaCsEquipo2 = new List<ControlSystem>();

    [SerializeField] private Canvas canvas;
    public static Canvas Canvas => self.canvas;

    [HideInInspector] public static Dictionary<int, int> equipo = new Dictionary<int, int>(); // gamepadId -> equipoJugador
    [HideInInspector] public static Dictionary<int, string> personaje = new Dictionary<int, string>(); // gamepadId -> personaje

    [SerializeField] private GameObject panelSelectTeam;
    [SerializeField] private GameObject panelSelectCh;
    [SerializeField] private GameObject botonJugar;
    [SerializeField] private GameObject tiras;

    public static bool escogiendoEquipo = true;

    public static int[] equipoControles = new[] { 0, 0, 0, 0 };

    public static int equipo1 = 0;
    public static int equipo2 = 0;

    //Variables heredadas del ControlSystem
    public List<RectTransform> slotsEquipo1;
    public List<RectTransform> slotsEquipo2;
    public RectTransform[] posicionesSlotsEquipo1;
    public RectTransform[] posicionesSlotsEquipo2;

    [SerializeField] private List<TMP_Text> textosEquipo1;
    [SerializeField] private List<TMP_Text> textosEquipo2;
    [SerializeField] private Sprite defaultSprite;
    // Nuevas variables para im�genes
    //[SerializeField] private Image[] teamImages;
    //[SerializeField] private Image[] characterImages;

    private void Awake()
    {
        self = this;

        inputs = GetComponentsInChildren<PlayerInput>();
        Awake_DesactivarControles();
        Awake_AsignarControles();
        InputSystem.onDeviceChange += CambiosEnControl;

        Awake_AcomodarSlots();
    }

    private void OnEnable()
    {
        listaCsEquipo1.Clear();
        listaCsEquipo2.Clear();

        enProgreso = false;
    }

    private IEnumerator CargarMenuConRetraso(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "LOBBY")
        {
            panelSelectTeam.SetActive(true);
            panelSelectCh.SetActive(false);
            botonJugar.SetActive(false);
            tiras.SetActive(false);

            escogiendoEquipo = true;
            equipoControles = new int[equipoControles.Length];

            equipo1 = 0;
            equipo2 = 0;

            equipo = new Dictionary<int, int>();
            personaje = new Dictionary<int, string>();
        }
        else
        {
            panelSelectTeam.SetActive(false);
            panelSelectCh.SetActive(false);
            botonJugar.SetActive(false);
            tiras.SetActive(false);
        }
        
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
                    Awake_AcomodarSlots();
                    break;
                }
            }
        }
        else if (cambio == InputDeviceChange.Removed)
        {
            dicControles[gamepad].gameObject.SetActive(false);
            dicControles.Remove(gamepad);
            Awake_AcomodarSlots();
        }
    }

    private void Awake_AcomodarSlots()
    {
        foreach (RectTransform slot in slotsEquipo1)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in slotsEquipo2)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in posicionesSlotsEquipo1)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (RectTransform slot in posicionesSlotsEquipo2)
        {
            slot.gameObject.SetActive(false);
        }

        int gamepadCount = Gamepad.all.Count;

        if (gamepadCount == 2)
        {
            posicionesSlotsEquipo1[1].gameObject.SetActive(true);
            posicionesSlotsEquipo2[0].gameObject.SetActive(true);

            slotsEquipo1[0].position = posicionesSlotsEquipo1[1].position;
            slotsEquipo2[0].position = posicionesSlotsEquipo2[0].position;
        }
        else if (gamepadCount == 4)
        {
            foreach (RectTransform slot in posicionesSlotsEquipo1) slot.gameObject.SetActive(true);
            foreach (RectTransform slot in posicionesSlotsEquipo2) slot.gameObject.SetActive(true);

            slotsEquipo1[0].position = posicionesSlotsEquipo1[0].position;
            slotsEquipo1[1].position = posicionesSlotsEquipo1[1].position;
            slotsEquipo2[0].position = posicionesSlotsEquipo2[0].position;
            slotsEquipo2[1].position = posicionesSlotsEquipo2[1].position;
        }
    }

    public static void SeleccionarEquipo(Gamepad gamepad, int equipoSeleccionado)
    {
        equipoControles[equipoSeleccionado - 1]++;

        int suma = equipoControles[0] + equipoControles[1] + equipoControles[2] + equipoControles[3];

        if (Gamepad.all.Count == 2)
        {
            if (suma == 2)
            {
                AsignarPunterosYActivarPanel();
                escogiendoEquipo = false;
            }
        }
        else if (Gamepad.all.Count == 4)
        {
            if (suma == 4)
            {
                AsignarPunterosYActivarPanel();
                escogiendoEquipo = false;
            }
        }

    }

    private static void AsignarPunterosYActivarPanel()
    {
        foreach (var slot in self.slotsEquipo1)
        {
            if (slot == null)
            {
                Debug.LogWarning("Uno de los slots de equipo 1 es null y no se puede asignar el puntero.");
                continue;
            }
        }

        foreach (var slot in self.slotsEquipo2)
        {
            if (slot == null)
            {
                Debug.LogWarning("Uno de los slots de equipo 2 es null y no se puede asignar el puntero.");
                continue;
            }
        }

        // Activar slots según la cantidad de GamePads conectados
        int gamepadCount = Gamepad.all.Count;

        if (gamepadCount == 2)
        {
            // Solo activamos el primer slot de cada equipo
            self.slotsEquipo1[0].gameObject.SetActive(true);
            self.slotsEquipo2[0].gameObject.SetActive(true);
        }
        else if (gamepadCount == 4)
        {
            // Activamos todos los slots de ambos equipos
            foreach (RectTransform slot in self.slotsEquipo1)
            {
                slot.gameObject.SetActive(true);
            }

            foreach (RectTransform slot in self.slotsEquipo2)
            {
                slot.gameObject.SetActive(true);
            }
        }

        int i = 0;

        // Añadir los ControlSystem a los equipos correspondientes y posicionar punteros
        foreach (var par in dicControles)
        {
            ControlSystem cs = par.Value.GetComponent<ControlSystem>();
            cs.gamepadID = par.Key.deviceId;
            cs.originalID = i++;

            listaCS.Add(cs);  

            if (cs.equipoJugador == 1) listaCsEquipo1.Add(cs);
            else listaCsEquipo2.Add(cs);
        }

        // Posicionar y activar los punteros de los jugadores en sus slots respectivos
        self.PosicionarYActivarPunteros();
        ActivarPanelSeleccionarPersonajes();
    }

    private void PosicionarYActivarPunteros()
    {
        for (int i = 0; i < listaCsEquipo1.Count; i++)
        {
            MoverPuntero(listaCsEquipo1[i], slotsEquipo1[i], textosEquipo1[i]);
        }

        for (int i = 0; i < listaCsEquipo2.Count; i++)
        {
            MoverPuntero(listaCsEquipo2[i], slotsEquipo2[i], textosEquipo2[i]);
        }
    }

    private void MoverPuntero(ControlSystem cs, RectTransform slot, TMP_Text textoSlot)
    {
        // Comprobar que `cs` y `slot` no son null antes de continuar
        if (cs == null || slot == null)
        {
            Debug.LogWarning("El ControlSystem o el slot es null y no se puede mover el puntero.");
            return;
        }

        cs.puntero.gameObject.SetActive(true);
        cs.puntero.position = slot.position;

        // Comprobar que `textoSlot` y `cs.spritePersonaje` no son null antes de usarlos
        if (textoSlot != null)
        {
            textoSlot.text = cs.jugadorNickName;
        }

        if (slot.childCount > 1)
        {
            cs.spritePersonaje = slot.GetChild(1).GetComponent<Image>();
            if (cs.spritePersonaje != null)
            {
                Debug.Log($"Puntero del jugador {cs.jugadorNickName} se movió al slot {slot.name}");
            }
            else
            {
                Debug.LogWarning("El componente Image para el sprite del personaje es null.");
            }
        }
    }

    private static bool enProgreso = false;

    public static void RechazarEquipo()
    {
        if (enProgreso) return; // Si ya estamos procesando, ignoramos nuevas llamadas
        enProgreso = true;

        try
        {
            if (self.panelSelectTeam.activeSelf)
            {
                // Validar si todos los jugadores están en equipo 0
                bool todosEquipo0 = true;

                foreach (var par in dicControles)
                {
                    ControlSystem cs = par.Value.GetComponent<ControlSystem>();
                    if (cs != null && cs.equipoJugador != 0)
                    {
                        todosEquipo0 = false;
                        break;
                    }
                }

                if (todosEquipo0) // Si todos los jugadores tienen equipo 0
                {
                    Debug.Log("Todos los jugadores están en equipo 0. Cargando el menú...");
                    self.StartCoroutine(self.CargarMenuConRetraso(0.8f)); // Llama la corrutina
                    return; // Detenemos aquí para evitar ejecutar el resto del método
                }
            }

            // Si al menos un jugador tiene equipo diferente de 0, ejecutamos el resto del código
            Debug.Log("Se resetearon los equipos");

            // Limpia el arreglo equipoControles, establece todos los elementos en 0
            Array.Clear(equipoControles, 0, equipoControles.Length);
            equipo1 = 0;
            equipo2 = 0;

            listaCsEquipo1.Clear();
            listaCsEquipo2.Clear();

            // Limpiar el diccionario de equipo y personaje
            equipo.Clear();
            personaje.Clear();

            // Ciclar a través de todos los `ControlSystem` y resetear variables
            foreach (var par in dicControles)
            {
                ControlSystem cs = par.Value.GetComponent<ControlSystem>();
                if (cs != null)
                {
                    cs.ResetearVariables();
                }
            }

            // Limpiar los sprites de los slots
            foreach (var slot in self.slotsEquipo1)
            {
                if (slot.childCount > 1)
                {
                    Image personajeImage = slot.GetChild(1).GetComponent<Image>();
                    if (personajeImage != null)
                    {
                        personajeImage.sprite = self.defaultSprite;
                    }
                }
            }

            foreach (var slot in self.slotsEquipo2)
            {
                if (slot.childCount > 1)
                {
                    Image personajeImage = slot.GetChild(1).GetComponent<Image>();
                    if (personajeImage != null)
                    {
                        personajeImage.sprite = self.defaultSprite;
                    }
                }
            }

            self.botonJugar.gameObject.SetActive(false);
            escogiendoEquipo = true;
            ActivarPanelSeleccionarEquipo();
        }
        finally
        {
            enProgreso = false; // Resetear bandera
        }
    }

    public void SeleccionarPersonaje(Gamepad gamepad, string personajeSeleccionado)
    {
        if (dicControles.ContainsKey(gamepad))
        {
            int gamepadId = gamepad.deviceId;
            personaje[gamepadId] = personajeSeleccionado;
            //Debug.Log($"Gamepad {gamepad.deviceId} seleccion� el personaje {personajeSeleccionado}");
        }

        if (Gamepad.all.Count == 2)
        {
            if (personaje.Count == 2)
            {
                botonJugar.SetActive(true);
                tiras.SetActive(true);
            }
            else
            {
                botonJugar.SetActive(false);
                tiras.SetActive(false);
            }
        }
        else if (Gamepad.all.Count == 4)
        {
            if (personaje.Count == 4)
            {
                botonJugar.SetActive(true);
                tiras.SetActive(true);
            }
            else
            {
                botonJugar.SetActive(false);
                tiras.SetActive(false);
            }
        }
    }



    private static void ActivarPanelSeleccionarPersonajes()
    {
        self.panelSelectTeam.SetActive(false);
        self.panelSelectCh.SetActive(true);
    }

    public static void ActivarPanelSeleccionarEquipo()
    {
        self.panelSelectTeam.SetActive(true);
        self.panelSelectCh.SetActive(false);
        self.tiras.SetActive(false);
    }

    [HideInInspector] public static Dictionary<int, Gamepad> idToGamepad = new Dictionary<int, Gamepad>();

    public void RegistrarGamepad(Gamepad gamepad)
    {
        int gamepadId = gamepad.deviceId;
        if (!idToGamepad.ContainsKey(gamepadId))
        {
            idToGamepad[gamepadId] = gamepad;
            Debug.Log($"Gamepad registrado correctamente: {gamepadId} para {gamepad}");
        }
        else
        {
            Debug.LogWarning($"El Gamepad con id {gamepadId} ya estaba registrado.");
        }
    }

    public Gamepad GetGamepadById(int gamepadId)
    {
        if (idToGamepad.ContainsKey(gamepadId))
        {
            Debug.Log($"GetGamepadById: Se encontró el Gamepad con id {gamepadId}");
            return idToGamepad[gamepadId];
        }

        Debug.LogWarning($"GetGamepadById: No se encontró el Gamepad con id {gamepadId}");
        return null;
    }

    public void RecopilarInformacion()
    {
        IniciarPartida();
        //GuardarInformacionJugadores();
        //Debug.Log("Iniciando recopilaci�n de informaci�n...");

        if (equipo.Count < 2 && personaje.Count < 2)
        {
            Debug.LogWarning("No hay suficientes datos para iniciar la partida.");
            return;
        }

        AudioManager.instance.PlaySound("botonJugar");


        return;
        InfoLobby infoLobby = new InfoLobby();

        foreach (var control in dicControles)
        {
            Gamepad gamepad = control.Key;
            int gamepadId = gamepad.deviceId;

            if (equipo.ContainsKey(gamepadId) && personaje.ContainsKey(gamepadId))
            {
                //Debug.Log($"Agregando informaci�n del jugadorImpactoBala {gamepadId} con equipoJugador {equipoJugador[gamepadId]} y personaje {personaje[gamepadId]}");
                infoLobby.AddPlayerInfo(gamepadId, equipo[gamepadId], personaje[gamepadId]);
            }
            else
            {
                Debug.LogWarning($"Falta informaci�n para el gamepad {gamepadId}");
            }
        }

        string json = JsonUtility.ToJson(infoLobby);
        //Debug.Log("JSON generado: " + json);
        AudioManager.instance.StopSound("menu");
        GameManager.Instance.RecibirInformacionLobby(json);

    }


    public void IniciarPartida()
    {
        //Obtenemos el numero de jugadores
        int nJugadores = listaCsEquipo1.Count + listaCsEquipo2.Count;

        PlayerPrefs.SetInt("nJugadores", nJugadores);

        // Convertir Gamepad.all a una lista para poder usar FindIndex
        List<Gamepad> gamepadList = new List<Gamepad>(Gamepad.all);

        //Si son 2 JUGADORES
        if (nJugadores == 2)
        {
            //Obtener los Control System
            ControlSystem c1 = listaCS[0];
            ControlSystem c2 = listaCS[1];

            PlayerPrefs.SetInt("P1_GID", c1.gamepadID);
            PlayerPrefs.SetString("P1_Personaje", c1.selectedCharacter);
            PlayerPrefs.SetInt("P1_Equipo", c1.equipoJugador);

            PlayerPrefs.SetInt("P2_GID", c2.gamepadID);
            PlayerPrefs.SetString("P2_Personaje", c2.selectedCharacter);
            PlayerPrefs.SetInt("P2_Equipo", c2.equipoJugador);

        }
        //Si son 4 JUGADORES
        else
        {
            //Obtener los Control System
            ControlSystem c1 = listaCS[0];
            ControlSystem c2 = listaCS[1];
            ControlSystem c3 = listaCS[2];
            ControlSystem c4 = listaCS[3];

            PlayerPrefs.SetInt("P1_GID", c1.gamepadID);
            PlayerPrefs.SetString("P1_Personaje", c1.selectedCharacter);
            PlayerPrefs.SetInt("P1_Equipo", c1.equipoJugador);

            PlayerPrefs.SetInt("P2_GID", c2.gamepadID);
            PlayerPrefs.SetString("P2_Personaje", c2.selectedCharacter);
            PlayerPrefs.SetInt("P2_Equipo", c2.equipoJugador);

            PlayerPrefs.SetInt("P3_GID", c3.gamepadID);
            PlayerPrefs.SetString("P3_Personaje", c3.selectedCharacter);
            PlayerPrefs.SetInt("P3_Equipo", c3.equipoJugador);

            PlayerPrefs.SetInt("P4_GID", c4.gamepadID);
            PlayerPrefs.SetString("P4_Personaje", c4.selectedCharacter);
            PlayerPrefs.SetInt("P4_Equipo", c4.equipoJugador);
        }

        PlayerPrefs.Save();

        GameManager.Instance.CargarEscenaProfe();
    }

    public void GuardarInformacionJugadores()
    {
        int nJugadores = listaCsEquipo1.Count + listaCsEquipo2.Count;
        PlayerPrefs.SetInt("nJugadores", nJugadores);

        if (nJugadores == 2)
        {
            GuardarJugador(1, listaCsEquipo1[0]);
            GuardarJugador(2, listaCsEquipo2[0]);
        }
        else if (nJugadores == 4)
        {
            GuardarJugador(1, listaCsEquipo1[0]);
            GuardarJugador(2, listaCsEquipo1[1]);
            GuardarJugador(3, listaCsEquipo2[0]);
            GuardarJugador(4, listaCsEquipo2[1]);
        }

        //// Transferir el diccionario de gamepads de LobbyManager al GameManager
        //GameManager.idDeGamepad = new Dictionary<int, Gamepad>(idToGamepad);

        PlayerPrefs.Save();
        GameManager.Instance.CargarEscenaProfe();





        RechazarEquipo();
    }

    private void GuardarJugador(int playerIndex, ControlSystem controlSystem)
    {
        PlayerPrefs.SetInt($"P{playerIndex}_GID", controlSystem.gamepadID);
        PlayerPrefs.SetString($"P{playerIndex}_Personaje", controlSystem.selectedCharacter);
        PlayerPrefs.SetInt($"P{playerIndex}_Equipo", controlSystem.equipoJugador);
    }
}
