using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool inGame;

    #region RECIBIR INFORMACION

    private static List<InfoLobby.PlayerInfo> infoLobbyPlayers;
    private bool cargarJuego = false;

    public void RecibirInformacionLobby(string json)
    {
        InfoLobby infoLobby = JsonUtility.FromJson<InfoLobby>(json);
        infoLobbyPlayers = infoLobby.playerInfos;
        //SetupHUDs(infoLobbyPlayers);
        //ActivarHUD();


        //CargarEscena();
        //Invoke("CargarEscena", 0.5f);
    }

    public void CargarEscenaProfe()
    {
        if (!cargarJuego)
        {
            cargarJuego = true;
            StartCoroutine(Transicion());
            Invoke("CargarEscena", 1f);
        }
    }

    #endregion RECIBIR INFORMACION

    #region HUD

    [Header("HUD Jugadores")]
    [SerializeField] private GameObject panelHUDs;
    [SerializeField] private PlayerHUD[] slotsHUD;

    private List<PlayerHUD> playerHUDs = new List<PlayerHUD>();

    void InicializarHUD()
    {
        panelHUDs.SetActive(true);
    }

    #endregion HUD

    #region CARGAR ESCENA

    private void CargarEscena()
    {
        //EscenaDeJuego();

        if (boolMapaStreetMHS)
        {
            SceneManager.LoadScene("MapaStreetMHS");
        }
        else if (boolMapaRestaurantMHS)
        {
            SceneManager.LoadScene("MapaRestaurantMHS");
        }
        else if (boolMapaDungeonMHS)
        {
            SceneManager.LoadScene("MapaDungeonMHS");
        }
        else if (boolMapaStreetMDS)
        {
            SceneManager.LoadScene("MapaStreetMDS");
        }
        else if (boolMapaRestaurantMDS)
        {
            SceneManager.LoadScene("MapaRestaurantMDS");
        }
        else if (boolMapaDungeonMDS)
        {
            SceneManager.LoadScene("MapaDungeonMDS");
        }

        Invoke("EscenaDeJuego", 1f);
    }
    #endregion CARGAR ESCENA

    #region MUSICA

    void InicializarMusica()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            AudioManager.instance.PlaySound("menu");
            AudioManager.instance.StopSound("hechizos");
            AudioManager.instance.StopSound("duelo");
            AudioManager.instance.StopSound("audiencia");
            AudioManager.instance.StopSound("victoria");
        }
        else if ((SceneManager.GetActiveScene().name != "ANDYMENUTEST"))
        {
            AudioManager.instance.StopSound("duelo");
            AudioManager.instance.StopSound("hechizos");
        }
    }

    #endregion MUSICA

    #region GAME MANAGER
    public static GameManager Instance;

    //[SerializeField] private GameObject panelFPS;

    private GameObject lastSelectedUI;
    private bool isInitialFocus = true; //Para ignorar el foco inicial

    //public static GameObject PanelFPS => Instance.panelFPS;

    private void Awake()
    {
        MakeSingleton();

        SceneManager.sceneLoaded += OnSceneLoaded;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    private void Start()
    {

        //InicializarMusica();
        //InicializarJugadores();
        InicializarTransicion();
        InicializarTransicioninicioPartida();
    }

    private void Update()
    {
        Update_Marcador_MHS();
        //DetectarNavegacionUI();
    }

    private void FixedUpdate()
    {
        FixUpdate_Temporizador();
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // Cuando la aplicación recupera el foco, asegúrate de que el cursor esté oculto y bloqueado
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (inGame && ultimoBotonSeleccionado != null)
            {
                EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionado);
            }
        }
    }

    /*
    private void DetectarNavegacionUI()
    {
        // Verificar si hay un EventSystem activo
        if (EventSystem.current == null) return;

        // Obtener el objeto actualmente seleccionado
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        // Si el objeto seleccionado cambió
        if (currentSelected != null && currentSelected != lastSelectedUI)
        {
            if (!isInitialFocus) // Ignorar el primer cambio de foco
            {
                Debug.Log("Navegación detectada en la UI: " + currentSelected.name);

                AudioManager.instance.PlaySound("navegar");
            }

            // Actualizar el último seleccionado
            lastSelectedUI = currentSelected;

            // Ignorar el foco inicial después del primer cambio
            isInitialFocus = false;
        }
    }
    */
    #endregion GAME MANAGER

    #region ESCENA JUEGO

    //En este metodo se pone todo lo que quieras que pase al cargar una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetiarVariables();
        InicializarMusica();
    }

    void ResetearParaVolverAJugar()
    {

    }

    void EscenaDeJuego()
    {
        InicializarCamara();
        InicializarMusica();
        InicializarHUD();
        //IniciarPartida();
        InicializarSpawnsPoints();
        //Invoke("IniciarPartida", 0.02f); PROFE
        Invoke("IniciarPartidaProfe", 0.05f);
        //InicializarMapas();
        InicializarPuntaje();
        //InicializarComponentesJugadores();
        InicializarPausa();
        AudioManager.instance.PlaySound("audiencia");
        AudioManager.instance.StopSound("menu");

        if (modoHS)
        {
            //Aqui ira todo lo que necesita el MHS
            magosPrincipales.SetActive(true);
            InicializarMHS();
            InicializarMarcadorMHS();
            AudioManager.instance.PlaySound("hechizos");
        }
        else if (modoDS)
        {
            //Aqui ira todo lo que necesita el MDS
            InicializarMDS();
            InicializarMarcadorMDS();
            InicializarTemporizador();
            AudioManager.instance.PlaySound("duelo");
        }

        StartCoroutine(TransicionInicioPartida());
    }

    void PrepararEscenaDeJuego()
    {
        inGame = true;
        Invoke("InicializarEnemySpawn", 5);

        if (modoHS)
        {
            AudioManager.instance.PlaySound("opHS");
        }
        else if (modoDS)
        {
            isRunning = true;
            AudioManager.instance.PlaySound("opDS");
            //pistaPintable.SetActive(true);
        }
    }

    public void ResetarBooleanosImportantesGM()
    {
        //Booleanos Partida
        modoHS = false;
        modoDS = false;
    }

    public void OcultarElementosImportantes()
    {
        if (panelTiempoAgotado != null)
            panelTiempoAgotado.SetActive(false);

        if (panelTemporizador != null)
            panelTemporizador.SetActive(false);

        if (panelFinish != null)
            panelFinish.SetActive(false);

        //Modo Hechizos Sazonados
        if (magosPrincipales != null)
            magosPrincipales.SetActive(false);

        if (panelMarcadorMHS != null)
            panelMarcadorMHS.SetActive(false);

        if (camaraObjeto != null)
            camaraObjeto.SetActive(false);

        if (panelMarcadorMDS != null)
            panelMarcadorMDS.SetActive(false);

        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (panelControles != null)
            panelControles.SetActive(false);

        if (panelConfirmacionSalida != null)
            panelConfirmacionSalida.SetActive(false);

        if (panelHUDs != null)
            panelHUDs.SetActive(false);

        inGame = false;
        deadEnemy = true;
    }

    public void ResetearVariablesEnLobby()
    {
        enPausa = false;

        jugadoresEquipo1 = new List<Jugador>();
        jugadoresEquipo2 = new List<Jugador>();

        cargarJuego = false;

        faltan15Seg = false;

        equipo1Ganado = false;
        equipo2Ganado = false;

        isRunning = false;

        if (panelTiempoAgotado != null)
            panelTiempoAgotado.SetActive(false);

        if (panelTemporizador != null)
            panelTemporizador.SetActive(false);

        //Paneles de condicion de Victoria
        if (panelFinish != null)
            panelFinish.SetActive(false);


        //Modo Hechizos Sazonados
        if (magosPrincipales != null)
            magosPrincipales.SetActive(false);

        if (panelMarcadorMHS != null)
            panelMarcadorMHS.SetActive(false);

        if (camaraObjeto != null)
            camaraObjeto.SetActive(false);

        puntosAGanarTeam1 = puntajeInicial;
        puntosAGanarTeam2 = puntajeInicial;

        //Modo Duelo De Salsas
        //pistaPintable.SetActive(false);
        if (panelMarcadorMDS != null)
            panelMarcadorMDS.SetActive(false);

        //Pausa
        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (panelControles != null)
            panelControles.SetActive(false);

        if (panelConfirmacionSalida != null)
            panelConfirmacionSalida.SetActive(false);

        //Jugadores
        activePlayers = new List<PlayerController>();

        //Spawn de Enemigos
        deadEnemy = true;
        enemigosInstanciados = new List<GameObject>();
        DestruirEnemigosActivos();

        //Cerrar el HUD
        if (panelHUDs != null)
            panelHUDs.SetActive(false);

        //Booleano para juego
        inGame = false;
    }

    public void ResetiarVariables()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {

            ////Resetear Diccionario de Gamepads
            //if(idDeGamepad == null) idDeGamepad = new Dictionary<int, Gamepad>();

            enPausa = false;

            jugadoresEquipo1 = new List<Jugador>();
            jugadoresEquipo2 = new List<Jugador>();

            cargarJuego = false;

            faltan15Seg = false;

            equipo1Ganado = false;
            equipo2Ganado = false;

            boolMapaStreetMHS = false;
            boolMapaDungeonMHS = false;
            boolMapaRestaurantMHS = false;
            boolMapaStreetMDS = false;
            boolMapaDungeonMDS = false;
            boolMapaRestaurantMDS = false;

            //Temporizador
            totalTime = 60;
            remainingTime = totalTime;
            isRunning = false;

            if (panelTiempoAgotado != null)
                panelTiempoAgotado.SetActive(false);

            if (panelTemporizador != null)
                panelTemporizador.SetActive(false);

            //Puntaje
            puntosParaGanar = 1;

            //Paneles de condicion de Victoria
            if (panelFinish != null)
                panelFinish.SetActive(false);


            //Modo Hechizos Sazonados
            if (magosPrincipales != null)
                magosPrincipales.SetActive(false);

            if (panelMarcadorMHS != null)
                panelMarcadorMHS.SetActive(false);

            if (camaraObjeto != null)
                camaraObjeto.SetActive(false);

            puntosAGanarTeam1 = puntajeInicial;
            puntosAGanarTeam2 = puntajeInicial;

            //Modo Duelo De Salsas
            //pistaPintable.SetActive(false);
            if (panelMarcadorMDS != null)
                panelMarcadorMDS.SetActive(false);

            //Pausa
            if (panelPausa != null)
                panelPausa.SetActive(false);

            if (panelControles != null)
                panelControles.SetActive(false);

            if (panelConfirmacionSalida != null)
                panelConfirmacionSalida.SetActive(false);

            //Jugadores
            activePlayers = new List<PlayerController>();

            //Spawn de Enemigos
            deadEnemy = true;
            enemigosInstanciados = new List<GameObject>();
            DestruirEnemigosActivos();

            //Cerrar el HUD
            if (panelHUDs != null)
                panelHUDs.SetActive(false);

            //Booleano para juego
            inGame = false;

            //Lista de info jugadores
            //InicializarJugadores();
        }
        else return;
    }
    #endregion ESCENA JUEGO

    #region PARTIDA
    public bool _modoHS = modoHS;
    public bool _modoDS = modoDS;
    public static bool boolMapaStreetMHS;
    public static bool boolMapaDungeonMHS;
    public static bool boolMapaRestaurantMHS;
    public static bool boolMapaStreetMDS;
    public static bool boolMapaDungeonMDS;
    public static bool boolMapaRestaurantMDS;
    public static bool modoHS;
    public static bool modoDS;

    public static List<PlayerController> activePlayers = new List<PlayerController>();

    private Transform respawnJ1;
    private Transform respawnJ2;
    private Transform respawnJ3;
    private Transform respawnJ4;

    private List<PlayerController> equipo1 = new List<PlayerController>();
    private List<PlayerController> equipo2 = new List<PlayerController>();

    private PlayerController[,] equipos;

    [SerializeField] public int nJugadores;

    //List<Jugador> jugadores = new List<Jugador>();

    List<Jugador> jugadoresEquipo1 = new List<Jugador>();
    List<Jugador> jugadoresEquipo2 = new List<Jugador>();

    //[HideInInspector] public static Dictionary<int, Gamepad> idDeGamepad;

    // Listas de respawns por equipo
    private List<Transform> respawnsEquipo1;
    private List<Transform> respawnsEquipo2;

    public void IniciarPartidaProfe()
    {
        nJugadores = PlayerPrefs.GetInt("nJugadores");

        // Cargar jugadores desde PlayerPrefs y dividirlos en equipos
        for (int i = 1; i <= nJugadores; i++)
        {
            Jugador jugador = CrearJugadorDesdePrefs(i);

            // Añadir a la lista correspondiente según el equipo
            if (jugador.equipo == 1)
                jugadoresEquipo1.Add(jugador);
            else if (jugador.equipo == 2)
                jugadoresEquipo2.Add(jugador);
        }

        // Instanciar jugadores y asignarles los respawn adecuados
        if (nJugadores == 2)
        {
            // Asignación de puntos de respawn según equipos (modos de 2v2)
            respawnJ1 = modo1v1spawnTeam1;
            respawnJ2 = modo1v1spawnTeam2;

            // Para 2 jugadores, un jugador por equipo
            if (jugadoresEquipo1.Count > 0) InstanciarJugadorProfe(jugadoresEquipo1[0], respawnJ1);
            if (jugadoresEquipo2.Count > 0) InstanciarJugadorProfe(jugadoresEquipo2[0], respawnJ2);

            DeshabilitarMovimientoJugadores();
            DeshabilitarDisparo();
        }
        else if (nJugadores == 4)
        {
            // Asignación de puntos de respawn según equipos (modos de 2v2)
            respawnJ1 = modo2v2spawnTeam1_1;
            respawnJ2 = modo2v2spawnTeam1_2;
            respawnJ3 = modo2v2spawnTeam2_1;
            respawnJ4 = modo2v2spawnTeam2_2;

            // Para 4 jugadores, 2 jugadores por equipo
            if (jugadoresEquipo1.Count > 0) InstanciarJugadorProfe(jugadoresEquipo1[0], respawnJ1);
            if (jugadoresEquipo1.Count > 1) InstanciarJugadorProfe(jugadoresEquipo1[1], respawnJ2);

            if (jugadoresEquipo2.Count > 0) InstanciarJugadorProfe(jugadoresEquipo2[0], respawnJ3);
            if (jugadoresEquipo2.Count > 1) InstanciarJugadorProfe(jugadoresEquipo2[1], respawnJ4);

            DeshabilitarMovimientoJugadores();
            DeshabilitarDisparo();
        }
    }


    private Jugador CrearJugadorDesdePrefs(int playerIndex)
    {
        int gamepadId = PlayerPrefs.GetInt($"P{playerIndex}_GID");
        string personaje = PlayerPrefs.GetString($"P{playerIndex}_Personaje");
        int equipo = PlayerPrefs.GetInt($"P{playerIndex}_Equipo");

        return new Jugador(playerIndex - 1, gamepadId, personaje, equipo);
    }
    /*
    public void IniciarPartidaProfe()
    {
        if (juegoIniciado) return;
        juegoIniciado = true;

        nJugadores = PlayerPrefs.GetInt("nJugadores");

        //Si son 2 jugadores
        if (nJugadores == 2)
        {
            //Respawns
            respawnJ1 = modo1v1spawnTeam1;
            respawnJ2 = modo1v1spawnTeam2;

            //Obtener datos
            int indexC1 = PlayerPrefs.GetInt("P1_G_Index");
            int indexC2 = PlayerPrefs.GetInt("P2_G_Index");

            // Recuperar Gamepad usando el índice
            Gamepad gamepadC1 = Gamepad.all[indexC1];
            Gamepad gamepadC2 = Gamepad.all[indexC2];

            Jugador j1 = new Jugador(0, gamepadC1.deviceId, PlayerPrefs.GetString("P1_Personaje"), 1);
            Jugador j2 = new Jugador(1, gamepadC2.deviceId, PlayerPrefs.GetString("P2_Personaje"), 2);

            //Instanciar y Guardar
            equipos = new PlayerController[,]
            {
                {InstanciarJugadorProfe(j1,respawnJ1)},
                {InstanciarJugadorProfe(j2,respawnJ2)},
            };
        }
        else
        {
            //Respawns
            respawnJ1 = modo2v2spawnTeam1_1;
            respawnJ2 = modo2v2spawnTeam1_2;
            respawnJ3 = modo2v2spawnTeam2_1;
            respawnJ4 = modo2v2spawnTeam2_2;

            //Obtener datos
            Jugador j1 = new Jugador(0, PlayerPrefs.GetInt("P1_GID"), PlayerPrefs.GetString("P1_Personaje"), 1);
            Jugador j2 = new Jugador(1, PlayerPrefs.GetInt("P2_GID"), PlayerPrefs.GetString("P2_Personaje"), 1);
            Jugador j3 = new Jugador(2, PlayerPrefs.GetInt("P3_GID"), PlayerPrefs.GetString("P3_Personaje"), 2);
            Jugador j4 = new Jugador(3, PlayerPrefs.GetInt("P4_GID"), PlayerPrefs.GetString("P4_Personaje"), 2);

            //Instanciar y Guardar
            equipos = new PlayerController[,]
            {
                {InstanciarJugadorProfe(j1,respawnJ1), InstanciarJugadorProfe(j2,respawnJ2)},
                {InstanciarJugadorProfe(j3,respawnJ3), InstanciarJugadorProfe(j4,respawnJ4)},
            };
        }

    }
    */

    /*
        public void IniciarPartida()
        {

            if (infoLobbyPlayers == null)
                InicializarJugadores();

            slotsHUD[0].gameObject.SetActive(false);
            slotsHUD[1].gameObject.SetActive(false);
            slotsHUD[2].gameObject.SetActive(false);
            slotsHUD[3].gameObject.SetActive(false);

            if (infoLobbyPlayers.Count == 2)
            {
                int equipoJ1 = infoLobbyPlayers[0].equipoJugador;
                int equipoJ2 = infoLobbyPlayers[1].equipoJugador;

                Transform spawn1v1J1;
                Transform spawn1v1J2;

                if (equipoJ1 == 1)
                {
                    spawn1v1J1 = modo1v1spawnTeam1;
                }
                else
                {
                    spawn1v1J1 = modo1v1spawnTeam2;
                }

                if (equipoJ2 == 1)
                {
                    spawn1v1J2 = modo1v1spawnTeam1;
                }
                else
                {
                    spawn1v1J2 = modo1v1spawnTeam2;
                }

                respawnJ1 = spawn1v1J1;
                respawnJ2 = spawn1v1J2;

                slotsHUD[0].gameObject.SetActive(true);
                slotsHUD[1].gameObject.SetActive(true);

                p1 = SpawnJugador(infoLobbyPlayers[0].personaje, spawn1v1J1, infoLobbyPlayers[0].gamepadId);
                p1.equipoJugador = equipoJ1;
                p1.AsignarSlot(slotsHUD[0]);
                p1.playerHUD.Name = infoLobbyPlayers[0].personaje;
                p1.gameObject.name = infoLobbyPlayers[0].personaje;
                activePlayers.Add(p1);

                p2 = SpawnJugador(infoLobbyPlayers[1].personaje, spawn1v1J2, infoLobbyPlayers[1].gamepadId);
                p2.equipoJugador = equipoJ2;
                p2.AsignarSlot(slotsHUD[1]);
                p2.playerHUD.Name = infoLobbyPlayers[1].personaje;
                p2.gameObject.name = infoLobbyPlayers[1].personaje;
                activePlayers.Add(p2);

                foreach(PlayerController player in activePlayers)
                {
                    player.DeshabilitarMovimiento();
                }

                Invoke("HabilitarMovimientoInicialJugadores", 2f);
            }
            else if (infoLobbyPlayers.Count == 4)
            {
                p1.equipoJugador = infoLobbyPlayers[0].equipoJugador;
                p2.equipoJugador = infoLobbyPlayers[1].equipoJugador;
                p3.equipoJugador = infoLobbyPlayers[2].equipoJugador;
                p4.equipoJugador = infoLobbyPlayers[3].equipoJugador;

                activePlayers.Add(p1);
                activePlayers.Add(p2);
                activePlayers.Add(p3);
                activePlayers.Add(p4);

                foreach (PlayerController player in activePlayers)
                {
                    if (player.equipoJugador == 1)
                    {
                        equipo1.Add(player);
                    }
                    else
                    {
                        equipo2.Add(player);
                    }
                }

                //spawn izq
                equipo1[0].transform.position = respawnJ1.position;
                equipo1[1].transform.position = respawnJ2.position;

                //spawns der
                equipo2[0].transform.position = respawnJ3.position;
                equipo2[1].transform.position = respawnJ4.position;

                respawnJ1 = modo2v2spawnTeam1_1;
                respawnJ2 = modo2v2spawnTeam1_2;
                respawnJ3 = modo2v2spawnTeam2_1;
                respawnJ4 = modo2v2spawnTeam2_2;

                slotsHUD[0].gameObject.SetActive(true);
                slotsHUD[1].gameObject.SetActive(true);
                slotsHUD[2].gameObject.SetActive(true);
                slotsHUD[3].gameObject.SetActive(true);

                //aqui me atore xd
                p1 = SpawnJugador(equipo1[0].name, respawnJ1, infoLobbyPlayers[0].gamepadId);
                p1.AsignarSlot(slotsHUD[0]);
                p1.playerHUD.Name = infoLobbyPlayers[0].personaje;

                p2 = SpawnJugador(infoLobbyPlayers[1].personaje, modo2v2spawnTeam1_2, infoLobbyPlayers[1].gamepadId);
                p2.AsignarSlot(slotsHUD[1]);
                p2.playerHUD.Name = infoLobbyPlayers[1].personaje;
                p2.BloquearMovimiento = false;

                p3 = SpawnJugador(infoLobbyPlayers[2].personaje, modo2v2spawnTeam2_1, infoLobbyPlayers[2].gamepadId);
                p3.AsignarSlot(slotsHUD[2]);
                p3.playerHUD.Name = infoLobbyPlayers[2].personaje;
                p3.BloquearMovimiento = false;

                p4 = SpawnJugador(infoLobbyPlayers[3].personaje, modo2v2spawnTeam2_2, infoLobbyPlayers[3].gamepadId);
                p4.AsignarSlot(slotsHUD[3]);
                p4.playerHUD.Name = infoLobbyPlayers[3].personaje;
                p4.BloquearMovimiento = false;

                foreach (PlayerController player in activePlayers)
                {
                    player.DeshabilitarMovimiento();
                }

                Invoke("HabilitarMovimientoInicialJugadores", 2f);
            }

            //InicializarBodyJugadores();
        }*/


    void HabilitarMovimientoJugadores()
    {
        if (puntosAGanarTeam1 == puntosParaGanar || puntosAGanarTeam2 == puntosParaGanar) return;

        foreach (PlayerController player in activePlayers)
        {
            player.HabilitarMovimiento();
        }
    }

    void DeshabilitarMovimientoJugadores()
    {
        foreach (PlayerController player in activePlayers)
        {
            player.DeshabilitarMovimiento();
        }
    }

    void HabilitarDisparo()
    {
        if (puntosAGanarTeam1 == puntosParaGanar || puntosAGanarTeam2 == puntosParaGanar) return;

        foreach (PlayerController player in activePlayers)
        {
            player.muerto = false;
        }
    }

    void DeshabilitarDisparo()
    {
        foreach (PlayerController player in activePlayers)
        {
            player.muerto = true;
        }
    }

    private void InicializarJugadores()
    {
        infoLobbyPlayers = new List<InfoLobby.PlayerInfo>();
    }


    #endregion PARTIDA

    #region MAPAS
    //[Header("Mapas")]

    //[Header("Modo Hechizos Sazonados")]
    //[SerializeField] private GameObject mapaStreetMHS;
    //[SerializeField] private GameObject mapaRestaurantMHS;
    //[SerializeField] private GameObject mapaDungeonMHS;

    //[Header("Modo Duelo de Salsas")]
    //[SerializeField] private GameObject mapaStreetMDS;
    //[SerializeField] private GameObject mapaRestaurantMDS;
    //[SerializeField] private GameObject mapaDungeonMDS;

    //void InicializarMapas()
    //{
    //    if (boolMapaStreetMHS)
    //    {
    //        mapaStreetMHS.SetActive(true);
    //    }
    //    else if (boolMapaDungeonMHS)
    //    {
    //        mapaDungeonMHS.SetActive(true);
    //    }
    //    else if (boolMapaRestaurantMHS)
    //    {
    //        mapaRestaurantMHS.SetActive(true);
    //    }
    //    else if (boolMapaStreetMDS)
    //    {
    //        mapaStreetMDS.SetActive(true);
    //    }
    //    else if (boolMapaRestaurantMDS)
    //    {
    //        mapaRestaurantMDS.SetActive(true);
    //    }
    //    else if (boolMapaDungeonMDS)
    //    {
    //        mapaDungeonMDS.SetActive(true);
    //    }
    //}
    #endregion MAPAS

    #region PAUSA
    [Header("Pausa Core")]

    [Header("Panel Pausa")]
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelControles;
    [SerializeField] private GameObject panelConfirmacionSalida;
    [SerializeField] private Button botonContinuar;
    [SerializeField] private Button botonConfiguracion;
    [SerializeField] private Button botonControles;
    [SerializeField] private Button botonSalir;
    [SerializeField] private static Button botonContinuarStatic;
    [SerializeField] private static GameObject panelStaticPausa;

    public static GameObject ultimoBotonSeleccionadoStatic;
    private GameObject ultimoBotonSeleccionado;

    private static bool enPausa;
    public static bool EnPausa => enPausa;

    void InicializarPausa()
    {
        panelStaticPausa = panelPausa;
        botonContinuarStatic = botonContinuar;
        ultimoBotonSeleccionadoStatic = botonContinuarStatic.gameObject;
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
        InputManager.Instance.SetActivePanel(null, Back);
    }

    public static void Pausa(PlayerController player)
    {
        //// Bloquear si la pausa está activa y el jugador que intenta pausar no es el `activePlayerController`
        //if (enPausa && activePlayerController != player)
        //    return;

        AudioManager.instance.PlaySound("botonmenu");

        enPausa = !enPausa;

        if (enPausa)
        {
            //// Guardar el controlador que activó la pausa
            //activePlayerController = player;

            // Pausar el tiempo y activar el panel de pausa
            Time.timeScale = 0;
            panelStaticPausa.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonContinuarStatic.gameObject);

            ultimoBotonSeleccionadoStatic = botonContinuarStatic.gameObject;

            //// Asignar el control de navegación solo al Gamepad del jugador que activó la pausa
            //InputManager.Instance.SetExclusiveGamepad(activePlayerController._gamepad);

            // Cambiar el esquema de todos los jugadores a "UI Pausa"
            foreach (PlayerController j in activePlayers)
            {
                j.DeshabilitarMovimiento();
                //j.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI Pausa");
                //Debug.Log("Switched to UI Pausa for player: " + j.name);
            }

        }
        else
        {
            // Reanudar el tiempo y cerrar el panel de pausa
            Time.timeScale = 1;
            panelStaticPausa.SetActive(false);

            //// Limpiar la exclusividad del Gamepad en InputManager
            //InputManager.Instance.ClearExclusiveGamepad();
            //Debug.Log("Removing exclusivity");

            // Restaurar el esquema de control de todos los jugadores a "Player"
            foreach (PlayerController j in activePlayers)
            {
                j.HabilitarMovimiento();
                //j.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                //Debug.Log("Switched to Player for player: " + j.name);
            }

        }
    }

    public void Resumir()
    {
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
        AudioManager.instance.PlaySound("botonmenu");
        //Pausa(null);
        //enPausa = false;
        // Llamamos a Pausa con la referencia al controlador original
        Pausa(null);
    }


    public void Controles()
    {
        panelControles.SetActive(true);
        panelConfirmacionSalida.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonControles.gameObject);
        ultimoBotonSeleccionadoStatic = botonControles.gameObject;
        AudioManager.instance.PlaySound("botonmenu");
    }

    public void Configuracion()
    {
        if (ConfiguracionManager.Instance != null)
        {
            InputManager.Instance.SetActivePanel(null, null);
            ConfiguracionManager.Instance.PrenderConfiguracion();
            ConfiguracionManager.Instance.ResetarInputCM();
            panelControles.SetActive(false);
            panelConfirmacionSalida.SetActive(false);
            AudioManager.instance.PlaySound("botonmenu");
            ultimoBotonSeleccionadoStatic = botonConfiguracion.gameObject;
        }
    }

    public void GoToMenuVictory()
    {
        Time.timeScale = 1;
        deadEnemy = true;
        DestruirEnemigosActivos();
        MainMenuSystem.instance.ResetarBooleanosImportantesMS();
        ResetarBooleanosImportantesGM();
        AudioManager.instance.PlaySound("botonmenu");

        SceneManager.LoadScene("ANDYMENUTEST");
    }

    public void ConfirmarSalida()
    {
        panelConfirmacionSalida.SetActive(true);
        panelControles.SetActive(false);

        InputManager.Instance.SetActivePanel(Salir, Back);
    }

    public void Salir()
    {
        Debug.Log("QUIT");
        AudioListener.volume = 0f;
        Application.Quit();
    }

    public void Back()
    {
        if (panelConfirmacionSalida.activeSelf)
        {
            panelConfirmacionSalida.SetActive(false);
            EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
            AudioManager.instance.PlaySound("botonBack");
        }
        else if (panelControles.activeSelf)
        {
            panelControles.SetActive(false);
            EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
            AudioManager.instance.PlaySound("botonBack");
        }
        else if (panelStaticPausa.activeSelf)
        {
            Resumir();
        }
    }

    public void Aceptar()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected == botonContinuar)
        {
            botonContinuar.onClick.Invoke();
        }
        else if (currentSelected == botonConfiguracion)
        {
            botonConfiguracion.onClick.Invoke();
        }
    }

    public void ResetearInputGM()
    {
        InputManager.Instance.SetActivePanel(Aceptar, Back);
    }
    #endregion PAUSA

    #region VICTORIA

    private bool equipo1Ganado = false;
    private bool equipo2Ganado = false;

    void ProcesarVictoriaEquipo(int equipoGanador)
    {
        deadEnemy = true;
        DestruirEnemigosActivos();
        DeshabilitarMovimientoJugadores();
        DetenerMusica();

        if (modoHS)
        {
            AudioManager.instance.PlaySound("edHS");
        }
        else
        {
            AudioManager.instance.PlaySound("edDS");
        }

        // Realizar acciones específicas para el equipo ganador
        Invoke("AbrirPanelFinish", 4f);
        Invoke("IniciarCorutinaTransicion", 7f);

        if (equipoGanador == 1)
            Invoke("EnviarTeam1", 8f);
        else if (equipoGanador == 2)
            Invoke("EnviarTeam2", 8f);
    }

    private void EnviarTeam1()
    {
        GuardarJugadoresPorEquipo(1); // Guarda el equipo 1 como ganador
        Debug.Log("Se envio en team 1");
    }

    private void EnviarTeam2()
    {
        GuardarJugadoresPorEquipo(2); // Guarda el equipo 2 como ganador
        Debug.Log("Se envio en team 2");
    }

    private void GuardarJugadoresPorEquipo(int equipoGanador)
    {
        // Guardar el equipo ganador en PlayerPrefs
        PlayerPrefs.SetInt("EquipoGanador", equipoGanador);

        // Guardar jugadores de cada equipo
        for (int i = 0; i < activePlayers.Count; i++)
        {
            var player = activePlayers[i];
            if (player == null) continue;

            PlayerPrefs.SetInt("Jugador_" + i + "_Equipo", activePlayers[i].equipo);
            PlayerPrefs.SetString("Jugador_" + i + "_Personaje", activePlayers[i].name);
        }

        PlayerPrefs.SetInt("TotalJugadores", activePlayers.Count); // Guarda el total de jugadores
        PlayerPrefs.Save();

        // Cargar la escena de victoria
        Debug.Log("Se cargo escena de Victoria");
        Time.timeScale = 1;
        SceneManager.LoadScene("TESTVICTORY2");
    }
    #endregion VICTORIA

    #region TRANSICION INICIO PARTIDA

    [Header("Transicion Inicio Partida Core")]
    [SerializeField] public GameObject panelTransicionInicioPartida;
    [SerializeField] private float timeTransitionInicioPartida = 0.5f;
    private Animator animTransitionInicioPartida;

    void InicializarTransicioninicioPartida()
    {
        animTransitionInicioPartida = panelTransicionInicioPartida.GetComponent<Animator>();
    }

    public IEnumerator TransicionInicioPartida()
    {
        yield return new WaitForSeconds(3f);
        animTransitionInicioPartida.SetTrigger("ready");
        //Audio Ready
        yield return new WaitForSeconds(timeTransition);
        animTransitionInicioPartida.SetTrigger("go");
        //Adio Go
        yield return new WaitForSeconds(1f);
        PrepararEscenaDeJuego();
        HabilitarMovimientoJugadores();
        HabilitarDisparo();
    }

    #endregion TRANSICION INICIO PARTIDA

    #region TRANSICION ESCENA
    [Header("Transicion Core")]
    [SerializeField] public GameObject panelTransicion;
    [SerializeField] private float timeTransition = 1f;
    private Animator animTransition;

    void InicializarTransicion()
    {
        animTransition = panelTransicion.GetComponent<Animator>();
    }

    public IEnumerator Transicion()
    {
        yield return new WaitForSeconds(0.5f);
        animTransition.SetTrigger("fadeIn");
        yield return new WaitForSeconds(timeTransition);
        animTransition.SetTrigger("fadeOut");
    }
    #endregion TRANSICION ESCENA

    #region PUNTAJE
    [Header("Puntaje Core")]
    [SerializeField] private GameObject panelFinish;
    [SerializeField] private int puntosAGanarTeam1;
    [SerializeField] private int puntosAGanarTeam2;
    [SerializeField] public int puntosParaGanar;
    private int puntajeInicial = 0;
    private Animator finishAnimator;

    [Header("Puntaje MHS")]
    [SerializeField] private TMP_Text puntajeTeam1MHS;
    [SerializeField] private TMP_Text puntajeTeam2MHS;

    [Header("Puntaje MDS")]
    [SerializeField] private TMP_Text puntajeTeam1MDS;
    [SerializeField] private TMP_Text puntajeTeam2MDS;
    void InicializarPuntaje()
    {
        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();

        puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
        puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();

        puntosAGanarTeam1 = puntajeInicial;
        puntosAGanarTeam2 = puntajeInicial;

        //finishAnimator = panelFinish.GetComponent<Animator>();
    }
    #endregion PUNTAJE

    #region MARCADOR MHS
    [Header("Marcador Modo HS")]
    [SerializeField] private GameObject panelMarcadorMHS;
    [SerializeField] private TMP_Text rondaText;
    [SerializeField] private int numeroDeRonda;
    private int rondaInicial = 1;

    void InicializarMarcadorMHS()
    {
        panelFinish.SetActive(false);
        panelMarcadorMHS.SetActive(true);
        numeroDeRonda = rondaInicial;
        rondaText.text = numeroDeRonda.ToString();

        //AudioManager.instance.PlaySound("introHS");
    }

    void Update_Marcador_MHS()
    {
        if (inGame && modoHS)
        {
            if (nJugadores == 2)
            {
                if (!equipo1Ganado && puntosAGanarTeam1 >= puntosParaGanar)
                {
                    equipo1Ganado = true; // Aseguramos que esta condición se ejecute una sola vez
                    ProcesarVictoriaEquipo(1);
                    AudioManager.instance.PlaySound("finishHS");
                }
                else if (!equipo2Ganado && puntosAGanarTeam2 >= puntosParaGanar)
                {
                    equipo2Ganado = true; // Aseguramos que esta condición se ejecute una sola vez
                    ProcesarVictoriaEquipo(2);
                    AudioManager.instance.PlaySound("finishHS");
                }
            }
            else
            {
                if (!equipo1Ganado && puntosAGanarTeam1 >= puntosParaGanar)
                {
                    equipo1Ganado = true;
                    ProcesarVictoriaEquipo(1);
                    AudioManager.instance.PlaySound("finishHS");
                }
                else if (!equipo2Ganado && puntosAGanarTeam2 >= puntosParaGanar)
                {
                    equipo2Ganado = true;
                    ProcesarVictoriaEquipo(2);
                    AudioManager.instance.PlaySound("finishHS");
                }
            }
        }

    }

    void AbrirPanelFinish()
    {
        panelFinish.SetActive(true);
        DeshabilitarMovimientoJugadores();
        DeshabilitarDisparo();
    }

    public void IniciarCorutinaTransicion()
    {
        StartCoroutine(Transicion());
    }

    void DetenerMusica()
    {
        AudioManager.instance.StopSound("hechizos");
        AudioManager.instance.StopSound("duelo");
    }

    void DetenerTiempo()
    {
        Time.timeScale = 0;
    }
    #endregion MARCADOR MHS

    #region MARCADOR MDS
    [Header("Marcador Modo DS")]
    [SerializeField] private GameObject panelMarcadorMDS;

    void InicializarMarcadorMDS()
    {
        panelMarcadorMDS.SetActive(true);
    }
    #endregion MARCADOR MDS

    #region TEMPORIZADOR
    [Header("Temporizador")]
    [SerializeField] private GameObject panelTemporizador;
    [SerializeField] private GameObject panelTiempoAgotado;
    [SerializeField] public float totalTime = 60f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text TiempoAgotadoText;

    public float remainingTime;
    private bool isRunning = false;
    private bool faltan15Seg = false;

    private void InicializarTemporizador()
    {
        panelTemporizador.SetActive(true);
        panelTiempoAgotado.SetActive(false);

        remainingTime = totalTime;
        InicializarTimerText();


        AudioManager.instance.PlaySound("introDS");
    }

    private void FixUpdate_Temporizador()
    {
        if (inGame && modoDS && isRunning)
        {
            //timerText.text = remainingTime.ToString();
            remainingTime -= Time.fixedDeltaTime;

            InicializarTimerText();

            if (remainingTime <= 0)
            {
                isRunning = false;
                TimerEnded();
                TiempoAgotadoMDS();
                AudioManager.instance.PlaySound("finishDS");
            }

            if (remainingTime <= 15f && !faltan15Seg)
            {
                AudioManager.instance.PlaySound("midDS");
                Debug.Log("¡Quedan 15 segundos!");
                faltan15Seg = true;
            }

        }
        else return;
    }

    private void TimerEnded()
    {
        timerText.text = "00:00";
    }

    private void InicializarTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion TEMPORIZADOR

    #region PERSONAJES
    [Header("Personajes")]
    [SerializeField] private PlayerController pfCRIM;
    [SerializeField] private PlayerController pfKAI;
    [SerializeField] private PlayerController pfNOVA;
    [SerializeField] private PlayerController pfSKYIE;
    #endregion PERSONAJES

    #region JUGADORES

    [Header("Main Spawn Points")]
    [SerializeField] private Transform modo1v1spawnTeam1;
    [SerializeField] private Transform modo1v1spawnTeam2;
    [SerializeField] private Transform modo2v2spawnTeam1_1;
    [SerializeField] private Transform modo2v2spawnTeam1_2;
    [SerializeField] private Transform modo2v2spawnTeam2_1;
    [SerializeField] private Transform modo2v2spawnTeam2_2;

    //private GameObject bodyP1;
    //private GameObject bodyP2;
    //private GameObject bodyP3;
    //private GameObject bodyP4;

    private static PlayerController p1, p2, p3, p4;
    /*
    private void InicializarBodyJugadores()
    {
        if (infoLobbyPlayers.Count == 2)
        {
            bodyP1 = p1.transform.GetChild(0).gameObject;
            bodyP2 = p2.transform.GetChild(0).gameObject;
        }
        else if (infoLobbyPlayers.Count == 4)
        {
            bodyP1 = p1.transform.GetChild(0).gameObject;
            bodyP2 = p2.transform.GetChild(0).gameObject;
            bodyP3 = p3.transform.GetChild(0).gameObject;
            bodyP4 = p4.transform.GetChild(0).gameObject;
        }

    }
    */
    private PlayerController SpawnJugador(string personaje, Transform spawnPoint, int gamepadId)
    {
        PlayerController prefabPersonaje = null;

        switch (personaje)
        {
            case "CRIM": prefabPersonaje = pfCRIM; break;
            case "KAI": prefabPersonaje = pfKAI; break;
            case "NOVA": prefabPersonaje = pfNOVA; break;
            case "SKYIE": prefabPersonaje = pfSKYIE; break;
            default:
                Debug.LogError("Personaje no reconocido: " + personaje);
                break;
        }

        if (prefabPersonaje != null)
        {
            PlayerController playerController = Instantiate(prefabPersonaje, spawnPoint.position, spawnPoint.rotation);
            //playerController.gamepadIndex = LobbyManager.self.GetGamepadById(gamepadId); // Asignar el Gamepad
            return playerController;
        }

        return null;
    }
    /*
    public PlayerController InstanciarJugadorProfe(Jugador jugador, Transform spawnPoint)
    {
        PlayerController prefabPersonaje = null;

        switch (jugador.personaje)
        {
            case "CRIM": prefabPersonaje = pfCRIM; break;
            case "KAI": prefabPersonaje = pfKAI; break;
            case "NOVA": prefabPersonaje = pfNOVA; break;
            case "SKYIE": prefabPersonaje = pfSKYIE; break;
        }

        print("SE INSTANCIA: " + jugador.personaje);
        PlayerController pc = Instantiate(prefabPersonaje, spawnPoint.position, spawnPoint.rotation);
        Gamepad gamepad = LobbyManager.self.GetGamepadById(jugador.gamepadId); // Asignar el Gamepad

        if (gamepad != null)
        {
            pc.SetGamepad(gamepad);  // Asignar el Gamepad
        }
        else
        {
            Debug.LogError($"No se encontró un Gamepad con el ID {jugador.gamepadId} para el jugador {jugador.personaje}");
        }

        pc.PlayerHUD = slotsHUD[jugador.indice];
        pc.Jugador = jugador;
        pc.equipo = jugador.equipo;
        activePlayers.Add(pc);
        return pc;
    }

    #endregion JUGADORES
    */

    public PlayerController InstanciarJugadorProfe(Jugador jugador, Transform spawnPoint)
    {
        PlayerController prefabPersonaje = ObtenerPrefabPersonaje(jugador.personaje);
        PlayerController pc = Instantiate(prefabPersonaje, spawnPoint.position, spawnPoint.rotation);
        print("SE INSTANCIA: " + jugador.personaje);

        // Configurar HUD y equipo del jugador
        pc.PlayerHUD = slotsHUD[jugador.indice];
        pc.Jugador = jugador;  // Esto activa automáticamente la asignación del Gamepad
        pc.equipo = jugador.equipo;
        pc.gameObject.name = jugador.personaje;
        jugador.controlador = pc;

        // Asegurarse de que el PlayerInput esté configurado de forma aislada
        pc.GetComponent<PlayerInput>().user.UnpairDevices();
        pc.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller", new[] { pc._gamepad });

        activePlayers.Add(pc);
        return pc;
    }

    private PlayerController ObtenerPrefabPersonaje(string personaje)
    {
        switch (personaje)
        {
            case "CRIM": return pfCRIM;
            case "KAI": return pfKAI;
            case "NOVA": return pfNOVA;
            case "SKYIE": return pfSKYIE;
            default: return null;
        }
    }
    #endregion JUGADORES

    #region SPAWN POSITIONS
    [Header("Spawns Players for Map")]

    [Header("Spawns Street MHS")]
    [SerializeField] private Transform[] streetMHS;
    [Header("Spawns Restaurant MHS")]
    [SerializeField] private Transform[] restaurantMHS;
    [Header("Spawns Dungueon MHS")]
    [SerializeField] private Transform[] dungueonMHS;
    [Header("Spawns Street MDS")]
    [SerializeField] private Transform[] streetMDS;
    [Header("Spawns Restaurant MDS")]
    [SerializeField] private Transform[] restaurantMDS;
    [Header("Spawns Dungueon MDS")]
    [SerializeField] private Transform[] dungueonMDS;

    [Header("Spawns Enemies for Map")]
    [SerializeField] private Transform[] EnemyStreetMHS;
    [SerializeField] private Transform[] EnemyRestaurantMHS;
    [SerializeField] private Transform[] EnemyDungeonMHS;
    [SerializeField] private Transform[] EnemyStreetMDS;
    [SerializeField] private Transform[] EnemyRestaurantMDS;
    [SerializeField] private Transform[] EnemyDungeonMDS;
    private void InicializarSpawnsPoints()
    {
        if (boolMapaStreetMHS)
        {
            modo1v1spawnTeam1.position = streetMHS[0].position;
            modo1v1spawnTeam2.position = streetMHS[1].position;
            modo2v2spawnTeam1_1.position = streetMHS[2].position;
            modo2v2spawnTeam1_2.position = streetMHS[3].position;
            modo2v2spawnTeam2_1.position = streetMHS[4].position;
            modo2v2spawnTeam2_2.position = streetMHS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyStreetMHS[i].position;
            }
        }
        else if (boolMapaRestaurantMHS)
        {
            modo1v1spawnTeam1.position = restaurantMHS[0].position;
            modo1v1spawnTeam2.position = restaurantMHS[1].position;
            modo2v2spawnTeam1_1.position = restaurantMHS[2].position;
            modo2v2spawnTeam1_2.position = restaurantMHS[3].position;
            modo2v2spawnTeam2_1.position = restaurantMHS[4].position;
            modo2v2spawnTeam2_2.position = restaurantMHS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyRestaurantMHS[i].position;
            }
        }
        else if (boolMapaDungeonMHS)
        {
            modo1v1spawnTeam1.position = dungueonMHS[0].position;
            modo1v1spawnTeam2.position = dungueonMHS[1].position;
            modo2v2spawnTeam1_1.position = dungueonMHS[2].position;
            modo2v2spawnTeam1_2.position = dungueonMHS[3].position;
            modo2v2spawnTeam2_1.position = dungueonMHS[4].position;
            modo2v2spawnTeam2_2.position = dungueonMHS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyDungeonMHS[i].position;
            }
        }
        else if (boolMapaStreetMDS)
        {
            modo1v1spawnTeam1.position = streetMDS[0].position;
            modo1v1spawnTeam2.position = streetMDS[1].position;
            modo2v2spawnTeam1_1.position = streetMDS[2].position;
            modo2v2spawnTeam1_2.position = streetMDS[3].position;
            modo2v2spawnTeam2_1.position = streetMDS[4].position;
            modo2v2spawnTeam2_2.position = streetMDS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyStreetMDS[i].position;
            }
        }
        else if (boolMapaRestaurantMDS)
        {
            modo1v1spawnTeam1.position = restaurantMDS[0].position;
            modo1v1spawnTeam2.position = restaurantMDS[1].position;
            modo2v2spawnTeam1_1.position = restaurantMDS[2].position;
            modo2v2spawnTeam1_2.position = restaurantMDS[3].position;
            modo2v2spawnTeam2_1.position = restaurantMDS[4].position;
            modo2v2spawnTeam2_2.position = restaurantMDS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyRestaurantMDS[i].position;
            }
        }
        else if (boolMapaDungeonMDS)
        {
            modo1v1spawnTeam1.position = dungueonMDS[0].position;
            modo1v1spawnTeam2.position = dungueonMDS[1].position;
            modo2v2spawnTeam1_1.position = dungueonMDS[2].position;
            modo2v2spawnTeam1_2.position = dungueonMDS[3].position;
            modo2v2spawnTeam2_1.position = dungueonMDS[4].position;
            modo2v2spawnTeam2_2.position = dungueonMDS[5].position;

            for (int i = 0; i < spawnPointsEnemies.Length; i++)
            {
                spawnPointsEnemies[i].position = EnemyDungeonMDS[i].position;
            }
        }
    }
    #endregion SPAWN POSITIONS

    #region CAMARA
    [Header("Camara Principal De Juego")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private GameObject camaraObjeto;

    [Header("Shake Settings")]
    [SerializeField] private Transform shakePivot; // Objeto hijo para el efecto de "shake"
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    private Vector3 originalShakePosition;

    void InicializarCamara()
    {
        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);

        if (shakePivot != null)
        {
            originalShakePosition = shakePivot.localPosition;
        }
    }

    private void IniciarShake()
    {
        if (shakePivot != null)
        {
            StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
        }
        else
        {
            Debug.LogWarning("ShakePivot no está asignado.");
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            // Aplicar el desplazamiento al ShakePivot
            shakePivot.localPosition = new Vector3(
                originalShakePosition.x + offsetX,
                originalShakePosition.y + offsetY,
                originalShakePosition.z
            );

            elapsed += Time.deltaTime;

            Debug.Log("Se agitó la cámara");
            yield return null;
        }

        // Restaurar la posición original
        shakePivot.localPosition = originalShakePosition;
    }
    #endregion CAMARA

    #region MODO DUELO DE SALSAS
    //[Header("Modo Duelo De Salsas")]

    //[Header("Pista")]
    //[SerializeField] private GameObject pistaPintable;
    //[SerializeField] private List<CuadroPintable> cuadrosPintables;
    //BORRAR LISTAS
    private List<CuadroPintable> cuadrosTeam1;
    private List<CuadroPintable> cuadrosTeam2;

    private static int _cuadrosEquipo1 = 0;
    private static int _cuadrosEquipo2 = 0;

    public static int CuadrosEquipo1
    {
        get => _cuadrosEquipo1;
        set
        {
            _cuadrosEquipo1 = value;
            Instance.puntajeTeam1MDS.text = value.ToString();
        }
    }

    public static int CuadrosEquipo2
    {
        get => _cuadrosEquipo2;
        set
        {
            _cuadrosEquipo2 = value;
            Instance.puntajeTeam2MDS.text = value.ToString();
        }
    }

    void InicializarMDS()
    {
        //Reiniciamos los contadores
        CuadrosEquipo1 = 0;
        CuadrosEquipo2 = 0;

        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);
        panelMarcadorMDS.SetActive(true);
        //cuadrosTeam1 = new List<CuadroPintable>();
        //cuadrosTeam2 = new List<CuadroPintable>();
    }

    public static void CuadradoCambiado(CuadroPintable cuadro)
    {
        //Equipo 1
        if (cuadro.equipoActual == 1)
        {
            //Aumentamos el puntaje
            CuadrosEquipo1++;

            //Si el cuadro ya estaba pintado por otro equipoJugador, le restamos al otro equipoJugador
            if (cuadro.pintado) CuadrosEquipo2--;
        }
        //Equipo 2
        else
        {
            //Aumentamos el puntaje
            CuadrosEquipo2++;

            //Si el cuadro ya estaba pintado por otro equipoJugador, le restamos al otro equipoJugador
            if (cuadro.pintado) CuadrosEquipo1--;
        }

    }

    //Metodo para verifica que lista tiene mas cuadros pintados cuanso acabe el tiempo
    private void TiempoAgotadoMDS()
    {
        int team1Count = _cuadrosEquipo1;
        int team2Count = _cuadrosEquipo2;

        if (nJugadores == 2)
        {
            deadEnemy = true;
            DestruirEnemigosActivos();

            if (!equipo1Ganado && team1Count > team2Count)
            {
                equipo1Ganado = true;
                ProcesarVictoriaEquipo(1);
            }
            else if (!equipo2Ganado && team2Count > team1Count)
            {
                equipo1Ganado = true;
                ProcesarVictoriaEquipo(2);
            }
            else if (team1Count == 0 || team2Count == 0 || team1Count == team2Count)
            {
                ManejarEmpate();
            }
        }

        else if (nJugadores == 4)
        {
            deadEnemy = true;
            DestruirEnemigosActivos();

            if (!equipo1Ganado && team1Count > team2Count)
            {
                equipo1Ganado = true;
                ProcesarVictoriaEquipo(1);
            }
            else if (!equipo2Ganado && team2Count > team1Count)
            {
                equipo1Ganado = true;
                ProcesarVictoriaEquipo(2);
            }
            else if (team1Count == 0 || team2Count == 0 || team1Count == team2Count)
            {
                ManejarEmpate();
            }
        }
    }

    private void ManejarEmpate()
    {
        panelTiempoAgotado.SetActive(true);
        TiempoAgotadoText.text = "¡EMPATE!";

        DeshabilitarMovimientoJugadores();
        Invoke("IniciarCorutinaTransicion", 0.7f);
        Invoke("RegresarAMenuTiempoAgotado", 1.4f);
    }

    void RegresarAMenuTiempoAgotado()
    {
        SceneManager.LoadScene("ANDYMENUTEST");
    }
    #endregion MODO DUELO DE SALSAS

    #region MODO HECHIZOS SAZONADOS
    [Header("Modo Hechizos Sazonados")]
    [SerializeField] private GameObject magosPrincipales;
    [SerializeField] private GameObject vfxImpactoHechizo;
    [SerializeField] private ParticleSystem vfxCargaMago1;
    [SerializeField] private ParticleSystem vfxCargaMago2;
    [SerializeField] private ParticleSystem vfxImactoRocaMago1;
    [SerializeField] private ParticleSystem vfxImactoRocaMago2;

    [Header("Magos")]
    [SerializeField] private GameObject mago1;
    [SerializeField] private GameObject mago2;

    [Header("Camara Stats MH")]
    [SerializeField] private Transform posicionCinematica;
    [SerializeField] private float velocidadMovCamara = 2f;
    private Animator camaraPrincipalAnimator;


    [Header("Hechizo")]
    [SerializeField] private GameObject hechizoPrefab;
    [SerializeField] private float hechizoVelocidad = 10f;
    [SerializeField] private Transform spawnHechizo1;
    [SerializeField] private Transform spawnHechizo2;

    void InicializarMHS()
    {
        panelMarcadorMHS.SetActive(true);
        camaraPrincipalAnimator = camaraPrincipal.GetComponent<Animator>();
        vfxCargaMago1.Stop();
        vfxCargaMago2.Stop();
        vfxImactoRocaMago1.Stop();
        vfxImactoRocaMago2.Stop();
    }

    //Parte de los Magos
    void Mago1()
    {
        Debug.Log("Si se intancio el hechizo del mago 1");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo1.transform.position, Quaternion.identity);
        StartCoroutine(MoverHechizo1(hechizo));
        vfxCargaMago1.Stop();
        AudioManager.instance.PlaySound("hechizoMago");
    }

    void Mago2()
    {
        Debug.Log("Si se intancio el hechizo del mago 2");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo2.transform.position, Quaternion.identity);
        StartCoroutine(MoverHechizo2(hechizo));
        vfxCargaMago2.Stop();
        AudioManager.instance.PlaySound("hechizoMago");
    }

    IEnumerator MoverHechizo1(GameObject hechizo)
    {
        while (hechizo != null)
        {
            hechizo.transform.position = Vector3.MoveTowards(hechizo.transform.position, mago2.transform.position, 10f * Time.deltaTime);

            if (Vector3.Distance(hechizo.transform.position, mago2.transform.position) < 0.1f)
            {
                GameObject cloneVFX = Instantiate(vfxImpactoHechizo, hechizo.transform.position, Quaternion.identity);
                Destroy(hechizo);
                Destroy(cloneVFX, 1.8f);
                vfxImactoRocaMago2.Play();
                IniciarShake();
                AudioManager.instance.StopSound("hechizoMago");
                AudioManager.instance.PlaySound("hechizoImpacto");
                AudioManager.instance.PlaySound("magoRoca");
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator MoverHechizo2(GameObject hechizo)
    {
        while (hechizo != null)
        {
            hechizo.transform.position = Vector3.MoveTowards(hechizo.transform.position, mago1.transform.position, 10f * Time.deltaTime);

            if (Vector3.Distance(hechizo.transform.position, mago1.transform.position) < 0.1f)
            {
                GameObject cloneVFX = Instantiate(vfxImpactoHechizo, hechizo.transform.position, Quaternion.identity);
                Destroy(hechizo);
                Destroy(cloneVFX, 1.8f);
                vfxImactoRocaMago1.Play();
                IniciarShake();
                AudioManager.instance.StopSound("hechizoMago");
                AudioManager.instance.PlaySound("hechizoImpacto");
                AudioManager.instance.PlaySound("magoRoca");
                yield break;
            }

            yield return null;
        }
    }

    ////Parte de la Camara
    //public void PosicionCamaraCinematica()
    //{
    //    StartCoroutine(MoverCamara(camaraPrincipal.transform, posicionCinematica.position));
    //}

    //IEnumerator MoverCamara(Transform camaraTransform, Vector3 targetPosition)
    //{
    //    while (Vector3.Distance(camaraTransform.position, targetPosition) > 0.1f)
    //    {
    //        camaraTransform.position = Vector3.Lerp(camaraTransform.position, targetPosition, velocidadMovCamara * Time.deltaTime);
    //        yield return null;
    //    }

    //    // Asegurarse de que la posici�n final sea exacta.
    //    camaraTransform.position = targetPosition;

    //    // Regresar la c�mara a su posici�n original
    //    StartCoroutine(RetornarCamara(3.0f));
    //}

    //void PosicionCamaraPrincipal()
    //{
    //    StartCoroutine(MoverCamara(camaraPrincipal.transform, originalCameraPosition));
    //}

    //IEnumerator RetornarCamara(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    PosicionCamaraPrincipal();
    //}
    #endregion MODO HECHIZOS SAZONADOS

    #region DEAD EVENT PLAYER
    //En caso de fallar la muerte, volver a colocar los valores directamente en vez de solo llamar al metodo Revivir de dicho jugador

    //CharacterController chP1;
    //CharacterController chP2;

    void InicializarComponentesJugadores()
    {
        //chP1 = p1.GetComponent<CharacterController>();
        //chP2 = p2.GetComponent<CharacterController>();
    }

    public void DeadPlayerEventMHS(PlayerController player)
    {
        if (modoHS)
        {
            Debug.Log("Si entro al metodo de muerte");

            if (puntosAGanarTeam1 != puntosParaGanar && puntosAGanarTeam2 != puntosParaGanar)
            {
                if (nJugadores == 2)
                {
                    if (player.equipo == 1)
                    {
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        DeshabilitarMovimientoJugadores();
                        DeshabilitarDisparo();
                        vfxCargaMago2.Play();
                        Invoke("Mago2", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                    else if (player.equipo == 2)
                    {
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        DeshabilitarMovimientoJugadores();
                        DeshabilitarDisparo();
                        vfxCargaMago1.Play();
                        Invoke("Mago1", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                }

                else if (nJugadores == 4)
                {
                    if (player.equipo == 1)
                    {
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        DeshabilitarMovimientoJugadores();
                        DeshabilitarDisparo();
                        vfxCargaMago2.Play();
                        Invoke("Mago2", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                    else if (player.equipo == 2)
                    {
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        DeshabilitarMovimientoJugadores();
                        DeshabilitarDisparo();
                        vfxCargaMago1.Play();
                        Invoke("Mago1", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                }


            }
            else
            {
                return;
            }

        }
        else if (modoDS)
        {
            player.DeshabilitarMovimiento();
            StartCoroutine(RespawnearJugadorMDS(player));

        }
        else
        {
            return;
        }

    }


    private void CambioDeRondaMHS()
    {
        // Ajustamos el n�mero de ronda
        numeroDeRonda++;
        rondaText.text = numeroDeRonda.ToString();

        // Mantener un índice para los jugadores en cada equipo
        int indexEquipo1 = 0, indexEquipo2 = 0;

        Debug.Log("CAMBIO DE RONDA: Iniciando reubicación de jugadores");

        if (nJugadores == 2)
        {
            Debug.Log("Modo 1v1: Reubicando jugadores");
            // Posicionar a los jugadores en los respawns correspondientes
            foreach (PlayerController player in activePlayers)
            {
                player.transform.GetChild(0).gameObject.SetActive(false); // Desactivar el cuerpo

                if (player.equipo == 1)
                    player.transform.position = respawnJ1.position; // Respawn del equipo 1
                else if (player.equipo == 2)
                    player.transform.position = respawnJ2.position; // Respawn del equipo 2
            }
        }
        else if (nJugadores == 4)
        {
            Debug.Log("Modo 2v2: Reubicando jugadores");


            foreach (PlayerController player in activePlayers)
            {
                player.transform.GetChild(0).gameObject.SetActive(false); // Desactivar el cuerpo

                // Asignar los respawns en base al equipo y su índice
                if (player.equipo == 1)
                {
                    if (indexEquipo1 == 0)
                        player.transform.position = respawnJ1.position;
                    else if (indexEquipo1 == 1)
                        player.transform.position = respawnJ2.position;

                    indexEquipo1++; // Incrementar índice del equipo 1
                }
                else if (player.equipo == 2)
                {
                    if (indexEquipo2 == 0)
                        player.transform.position = respawnJ3.position;
                    else if (indexEquipo2 == 1)
                        player.transform.position = respawnJ4.position;

                    indexEquipo2++; // Incrementar índice del equipo 2
                }
            }
        }

        foreach (PlayerController player in activePlayers)
        {
            player.ResetearVariablesCambioRonda();
        }

        // Los reactivamos
        StartCoroutine(ReactivacionMHS(2.5f));
    }

    IEnumerator ReactivacionMHS(float time)
    {
        Debug.Log("Reactivando jugadores después del cambio de ronda");
        yield return new WaitForSeconds(2.25f);

        foreach (PlayerController player in activePlayers)
        {
            player.transform.GetChild(0).gameObject.SetActive(true); // Activar el cuerpo
            player.Revivir(); // Llamar al método de revivir
        }

        yield return new WaitForSeconds(1.5f);
        HabilitarMovimientoJugadores();
        HabilitarDisparo();
        yield return new WaitForSeconds(5f);
        InicializarEnemySpawn();
    }

    private IEnumerator RespawnearJugadorMDS(PlayerController player)
    {
        yield return new WaitForSeconds(4f);
        player.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        if (nJugadores == 2)
        {
            // Para dos jugadores, asignar el respawn según el equipo
            if (player.equipo == 1)
            {
                player.transform.position = respawnJ1.position;
            }
            else if (player.equipo == 2)
            {
                player.transform.position = respawnJ2.position;
            }
        }
        else if (nJugadores == 4)
        {
            // Para cuatro jugadores, usar respawns definidos al inicio
            if (player == activePlayers[0])
            {
                player.transform.position = respawnJ1.position;
            }
            else if (player == activePlayers[1])
            {
                player.transform.position = respawnJ2.position;
            }
            else if (player == activePlayers[2])
            {
                player.transform.position = respawnJ3.position;
            }
            else if (player == activePlayers[3])
            {
                player.transform.position = respawnJ4.position;
            }
        }

        yield return new WaitForSeconds(2f);

        // Reactivar al jugador
        player.Revivir();
        player.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        player.HabilitarMovimiento();

        Debug.Log($"Jugador {player.name} revivido y posicionado en su respawn.");
    }
    #endregion DEAD EVENT PLAYER

    #region SPAWN DE ENEMIGOS
    [Header("Spawn Enemy Core")]
    [SerializeField] private GameObject[] prefabsEnemigos;
    [SerializeField] private Transform[] spawnPointsEnemies;
    [SerializeField] private int enemigosMaximosActivos = 4;
    [SerializeField] private float minSpawnTime = 3f; // Tiempo m�nimo entre spawns
    [SerializeField] private float maxSpawnTime = 5f; // Tiempo m�ximo entre spawns
    [SerializeField] private bool deadEnemy = false;
    [SerializeField] private GameObject vfxDisappearEnemy;

    private List<GameObject> enemigosInstanciados = new List<GameObject>();

    void InicializarEnemySpawn()
    {
        deadEnemy = false;

        if (inGame && !deadEnemy)
        {
            //Debug.Log("Se inicio el Spawn de Enemigos");
            //print(deadEnemy);
            if (modoHS)
            {
                for (int i = 0; i < enemigosMaximosActivos; i++)
                {
                    SpawnEnemy();
                }

                InvokeRepeating(nameof(SpawnEnemyContinuously), Random.Range(minSpawnTime, maxSpawnTime), Random.Range(minSpawnTime, maxSpawnTime));
            }
            else if (modoDS)
            {
                if (isRunning)
                {
                    for (int i = 0; i < enemigosMaximosActivos; i++)
                    {
                        SpawnEnemy();
                    }

                    InvokeRepeating(nameof(SpawnEnemyContinuously), Random.Range(minSpawnTime, maxSpawnTime), Random.Range(minSpawnTime, maxSpawnTime));
                }
            }
        }
        else
        {
            return;
        }

    }

    private void SpawnEnemy()
    {
        if (enemigosInstanciados.Count >= enemigosMaximosActivos) return; // No spawnear m�s si se ha alcanzado el l�mite

        // Seleccionamos un enemigo aleatoriamente y lo agregamos al objeto
        GameObject enemyPrefab = prefabsEnemigos[Random.Range(0, prefabsEnemigos.Length)];

        // Seleccionamos un punto de spawn aleatorio que no est� ocupado
        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null) return; // Si no hay puntos de spawn disponibles, no hacer nada

        // Instanciar el enemigo
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemigosInstanciados.Add(spawnedEnemy);
    }

    void DestruirEnemigosActivos()
    {
        for (int i = enemigosInstanciados.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemigosInstanciados[i];
            if (enemy != null && enemy.activeSelf)
            {
                GameObject cloneVFX = Instantiate(vfxDisappearEnemy, enemy.transform.position, Quaternion.identity);
                Destroy(cloneVFX, 3);
                Destroy(enemy);
                enemigosInstanciados.RemoveAt(i);
            }
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>(spawnPointsEnemies);

        // Eliminamos puntos de spawn ocupados
        foreach (GameObject enemy in enemigosInstanciados)
        {
            availablePoints.Remove(enemy.transform);
        }

        if (availablePoints.Count == 0) return null; // No hay puntos de spawn disponibles

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    private void SpawnEnemyContinuously()
    {
        if (!inGame || deadEnemy)
            return;

        CheckAndRemoveDeadEnemies();

        if (enemigosInstanciados.Count < enemigosMaximosActivos)
        {
            // Generar un nuevo enemigo en un punto de spawn aleatorio
            int spawnPointIndex = Random.Range(0, spawnPointsEnemies.Length);
            Transform spawnPoint = spawnPointsEnemies[spawnPointIndex];

            // Seleccionar un enemigo aleatorio del array
            GameObject enemyPrefab = prefabsEnemigos[Random.Range(0, prefabsEnemigos.Length)];

            // Instanciar el enemigo
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemigosInstanciados.Add(newEnemy);
        }

        // Planificar la siguiente llamada a este m�todo
        Invoke("SpawnEnemyContinuously", Random.Range(minSpawnTime, maxSpawnTime));
    }

    private void CheckAndRemoveDeadEnemies()
    {
        for (int i = enemigosInstanciados.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemigosInstanciados[i];

            if (enemy == null)
            {
                enemigosInstanciados.RemoveAt(i);
                continue;
            }

            EnemyAI_Meele meeleAI = enemy.GetComponent<EnemyAI_Meele>();
            EnemyAI_Flying flyingAI = enemy.GetComponent<EnemyAI_Flying>();


            if ((meeleAI != null && meeleAI.vida <= 0) || (flyingAI != null && flyingAI.vida <= 0))
            {
                Destroy(enemy);
                enemigosInstanciados.RemoveAt(i);
            }
        }
    }

    private IEnumerator SpawnEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy();
    }
    #endregion SPAWN DE ENEMIGOS

}

