using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool inGame;

    #region LOBBY

    #region RECIBIR INFORMACION

    private static List<InfoLobby.PlayerInfo> infoLobbyPlayers;

    public void RecibirInformacionLobby(string json)
    {
        InfoLobby infoLobby = JsonUtility.FromJson<InfoLobby>(json);
        infoLobbyPlayers = infoLobby.playerInfos;
        //SetupHUDs(infoLobbyPlayers);
        //ActivarHUD();


        CargarEscena();
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

    #endregion LOBBY

    #region CARGAR ESCENA

    private void CargarEscena()
    {
        //EscenaDeJuego();
        Invoke("EscenaDeJuego", 0.01f);

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
        else
        {
            return;
        }

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
        }
        else if (inGame)
        {
            if (modoHS)
            {
                AudioManager.instance.StopSound("menu");
                AudioManager.instance.PlaySound("hechizos");
            }
            else if (modoDS)
            {
                AudioManager.instance.StopSound("menu");
                AudioManager.instance.PlaySound("duelo");
            }
        }
    }

    #endregion MUSICA

    #region GAME MANAGER
    public static GameManager Instance;

    private void Awake()
    {
        MakeSingleton();

        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        //InicializarMusica();
        InicializarJugadores();
        ResetiarVariables();
    }

    private void Update()
    {
        Update_Marcador_MHS();
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
    #endregion GAME MANAGER

    #region CARGAR ESCENA

    //En este metodo se pone todo lo que quieras que pase al cargar una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetiarVariables();
        InicializarMusica();
    }

    void EscenaDeJuego()
    {
        //AudioManager.instance.PlaySound("");
        inGame = true;
        InicializarHUD();
        //IniciarPartida();
        InicializarSpawnsPoints();
        Invoke("IniciarPartida", 0.02f);
        InicializarTemporizador();
        //InicializarMapas();
        InicializarPuntaje();
        //InicializarComponentesJugadores();
        InicializarPausa();
        InicializarCamara();
        Invoke("InicializarEnemySpawn", 5);

        if (modoHS)
        {
            //Aqui ira todo lo que necesita el MHS
            InicializarMHS();
            InicializarMarcadorMHS();
            magosPrincipales.SetActive(true);
        }
        else if (modoDS)
        {
            //Aqui ira todo lo que necesita el MDS
            InicializarMDS();
            InicializarMarcadorMDS();
            //pistaPintable.SetActive(true);
        }
    }

    public void ResetiarVariables()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            AudioManager.instance.PlaySound("1");

            //Booleanos Partida
            modoHS = false;
            modoDS = false;
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
            panelTiempoAgotado.SetActive(false);
            panelTemporizador.SetActive(false);

            //Puntaje
            puntosParaGanar = 1;

            //Paneles de condicion de Victoria
            panelVictoria.SetActive(false);

            //Mapas
            //mapaStreetMHS.SetActive(false);
            //mapaRestaurantMHS.SetActive(false);
            //mapaDungeonMHS.SetActive(false);
            //mapaStreetMDS.SetActive(false);
            //mapaRestaurantMDS.SetActive(false);
            //mapaDungeonMDS.SetActive(false);


            //Modo Hechizos Sazonados
            magosPrincipales.SetActive(false);
            panelMarcadorMHS.SetActive(false);
            camaraObjeto.SetActive(false);
            puntosAGanarTeam1 = puntajeInicial;
            puntosAGanarTeam2 = puntajeInicial;

            //Modo Duelo De Salsas
            //pistaPintable.SetActive(false);
            panelMarcadorMDS.SetActive(false);
            camaraObjeto.SetActive(false);

            //Pausa
            panelPausa.SetActive(false);
            panelControles.SetActive(false);
            panelConfiguracion.SetActive(false);
            panelConfirmacionSalida.SetActive(false);

            //Jugadores
            activePlayers = new List<PlayerController>();

            //Spawn de Enemigos
            deadEnemy = true;
            enemigosInstanciados = new List<GameObject>();
            DestruirEnemigosActivos();

            //Cerrar el HUD
            panelHUDs.SetActive(false);

            //Booleano para juego
            inGame = false;

            //Lista de info jugadores
            //InicializarJugadores();
        }
        else return;
    }
    #endregion CARGAR ESCENA

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
            int equipoJ1 = infoLobbyPlayers[0].equipo;
            int equipoJ2 = infoLobbyPlayers[1].equipo;

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
            p1.equipo = equipoJ1;
            p1.AsignarSlot(slotsHUD[0]);
            p1.playerHUD.Name = infoLobbyPlayers[0].personaje;
            p1.gameObject.name = infoLobbyPlayers[0].personaje;
            p1.BloquearMovimiento = false;
            activePlayers.Add(p1);

            p2 = SpawnJugador(infoLobbyPlayers[1].personaje, spawn1v1J2, infoLobbyPlayers[1].gamepadId);
            p2.equipo = equipoJ2;
            p2.AsignarSlot(slotsHUD[1]);
            p2.playerHUD.Name = infoLobbyPlayers[1].personaje;
            p2.gameObject.name = infoLobbyPlayers[1].personaje;
            p2.BloquearMovimiento = false;
            activePlayers.Add(p2);



        }
        else if (infoLobbyPlayers.Count == 4)
        {
            //int equipoJ1 = infoLobbyPlayers[0].equipo;
            //int equipoJ2 = infoLobbyPlayers[1].equipo;
            //int equipoJ3 = infoLobbyPlayers[2].equipo;
            //int equipoJ4 = infoLobbyPlayers[3].equipo;

            //Transform spawn2v2J1;
            //Transform spawn2v2J2;
            //Transform spawn2v2J3;
            //Transform spawn2v2J4;

            //if (equipoJ1 == 1)
            //{
            //    spawn2v2J1 = modo2v2spawnTeam1_1;
            //}
            //else
            //{
            //    spawn2v2J1 = modo2v2spawnTeam2_1;
            //}

            //if (equipoJ2 == 1)
            //{
            //    spawn2v2J2 = modo2v2spawnTeam1_1;
            //}
            //else
            //{
            //    spawn2v2J1 = modo2v2spawnTeam2_1;
            //}

            //if (equipoJ3 == 1)
            //{

            //}

            //List<int> equipo1 = new List<int>();
            //List<int> equipo2 = new List<int>();

            //foreach (var i in infoLobbyPlayers)
            //{
            //    if (i.equipo == 1)
            //    {
            //        equipo1.Add(i.equipo);
            //    }
            //}

            slotsHUD[0].gameObject.SetActive(true);
            slotsHUD[1].gameObject.SetActive(true);
            slotsHUD[2].gameObject.SetActive(true);
            slotsHUD[3].gameObject.SetActive(true);

            p1 = SpawnJugador(infoLobbyPlayers[0].personaje, modo2v2spawnTeam1_1, infoLobbyPlayers[0].gamepadId);
            p1.equipo = infoLobbyPlayers[0].equipo;
            p1.AsignarSlot(slotsHUD[0]);
            p1.playerHUD.Name = infoLobbyPlayers[0].personaje;
            p1.BloquearMovimiento = false;
            activePlayers.Add(p1);

            p2 = SpawnJugador(infoLobbyPlayers[1].personaje, modo2v2spawnTeam1_2, infoLobbyPlayers[1].gamepadId);
            p2.equipo = infoLobbyPlayers[1].equipo;
            p2.AsignarSlot(slotsHUD[1]);
            p2.playerHUD.Name = infoLobbyPlayers[1].personaje;
            p2.BloquearMovimiento = false;
            activePlayers.Add(p2);

            p3 = SpawnJugador(infoLobbyPlayers[2].personaje, modo2v2spawnTeam2_1, infoLobbyPlayers[2].gamepadId);
            p3.equipo = infoLobbyPlayers[2].equipo;
            p3.AsignarSlot(slotsHUD[2]);
            p3.playerHUD.Name = infoLobbyPlayers[2].personaje;
            p3.BloquearMovimiento = false;
            activePlayers.Add(p3);

            p4 = SpawnJugador(infoLobbyPlayers[3].personaje, modo2v2spawnTeam2_2, infoLobbyPlayers[3].gamepadId);
            p4.equipo = infoLobbyPlayers[3].equipo;
            p4.AsignarSlot(slotsHUD[3]);
            p4.playerHUD.Name = infoLobbyPlayers[3].personaje;
            p4.BloquearMovimiento = false;
            activePlayers.Add(p4);
        }

        foreach (PlayerController j in activePlayers)
        {
            j.BloquearMovimiento = false;
        }

        InicializarBodyJugadores();
    }

    void InicializarSpawnJugadores()
    {
        if (infoLobbyPlayers.Count == 2)
        {
            if (p1.equipo == 1)
            {
                p1 = SpawnJugador(infoLobbyPlayers[0].personaje, modo1v1spawnTeam1, infoLobbyPlayers[0].gamepadId);
            }
            else
            {
                p1 = SpawnJugador(infoLobbyPlayers[0].personaje, modo1v1spawnTeam2, infoLobbyPlayers[0].gamepadId);
            }

            if (p2.equipo == 1)
            {
                p2 = SpawnJugador(infoLobbyPlayers[1].personaje, modo1v1spawnTeam1, infoLobbyPlayers[1].gamepadId);
            }
            else
            {
                p2 = SpawnJugador(infoLobbyPlayers[1].personaje, modo1v1spawnTeam2, infoLobbyPlayers[1].gamepadId);
            }
        }
    }

    private void InicializarJugadores()
    {
        infoLobbyPlayers = new List<InfoLobby.PlayerInfo>();
    }

    public void GoToMenuVictory()
    {
        Time.timeScale = 1;
        deadEnemy = true;
        DestruirEnemigosActivos();

        SceneManager.LoadScene("ANDYMENUTEST");
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
    [SerializeField] private GameObject panelConfiguracion;
    [SerializeField] private GameObject panelConfirmacionSalida;
    [SerializeField] private static GameObject panelStaticPausa;

    private static bool enPausa;
    public static bool EnPausa => enPausa;

    void InicializarPausa()
    {
        panelStaticPausa = panelPausa;
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
    }

    public static void Pausa(PlayerController player)
    {
        //Se invierten entre ellos
        enPausa = !enPausa;

        if (enPausa)
        {
            Time.timeScale = 0;
            panelStaticPausa.SetActive(true);

            foreach (PlayerController j in activePlayers)
            {
                j.BloquearMovimiento = true;
            }
        }
        else
        {
            Time.timeScale = 1;
            panelStaticPausa.SetActive(false);

            foreach (PlayerController j in activePlayers)
            {
                j.BloquearMovimiento = false;
            }

        }

    }

    public void Resumir()
    {
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelConfirmacionSalida.SetActive(false);

        Pausa(null);
    }

    public void Controles()
    {
        panelControles.SetActive(true);
        panelConfiguracion.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
    }

    public void Configuracion()
    {
        panelConfiguracion.SetActive(true);
        panelControles.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
    }

    public void ConfirmarSalida()
    {
        panelConfirmacionSalida.SetActive(true);
        panelControles.SetActive(false);
        panelConfiguracion.SetActive(false);
    }

    public void Salir()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Back()
    {
        panelControles.SetActive(false);
        panelConfiguracion.SetActive(false);
        panelConfirmacionSalida.SetActive(false);
    }
    #endregion PAUSA

    #region PUNTAJE
    [Header("Puntaje Core")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private TMP_Text playerWinText;
    [SerializeField] private int puntosAGanarTeam1;
    [SerializeField] private int puntosAGanarTeam2;
    [SerializeField] public int puntosParaGanar;
    private int puntajeInicial = 0;

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
        panelVictoria.SetActive(false);
        panelMarcadorMHS.SetActive(true);
        numeroDeRonda = rondaInicial;
        rondaText.text = numeroDeRonda.ToString();
    }

    void Update_Marcador_MHS()
    {
        if (inGame)
        {
            if (infoLobbyPlayers.Count == 2)
            {
                if (puntosAGanarTeam1 >= puntosParaGanar && isRunning)
                {
                    isRunning = false;
                    deadEnemy = true;
                    DestruirEnemigosActivos();
                    panelVictoria.SetActive(true);
                    playerWinText.text = "Team 1";
                    p1.enabled = false;
                    p2.enabled = false;
                    Invoke("DetenerTiempo", 0.80f);
                }

                else if (puntosAGanarTeam2 >= puntosParaGanar && isRunning)
                {
                    isRunning = false;
                    deadEnemy = true;
                    DestruirEnemigosActivos();
                    panelVictoria.SetActive(true);
                    playerWinText.text = "Team 2";
                    p1.enabled = false;
                    p2.enabled = false;
                    Invoke("DetenerTiempo", 0.80f);
                }
            }

            else if (infoLobbyPlayers.Count == 4)
            {
                if (puntosAGanarTeam1 >= puntosParaGanar && isRunning)
                {
                    isRunning = false;
                    deadEnemy = true;
                    DestruirEnemigosActivos();
                    panelVictoria.SetActive(true);
                    playerWinText.text = "Team 1";
                    p1.enabled = false;
                    p2.enabled = false;
                    p3.enabled = false;
                    p4.enabled = false;
                    Invoke("DetenerTiempo", 0.80f);
                }

                else if (puntosAGanarTeam2 >= puntosParaGanar && isRunning)
                {
                    isRunning = false;
                    deadEnemy = true;
                    DestruirEnemigosActivos();
                    panelVictoria.SetActive(true);
                    playerWinText.text = "Team 2";
                    p1.enabled = false;
                    p2.enabled = false;
                    p3.enabled = false;
                    p4.enabled = false;
                    Invoke("DetenerTiempo", 0.80f);
                }
            }

        }
        else return;

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
        panelVictoria.SetActive(false);
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

    public static float remainingTime;
    private bool isRunning = false;

    private void InicializarTemporizador()
    {
        panelTemporizador.SetActive(true);
        panelTiempoAgotado.SetActive(false);
        remainingTime = totalTime;
        isRunning = true;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //Start_TimerText();
    }

    private void FixUpdate_Temporizador()
    {
        if (inGame)
        {
            if (isRunning)
            {
                timerText.text = remainingTime.ToString();
                remainingTime -= Time.fixedDeltaTime;
            }

            if (remainingTime <= 0)
            {
                isRunning = false;
                timerText.text = "00:00";
                TimerEnded();

                if (modoHS)
                {
                    panelTiempoAgotado.SetActive(true);
                    TiempoAgotadoText.text = "�SE AGOTO EL TIEMPO!";
                    Time.timeScale = 0;

                }
                else if (modoDS)
                {
                    TiempoAgotadoMDS();
                }

            }
        }
    }

    private void TimerEnded()
    {
        timerText.text = "00:00";
    }

    private void Start_TimerText()
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

    private GameObject bodyP1;
    private GameObject bodyP2;
    private GameObject bodyP3;
    private GameObject bodyP4;

    private static PlayerController p1, p2, p3, p4;

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
            playerController.gamepadIndex = LobbyManager.self.GetGamepadById(gamepadId); // Asignar el Gamepad
            return playerController;
        }

        return null;
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

    void InicializarCamara()
    {
        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);
        //originalCameraPosition = camaraPrincipal.transform.position;
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
        cuadrosTeam1 = new List<CuadroPintable>();
        cuadrosTeam2 = new List<CuadroPintable>();
    }

    public static void CuadradoCambiado(CuadroPintable cuadro)
    {
        //Equipo 1
        if (cuadro.equipoActual == 1)
        {
            //Aumentamos el puntaje
            CuadrosEquipo1++;

            //Si el cuadro ya estaba pintado por otro equipo, le restamos al otro equipo
            if (cuadro.pintado) CuadrosEquipo2--;
        }
        //Equipo 2
        else
        {
            //Aumentamos el puntaje
            CuadrosEquipo2++;

            //Si el cuadro ya estaba pintado por otro equipo, le restamos al otro equipo
            if (cuadro.pintado) CuadrosEquipo1--;
        }

    }

    //Metodo para verifica que lista tiene mas cuadros pintados cuanso acabe el tiempo
    private void TiempoAgotadoMDS()
    {
        int team1Count = cuadrosTeam1.Count;
        int team2Count = cuadrosTeam2.Count;

        if (infoLobbyPlayers.Count == 2)
        {
            deadEnemy = true;
            isRunning = false;
            DestruirEnemigosActivos();

            if (team1Count > team2Count)
            {
                panelVictoria.SetActive(true);
                playerWinText.text = "Team: 1";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
            else if (team2Count > team1Count)
            {
                panelVictoria.SetActive(true);
                playerWinText.text = "Team: 2";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
            else if (team1Count == 0 || team2Count == 0)
            {
                panelTiempoAgotado.SetActive(true);
                TiempoAgotadoText.text = "�SE AGOTO EL TIEMPO!";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
        }

        else if (infoLobbyPlayers.Count == 4)
        {
            if (team1Count > team2Count)
            {
                panelVictoria.SetActive(true);
                playerWinText.text = "Team: 1";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
            else if (team2Count > team1Count)
            {
                panelVictoria.SetActive(true);
                playerWinText.text = "Team: 2";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
            else if (team1Count == 0 || team2Count == 0)
            {
                panelTiempoAgotado.SetActive(true);
                TiempoAgotadoText.text = "�SE AGOTO EL TIEMPO!";

                foreach (PlayerController p in activePlayers)
                {
                    if (p != null)
                    {
                        p.BloquearMovimiento = true;
                    }
                }

                Time.timeScale = 0;
            }
        }
    }
    #endregion MODO DUELO DE SALSAS

    #region MODO HECHIZOS SAZONADOS
    [Header("Modo Hechizos Sazonados")]
    [SerializeField] private GameObject magosPrincipales;

    [Header("Magos")]
    [SerializeField] private GameObject mago1;
    [SerializeField] private GameObject mago2;

    [Header("C�mara Stats MH")]
    [SerializeField] private Transform posicionCinematica;
    [SerializeField] private float velocidadMovCamara = 2f;
    private Animator camaraPrincipalAnimator;

    private Vector3 originalCameraPosition; // Posici�n original de la c�mara

    [Header("Hechizo")]
    [SerializeField] private GameObject hechizoPrefab;
    [SerializeField] private float hechizoVelocidad = 10f;
    [SerializeField] private Transform spawnHechizo1;
    [SerializeField] private Transform spawnHechizo2;

    void InicializarMHS()
    {
        panelMarcadorMHS.SetActive(true);
        camaraPrincipalAnimator = camaraPrincipal.GetComponent<Animator>();
    }

    //Parte de los Magos
    void Mago1()
    {
        Debug.Log("Si se intancio el hechizo del mago 1");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo1.transform.position, Quaternion.identity);
        StartCoroutine(MoverHechizo1(hechizo));
    }

    void Mago2()
    {
        Debug.Log("Si se intancio el hechizo del mago 2");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo2.transform.position, Quaternion.identity);
        StartCoroutine(MoverHechizo2(hechizo));
    }

    IEnumerator MoverHechizo1(GameObject hechizo)
    {
        while (hechizo != null)
        {
            hechizo.transform.position = Vector3.MoveTowards(hechizo.transform.position, mago2.transform.position, 10f * Time.deltaTime);

            if (Vector3.Distance(hechizo.transform.position, mago2.transform.position) < 0.1f)
            {
                Destroy(hechizo);
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
                Destroy(hechizo);
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
                if (infoLobbyPlayers.Count == 2)
                {
                    if (player.equipo == 1 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.muerto = true;
                        p2.muerto = true;
                        p1.enabled = false;
                        p2.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        Invoke("Mago2", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                    else if (player.equipo == 2 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.muerto = true;
                        p2.muerto = true;
                        p1.enabled = false;
                        p2.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        Invoke("Mago1", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                }

                else if (infoLobbyPlayers.Count == 4)
                {
                    if (player.equipo == 1 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.muerto = true;
                        p2.muerto = true;
                        p3.muerto = true;
                        p4.muerto = true;
                        p1.enabled = false;
                        p2.enabled = false;
                        p3.enabled = false;
                        p4.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        p3.BloquearMovimiento = true;
                        p4.BloquearMovimiento = true;
                        Invoke("Mago2", 2f);
                        Invoke("CambioDeRondaMHS", 2f);
                    }
                    else if (player.equipo == 2 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        DestruirEnemigosActivos();
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.muerto = true;
                        p2.muerto = true;
                        p3.muerto = true;
                        p4.muerto = true;
                        p1.enabled = false;
                        p2.enabled = false;
                        p3.enabled = false;
                        p4.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        p3.BloquearMovimiento = true;
                        p4.BloquearMovimiento = true;
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
            if (infoLobbyPlayers.Count == 2)
            {
                if (player.equipo == 1)
                {
                    if (player == p1)
                    {
                        p1.BloquearMovimiento = true;
                        p1.enabled = false;
                        StartCoroutine(RespawnearJugadorMDS(player, 4.0f));

                    }
                    else
                    {
                        p2.BloquearMovimiento = true;
                        p2.enabled = false;
                        StartCoroutine(RespawnearJugadorMDS(player, 4.0f));

                    }
                }
                else if (player.equipo == 2)
                {
                    if (player == p1)
                    {
                        p1.BloquearMovimiento = true;
                        p1.enabled = false;
                        StartCoroutine(RespawnearJugadorMDS(player, 4.0f));
                    }
                    else
                    {
                        p2.BloquearMovimiento = true;
                        p2.enabled = false;
                        StartCoroutine(RespawnearJugadorMDS(player, 4.0f));
                    }

                }
            }
            else if (infoLobbyPlayers.Count == 4)
            {
                if (player.equipo == 1)
                {
                    player.BloquearMovimiento = true;
                    player.enabled = false;
                    StartCoroutine(RespawnearJugadorMDS(player, 5));
                }
                else if (player.equipo == 2)
                {
                    player.BloquearMovimiento = true;
                    player.enabled = false;
                    StartCoroutine(RespawnearJugadorMDS(player, 5));
                }
            }

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

        if (activePlayers.Count == 2)
        {
            Debug.Log("CAMBIO DE RONDA");
            // Desactivamos los prefabs de los jugadores
            p1.transform.GetChild(0).gameObject.SetActive(false);
            p2.transform.GetChild(0).gameObject.SetActive(false);



            // Los movemos a sus posiciones iniciales
            p1.transform.position = respawnJ1.position;
            p2.transform.position = respawnJ2.position;
        }
        else
        {
            // Desactivamos los prefabs de los jugadores
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            p3.gameObject.SetActive(false);
            p4.gameObject.SetActive(false);

            // Los movemos a sus posiciones iniciales
            p1.transform.position = modo2v2spawnTeam1_1.position;
            p2.transform.position = modo2v2spawnTeam1_2.position;
            p3.transform.position = modo2v2spawnTeam2_1.position;
            p4.transform.position = modo2v2spawnTeam2_2.position;
        }


        // Los reactivamos
        StartCoroutine(ReactivacionMHS(2.5f));
    }

    IEnumerator ReactivacionMHS(float time)
    {
        if (infoLobbyPlayers.Count == 2)
        {
            Debug.Log("Se reactivaron los jugadores");
            yield return new WaitForSeconds(2.25f);

            p1.Vida = 100;
            p2.Vida = 100;
            p1.anim.SetTrigger("spawn");
            p2.anim.SetTrigger("spawn");
            p1.enabled = true;
            p2.enabled = true;
            p1.transform.GetChild(0).gameObject.SetActive(true);
            p2.transform.GetChild(0).gameObject.SetActive(true);
            p1.BloquearMovimiento = false;
            p2.BloquearMovimiento = false;
            p1.muerto = false;
            p2.muerto = false;

        }

        else if (infoLobbyPlayers.Count == 4)
        {
            Debug.Log("Se reactivaron los jugadores");
            yield return new WaitForSeconds(2.25f);
            p1.gameObject.SetActive(true);
            p2.gameObject.SetActive(true);
            p3.gameObject.SetActive(true);
            p4.gameObject.SetActive(true);
            p1.enabled = true;
            p2.enabled = true;
            p3.enabled = true;
            p4.enabled = true;
            p1.Vida = 100;
            p2.Vida = 100;
            p3.Vida = 100;
            p4.Vida = 100;
            p1.anim.SetTrigger("spawn");
            p2.anim.SetTrigger("spawn");
            p3.anim.SetTrigger("spawn");
            p4.anim.SetTrigger("spawn");
            p1.BloquearMovimiento = false;
            p2.BloquearMovimiento = false;
            p3.BloquearMovimiento = false;
            p4.BloquearMovimiento = false;
            p1.muerto = false;
            p2.muerto = false;
            p3.muerto = false;
            p4.muerto = false;
        }

        yield return new WaitForSeconds(3f);
        isRunning = true;
        remainingTime = totalTime;
        yield return new WaitForSeconds(7f);
        InicializarEnemySpawn();
    }

    private IEnumerator RespawnearJugadorMDS(PlayerController player, float time)
    {
        Debug.Log("Se respauneo el jugador: " + player.name);
        yield return new WaitForSeconds(time);
        player.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        if (infoLobbyPlayers.Count == 2)
        {
            if (player == p1)
            {
                p1.Vida = 100;
                p1.habilidadProgreso = 0f;
                p1.BloquearMovimiento = false;
                p1.anim.SetTrigger("spawn");
                p1.transform.position = respawnJ1.position;
                p1.transform.GetChild(0).gameObject.SetActive(true);
                p1.enabled = true;
            }
            else if (player == p2)
            {
                p2.Vida = 100;
                p2.habilidadProgreso = 0f;
                p2.BloquearMovimiento = false;
                p2.anim.SetTrigger("spawn");
                p2.transform.position = respawnJ2.position;
                p2.transform.GetChild(0).gameObject.SetActive(true);
                p2.enabled = true;
            }

        }
        else if (infoLobbyPlayers.Count == 4)
        {
            if (player == p1)
            {
                p1.gameObject.SetActive(true);
                p1.Vida = 100;
                p1.habilidadProgreso = 0;
                p1.enabled = true;
                p1.anim.SetTrigger("spawn");
                p1.transform.position = modo2v2spawnTeam1_1.localPosition;
            }

            else if (player == p2)
            {
                p2.gameObject.SetActive(true);
                p2.Vida = 100;
                p2.habilidadProgreso = 0;
                p2.enabled = true;
                p2.anim.SetTrigger("spawn");
                p2.transform.position = modo2v2spawnTeam1_2.localPosition;
            }

            else if (player == p3)
            {
                p3.gameObject.SetActive(true);
                p3.Vida = 100;
                p3.habilidadProgreso = 0;
                p3.enabled = true;
                p3.anim.SetTrigger("spawn");
                p3.transform.position = modo2v2spawnTeam2_1.localPosition;
            }

            else if (player == p4)
            {
                p4.gameObject.SetActive(true);
                p4.Vida = 100;
                p4.habilidadProgreso = 0;
                p4.enabled = true;
                p4.anim.SetTrigger("spawn");
                p4.transform.position = modo2v2spawnTeam2_2.localPosition;
            }
        }

        yield return new WaitForSeconds(6.0f);
    }


    //IEnumerator ReactivacionMDS(float time)
    //{
    //    if(infoLobbyPlayers.Count == 2)
    //    {

    //    }
    //    yield return null;
    //    yield return new WaitForSeconds(6.0f);
    //    player.gameObject.SetActive(false);
    //    player.transform.position = spawnPoint.position;
    //    ResetearJugador(player);
    //    player.gameObject.SetActive(true);
    //    yield return new WaitForSeconds(time);
    //}

    private void BloquearMovimientoJugadores()
    {
        Debug.Log("Se bloquearon los movimientos de los jugadores");
        p1.BloquearMovimiento = true;
        p2.BloquearMovimiento = true;
        p3.BloquearMovimiento = true;
        p4.BloquearMovimiento = true;
    }

    private void ResetearJugador(PlayerController player)
    {
        player.Vida = 100;
        player.enabled = true;
        player.isInvulnerable = false;
        player.anim.SetTrigger("spawn");
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

            if (isRunning)
            {
                for (int i = 0; i < enemigosMaximosActivos; i++)
                {
                    SpawnEnemy();
                }

                InvokeRepeating(nameof(SpawnEnemyContinuously), Random.Range(minSpawnTime, maxSpawnTime), Random.Range(minSpawnTime, maxSpawnTime));
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
                Destroy(enemy);
                enemigosInstanciados.RemoveAt(i);
                GameObject cloneVFX = Instantiate(vfxDisappearEnemy, enemy.transform);
                Destroy(cloneVFX, 3);
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
