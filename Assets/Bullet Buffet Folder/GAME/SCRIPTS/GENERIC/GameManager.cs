using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region LOBBY

    #region RECIBIR INFORMACION

    private static List<InfoLobby.PlayerInfo> infoLobbyPlayers;

    public void RecibirInformacionLobby(string json)
    {
        InfoLobby infoLobby = JsonUtility.FromJson<InfoLobby>(json);
        infoLobbyPlayers = infoLobby.playerInfos;
        //SetupHUDs(infoLobbyPlayers);
        //ActivarHUD();
    }

    #endregion RECIBIR INFORMACION

    #region BUSCAR JUGADORES

    private PlayerController FindPlayerController(int gamepadId)
    {
        Gamepad gamepad;

        if (LobbyManager.self.idToGamepad.TryGetValue(gamepadId, out gamepad))
        {
            foreach (var player in FindObjectsOfType<PlayerController>())
            {
                //if (player._gamepad == gamepad)
                //{
                //    return player;
                //}
            }
        }
        return null;
    }
    #endregion BUSCAR JUGADORES

    #region LIGAR INFORMACION AL HUD

    [Header("Panels")]
    [SerializeField] private GameObject panelHUDs;
    [SerializeField] private PlayerHUD[] slotsHUD;
    //[SerializeField] private GameObject panelCuatroJugadores;

    private const float MaxHealth = 100f;

    private List<PlayerHUD> playerHUDs = new List<PlayerHUD>();

    void InicializarHUD()
    {
        panelHUDs.SetActive(true);
    }

    //void ActivarHUD()
    //{
    //    if(infoLobbyPlayers.Count == 2)
    //    {
    //        panelHUDs.SetActive(true);
    //    }
    //    else if (infoLobbyPlayers.Count == 4)
    //    {
    //        panelCuatroJugadores.SetActive(true);
    //    }
    //}

    //public void SetupPlayer(PlayerController player)
    //{
    //    InfoLobby.PlayerInfo playerInfo = infoLobbyPlayers.Find(p => p.gamepadId == player.gamepadIndex.deviceId);
    //    player.equipo = playerInfo.equipo;
    //    player.hudSlot = AssignHUDSlot(player);
    //}

    //public void RegisterPlayerHUD(PlayerController player, PlayerHUD playerHUD)
    //{
    //    playerHUDMapping[player] = playerHUD;
    //}

    //public PlayerHUD GetPlayerHUD(PlayerController player)
    //{
    //    return playerHUDMapping.TryGetValue(player, out PlayerHUD playerHUD) ? playerHUD : null;
    //}

    //public GameObject AssignHUDSlot(PlayerController player)
    //{
    //    if (infoLobbyPlayers.Count == 2)
    //    {
    //        panelHUDs.SetActive(true);
    //        panelCuatroJugadores.SetActive(false);
    //        return panelHUDs.transform.GetChild(player.gamepadIndex.deviceId).gameObject;
    //    }
    //    else if (infoLobbyPlayers.Count == 4)
    //    {
    //        panelHUDs.SetActive(false);
    //        panelCuatroJugadores.SetActive(true);
    //        return panelCuatroJugadores.transform.GetChild(player.gamepadIndex.deviceId).gameObject;
    //    }

    //    return null;
    //}

    //public void SetupHUDs(List<InfoLobby.PlayerInfo> playerInfos)
    //{
    //    if (playerInfos.Count == 2)
    //    {
    //        panelHUDs.SetActive(true);
    //        SetupTwoPlayersHUD(playerInfos);
    //    }
    //    else if (playerInfos.Count == 4)
    //    {
    //        panelCuatroJugadores.SetActive(true);
    //        SetupFourPlayersHUD(playerInfos);
    //    }

    //    foreach (var playerInfo in playerInfos)
    //    {
    //        Debug.Log("Entro al foreach, info del playerInfo: " + playerInfo);
    //        PlayerController playerController = FindPlayerController(playerInfo.gamepadId);
    //        if (playerController != null)
    //        {
    //            Debug.Log("Se encontro player metodo SetupHuds");
    //            PlayerHUD playerHUD = GetHUDForPlayer(playerController);

    //            if (playerHUD != null)
    //            {
    //                playerHUD.SetupHUD(playerInfo);
    //                playerHUDMapping[playerController] = playerHUD; // Mapear el PlayerController al HUD

    //                // Asignar valores de salud iniciales
    //                float initialHealth = MaxHealth;
    //                playerHUD.UpdateHealth(initialHealth, MaxHealth);
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"No se encontró el PlayerController para el gamepadId {playerInfo.gamepadId}");
    //        }
    //    }
    //}

    //private void SetupTwoPlayersHUD(List<InfoLobby.PlayerInfo> playerInfos)
    //{
    //    // Referenciar HUDs de los paneles de dos jugadores
    //    PlayerHUD hud1 = panelHUDs.transform.GetChild(0).GetComponent<PlayerHUD>();
    //    PlayerHUD hud2 = panelHUDs.transform.GetChild(1).GetComponent<PlayerHUD>();

    //    // Asignar información a los HUDs
    //    playerHUDs.Add(hud1);
    //    playerHUDs.Add(hud2);

    //    AssignHUDs(playerInfos);
    //}

    //private void SetupFourPlayersHUD(List<InfoLobby.PlayerInfo> playerInfos)
    //{
    //    // Referenciar HUDs de los paneles de cuatro jugadores
    //    for (int i = 0; i < 4; i++)
    //    {
    //        PlayerHUD hud = panelCuatroJugadores.transform.GetChild(i).GetComponentInChildren<PlayerHUD>();

    //        //PlayerHUD hud = panelCuatroJugadores.transform.GetComponentsInChildren<PlayerHUD>(i);
    //        playerHUDs.Add(hud);
    //    }

    //    // Asignar información a los HUDs
    //    AssignHUDs(playerInfos);
    //}

    //private void AssignHUDs(List<InfoLobby.PlayerInfo> playerInfos)
    //{
    //    int team1Index = 0;
    //    int team2Index = playerHUDs.Count / 2; // 2 for 1v1, 2 for 2v2

    //    foreach (var playerInfo in playerInfos)
    //    {
    //        PlayerHUD hud;
    //        if (playerInfo.equipo == 1)
    //        {
    //            hud = playerHUDs[team1Index++];
    //        }
    //        else
    //        {
    //            hud = playerHUDs[team2Index++];
    //        }

    //        hud.SetupHUD(playerInfo);
    //    }
    //}
    #endregion LIGAR INFORMACION AL HUD

    #region MODIFICAR HUD
    // Diccionario para mapear PlayerController a PlayerHUD
    //private Dictionary<PlayerController, PlayerHUD> playerHUDMapping = new Dictionary<PlayerController, PlayerHUD>();


    //public void UpdatePlayerHealth(PlayerController player, int health, int maxHealth)
    //{
    //    PlayerHUD hud = GetHUDForPlayer(player);
    //    if (hud != null)
    //    {
    //        hud.UpdateHealth((float)health, (float)maxHealth); // Pasar ambos parámetros como float
    //    }
    //}

    //public void UpdatePlayerAbility(PlayerController player, float progress)
    //{
    //    PlayerHUD hud = GetHUDForPlayer(player);
    //    if (hud != null)
    //    {
    //        hud.UpdateHability(progress);

    //        if (progress >= 0.97f)
    //        {
    //            hud.EnableSuperShootIcon();
    //        }
    //        else
    //        {
    //            hud.DisableSuperShootIcon();
    //        }

    //        if (progress >= 0.47f)
    //        {
    //            hud.EnableExplosiveBulletIcon();
    //        }
    //        else
    //        {
    //            hud.DisableExplosiveBulletIcon();
    //        }
    //    }
    //}

    //public void UpdatePlayerPowerUp(PlayerController player, string powerUp)
    //{
    //    PlayerHUD playerHUD = GetHUDForPlayer(player);
    //    if (playerHUD != null)
    //    {
    //        if (string.IsNullOrEmpty(powerUp))
    //        {
    //            playerHUD.DisablePowerUpIcons();
    //        }
    //        else
    //        {
    //            playerHUD.EnablePowerUpIcon(powerUp);
    //        }
    //    }
    //}

    //public void UpdateDashStatus(PlayerController player, bool isActive, float count)
    //{
    //    PlayerHUD hud = GetHUDForPlayer(player);
    //    if (hud != null)
    //    {
    //        hud.UpdateDashStatus(isActive, count);
    //    }
    //}

    //public void UpdateShieldStatus(PlayerController player, bool isActive, int count)
    //{
    //    PlayerHUD hud = GetHUDForPlayer(player);
    //    if (hud != null)
    //    {
    //        hud.UpdateShieldStatus(isActive, count);
    //    }
    //}

    //private PlayerHUD GetHUDForPlayer(PlayerController player)
    //{
    //    // Implementar lógica para obtener el HUD correspondiente basado en el PlayerController
    //    if (playerHUDMapping.TryGetValue(player, out PlayerHUD hud))
    //    {
    //        Debug.Log("Se obtuvo el hud para el player");
    //        return hud;
    //    }
    //    return null;
    //}
    #endregion MODIFICAR HUD

    #endregion LOBBY

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


        InicializarJugadores();
        EscenaDeJuego();
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
        EscenaDeJuego();
    }

    void EscenaDeJuego()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {

            InicializarHUD();
            IniciarPartida();
            InicializarTemporizador();
            InicializarMapas();
            InicializarPuntaje();
            InicializarMuerteJugadores();
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
                pistaPintable.SetActive(true);
            }
        }
        else
        {
            return;
        }
    }

    public void ResetiarVariables()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            Debug.Log("Se Resetiaron las variables");

            //Booleanos Partida
            modoHS = false;
            modoDS = false;
            boolMapaStreetMHS = false;
            boolMapaDungeonMHS = false;
            boolMapaRestaurantMHS = false;

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
            mapaStreetMHS.SetActive(false);
            mapaRestaurantMHS.SetActive(false);
            mapaDungeonMHS.SetActive(false);

            //Modo Hechizos Sazonados
            magosPrincipales.SetActive(false);
            panelMarcadorMHS.SetActive(false);
            camaraObjeto.SetActive(false);
            puntosAGanarTeam1 = puntajeInicial;
            puntosAGanarTeam2 = puntajeInicial;

            //Modo Duelo De Salsas
            pistaPintable.SetActive(false);
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
            deadEnemy = false;
            enemigosInstanciados = new List<GameObject>();
            DestruirEnemigosActivos();

            //Cerrar el HUD
            panelHUDs.SetActive(false);
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
    public static bool modoHS;
    public static bool modoDS;

    public static List<PlayerController> activePlayers = new List<PlayerController>();


    public void IniciarPartida()
    {
        if (SceneManager.GetActiveScene().name != "ANDYINGAME")
            return;

        Debug.Log("Se inicio el metodo: IniciarPartida");

        if (infoLobbyPlayers == null)
            InicializarJugadores();

        slotsHUD[0].gameObject.SetActive(false);
        slotsHUD[1].gameObject.SetActive(false);
        slotsHUD[2].gameObject.SetActive(false);
        slotsHUD[3].gameObject.SetActive(false);

        if (infoLobbyPlayers.Count == 2)
        {
            slotsHUD[0].gameObject.SetActive(true);
            slotsHUD[1].gameObject.SetActive(true);

            p1 = SpawnJugador(infoLobbyPlayers[0].personaje, modo1v1spawnTeam1, infoLobbyPlayers[0].gamepadId);
            p1.equipo = infoLobbyPlayers[0].equipo;
            p1.AsignarSlot(slotsHUD[0]);
            p1.playerHUD.Name = infoLobbyPlayers[0].personaje;
            p1.BloquearMovimiento = false;
            activePlayers.Add(p1);

            p2 = SpawnJugador(infoLobbyPlayers[1].personaje, modo1v1spawnTeam2, infoLobbyPlayers[1].gamepadId);
            p2.equipo = infoLobbyPlayers[1].equipo;
            p2.AsignarSlot(slotsHUD[1]);
            p2.playerHUD.Name = infoLobbyPlayers[1].personaje;
            p2.BloquearMovimiento = false;
            activePlayers.Add(p2);
        }
        else if (infoLobbyPlayers.Count == 4)
        {
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

    }

    private void InicializarJugadores()
    {
        infoLobbyPlayers = new List<InfoLobby.PlayerInfo>();
    }

    public void GoToMenuVictory()
    {
        Time.timeScale = 1;
        deadEnemy = false;
        DestruirEnemigosActivos();
        SceneManager.LoadScene("ANDYMENUTEST");
    }
    #endregion PARTIDA

    #region MAPAS
    [Header("Mapas")]
    [SerializeField] private GameObject mapaStreetMHS;
    [SerializeField] private GameObject mapaRestaurantMHS;
    [SerializeField] private GameObject mapaDungeonMHS;

    void InicializarMapas()
    {
        if (boolMapaStreetMHS)
        {
            mapaStreetMHS.SetActive(true);
        }
        else if (boolMapaDungeonMHS)
        {
            mapaDungeonMHS.SetActive(true);
        }
        else if (boolMapaRestaurantMHS)
        {
            mapaRestaurantMHS.SetActive(true);
        }
    }
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
    [SerializeField] public int puntosParaGanar; //Solo para testeo puse el 3 pero esto se modificara segun el numero de rondas que escojan los jugadores
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
        if (SceneManager.GetActiveScene().name == "ANDYINGAME" && modoHS)
        {
            if(infoLobbyPlayers.Count == 2)
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

            else if(infoLobbyPlayers.Count == 4)
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
                    p4.enabled= false;
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
    [SerializeField] private TMP_Text ganadorTiempoAgotadoText;

    public static float remainingTime;
    private bool isRunning = false;

    private void InicializarTemporizador()
    {
        panelTemporizador.SetActive(true);
        panelTiempoAgotado.SetActive(false);
        remainingTime = totalTime;
        isRunning = true;
        Start_TimerText();
    }

    private void FixUpdate_Temporizador()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {
            if (isRunning)
            {
                timerText.text = remainingTime.ToString();
                remainingTime -= Time.fixedDeltaTime;
            }

            if (remainingTime <= 0)
            {
                isRunning = false;
                TimerEnded();

                if (modoHS)
                {
                    if (p1.Vida < p2.Vida)
                    {
                        puntosAGanarTeam2++;
                        CambioDeRondaMHS();

                    }
                    else
                    {
                        puntosAGanarTeam1++;
                        CambioDeRondaMHS();
                    }

                }
                else if (modoDS)
                {
                    TiempoAgotadoMDS();
                }

            }
        }
        else
        {
            return;
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

    [Header("Spawn Points")]
    [SerializeField] private Transform modo1v1spawnTeam1;
    [SerializeField] private Transform modo1v1spawnTeam2;
    [SerializeField] private Transform modo2v2spawnTeam1_1;
    [SerializeField] private Transform modo2v2spawnTeam1_2;
    [SerializeField] private Transform modo2v2spawnTeam2_1;
    [SerializeField] private Transform modo2v2spawnTeam2_2;

    private static PlayerController p1, p2, p3, p4;

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
    [Header("Modo Duelo De Salsas")]

    [Header("Pista")]
    [SerializeField] private GameObject pistaPintable;
    [SerializeField] private List<CuadroPintable> cuadrosPintables;
    private List<CuadroPintable> cuadrosTeam1;
    private List<CuadroPintable> cuadrosTeam2;

    void InicializarMDS()
    {
        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);
        panelMarcadorMDS.SetActive(true);
        cuadrosTeam1 = new List<CuadroPintable>();
        cuadrosTeam2 = new List<CuadroPintable>();
    }

    public void RegistrarCuadroPintado(CuadroPintable cuadro)
    {
        //Verificamos que nombre nos llamo el cuadro
        if (cuadro.currentTeam == 1)
        {
            //Si no se encuentra en la lista el cuadro mandado, lo agregamos
            if (!cuadrosTeam1.Contains(cuadro))
            {
                puntosAGanarTeam1++;
                puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
                cuadrosTeam1.Add(cuadro);
            }

        }
        else if (cuadro.currentTeam == 2)
        {
            if (!cuadrosTeam2.Contains(cuadro))
            {
                puntosAGanarTeam2++;
                puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();
                cuadrosTeam2.Add(cuadro);
            }

        }
        else
        {
            Debug.LogError("No se registro ningun cuadro a ningun Jugador");
        }

    }

    public void RemoverCuadroPintado(CuadroPintable cuadro, int ownerTeam)
    {
        if (ownerTeam == 1 && cuadrosTeam1.Contains(cuadro))
        {
            puntosAGanarTeam1--;
            puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
            cuadrosTeam1.Remove(cuadro);
        }
        else if (ownerTeam == 2 && cuadrosTeam2.Contains(cuadro))
        {
            puntosAGanarTeam2--;
            puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();
            cuadrosTeam2.Remove(cuadro);
        }
    }

    //Metodo para verifica que lista tiene mas cuadros pintados cuanso acabe el tiempo
    private void TiempoAgotadoMDS()
    {
        int team1Count = cuadrosTeam1.Count;
        int team2Count = cuadrosTeam2.Count;

        if(infoLobbyPlayers.Count == 2)
        {
            deadEnemy = true;
            isRunning = false;
            DestruirEnemigosActivos();

            if (team1Count > team2Count)
            {
                panelVictoria.SetActive(true);
                playerWinText.text = "Team: 1";

                foreach(PlayerController p in activePlayers)
                {
                    if(p != null)
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
        }

        else if(infoLobbyPlayers.Count == 4)
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
        }
    }
    #endregion MODO DUELO DE SALSAS

    #region MODO HECHIZOS SAZONADOS
    [Header("Modo Hechizos Sazonados")]
    [SerializeField] private GameObject magosPrincipales;

    [Header("Magos")]
    [SerializeField] private GameObject mago1;
    [SerializeField] private GameObject mago2;

    [Header("Cámara Stats MH")]
    [SerializeField] private Transform posicionCinematica;
    [SerializeField] private float velocidadMovCamara = 2f;
    private Animator camaraPrincipalAnimator;

    private Vector3 originalCameraPosition; // Posición original de la cámara

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

    //    // Asegurarse de que la posición final sea exacta.
    //    camaraTransform.position = targetPosition;

    //    // Regresar la cámara a su posición original
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

    #region DEAD EVENT PLAYER MHS

    //CharacterController chP1;
    //CharacterController chP2;

    void InicializarMuerteJugadores()
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
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.enabled = false;
                        p2.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        Invoke("DestruirEnemigosActivos", 0.25f);
                        Invoke("Mago2", 3.0f);
                        Invoke("CambioDeRondaMHS", 4.0f);
                    }
                    else if (player.equipo == 2 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.enabled = false;
                        p2.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        Invoke("DestruirEnemigosActivos", 0.25f);
                        Invoke("Mago1", 3.0f);
                        Invoke("CambioDeRondaMHS", 4.0f);
                    }
                }

                else if(infoLobbyPlayers.Count == 4)
                {
                    if (player.equipo == 1 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        puntosAGanarTeam2++;
                        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.enabled = false;
                        p2.enabled = false;
                        p3.enabled = false;
                        p4.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        p3.BloquearMovimiento = true;
                        p4.BloquearMovimiento = true;
                        Invoke("DestruirEnemigosActivos", 0.25f);
                        Invoke("Mago2", 3.0f);
                        Invoke("CambioDeRondaMHS", 4.0f);
                    }
                    else if (player.equipo == 2 && isRunning)
                    {
                        isRunning = false;
                        deadEnemy = true;
                        puntosAGanarTeam1++;
                        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                        camaraPrincipalAnimator.SetTrigger("move");
                        p1.enabled = false;
                        p2.enabled = false;
                        p1.BloquearMovimiento = true;
                        p2.BloquearMovimiento = true;
                        Invoke("DestruirEnemigosActivos", 0.25f);
                        Invoke("Mago1", 3.0f);
                        Invoke("CambioDeRondaMHS", 4.0f);
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
                    player.BloquearMovimiento = true;
                    StartCoroutine(RespawnearJugadorMDS(player, 7));
                    player.enabled = false;
                }
                else if (player.equipo == 2)
                {
                    player.BloquearMovimiento = true;
                    StartCoroutine(RespawnearJugadorMDS(player, 4.0f));
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
        // Ajustamos el número de ronda
        numeroDeRonda++;
        rondaText.text = numeroDeRonda.ToString();

        if (activePlayers.Count == 2)
        {
            Debug.Log("CAMBIO DE RONDA");
            // Desactivamos los prefabs de los jugadores
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);

            

            // Los movemos a sus posiciones iniciales
            p1.transform.position = modo1v1spawnTeam1.position;
            p2.transform.position = modo1v1spawnTeam2.position;
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
        StartCoroutine(ReactivacionMHS(3.0f));
    }

    IEnumerator ReactivacionMHS(float time)
    {
        if (infoLobbyPlayers.Count == 2)
        {
            Debug.Log("Se reactivaron los jugadores");
            p1.gameObject.SetActive(true);
            p2.gameObject.SetActive(true);
            p1.enabled = true;
            p2.enabled = true;
            p1.Vida = 100;
            p2.Vida = 100;
            p1.anim.SetTrigger("spawn");
            p2.anim.SetTrigger("spawn");
            yield return new WaitForSeconds(time);
            p1.BloquearMovimiento = false;
            p2.BloquearMovimiento = false;
        }

        else if (infoLobbyPlayers.Count == 4)
        {
            Debug.Log("Se reactivaron los jugadores");
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
            yield return new WaitForSeconds(time);
            p1.BloquearMovimiento = false;
            p2.BloquearMovimiento = false;
            p3.BloquearMovimiento = false;
            p4.BloquearMovimiento = false;
        }

        isRunning = true;
        deadEnemy = false;
        remainingTime = totalTime;
        yield return new WaitForSeconds(3.50f);
        InicializarEnemySpawn();
        yield return new WaitForSeconds(6.0f);
    }

    private IEnumerator RespawnearJugadorMDS(PlayerController player, float time)
    {
        Debug.Log("Se respauneo el jugador");
        yield return new WaitForSeconds(time);
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        player.enabled = false;
        yield return new WaitForSeconds(1.5f);
        if (infoLobbyPlayers.Count == 2)
        {
            if (player.equipo == 1)
            {
                if (player == p1)
                {
                    p1.gameObject.SetActive(true);
                    p1.Vida = 100;
                    p1.enabled = true;
                    p1.BloquearMovimiento = false;
                    p1.anim.SetTrigger("spawn");
                    p1.transform.position = modo1v1spawnTeam1.position;
                }
                else
                {
                    p1.gameObject.SetActive(true);
                    p1.Vida = 100;
                    p1.enabled = true;
                    p1.BloquearMovimiento = false;
                    p1.anim.SetTrigger("spawn");
                    p1.transform.position = modo1v1spawnTeam2.position;
                }
            }
            else if (player.equipo == 2)
            {
                if (player == p2)
                {
                    p2.gameObject.SetActive(true);
                    p2.salud = 100;
                    p2.enabled = true;
                    p2.BloquearMovimiento = false;
                    p2.anim.SetTrigger("spawn");
                    p2.transform.position = modo1v1spawnTeam2.position;
                }
                else
                {
                    p2.gameObject.SetActive(true);
                    p2.salud = 100;
                    p2.enabled = true;
                    p2.BloquearMovimiento = false;
                    p2.anim.SetTrigger("spawn");
                    p2.transform.position = modo1v1spawnTeam1.position;
                }
            }
        }
        else if (infoLobbyPlayers.Count == 4)
        {
            if (p1.equipo == 1 && p2.equipo == 1)
            {
                if (player.equipo == p1.equipo)
                {
                    if (p1.equipo == 1)
                    {
                        p1.gameObject.SetActive(true);
                        p1.enabled = true;
                        p1.Vida = 100;
                        p1.anim.SetTrigger("spawn");
                        p1.transform.position = modo2v2spawnTeam1_1.position;
                    }
                    else
                    {
                        p1.gameObject.SetActive(true);
                        p1.enabled = true;
                        p1.Vida = 100;
                        p1.anim.SetTrigger("spawn");
                        p1.transform.position = modo2v2spawnTeam2_1.position;
                    }
                }

                else if (player.equipo == p2.equipo)
                {
                    if (p2.equipo == 1)
                    {
                        p2.gameObject.SetActive(true);
                        p2.enabled = true;
                        p2.Vida = 100;
                        p2.anim.SetTrigger("spawn");
                        p2.transform.position = modo2v2spawnTeam1_2.position;
                    }
                    else if (p2.equipo == 2)
                    {
                        p2.gameObject.SetActive(true);
                        p2.enabled = true;
                        p2.Vida = 100;
                        p2.anim.SetTrigger("spawn");
                        p2.transform.position = modo2v2spawnTeam2_1.position;
                    }

                }
            }

            if (p3.equipo == 2 && p4.equipo == 2)
            {
                if (player.equipo == p3.equipo)
                {
                    if (p3.equipo == 1)
                    {
                        p3.gameObject.SetActive(true);
                        p3.enabled = true;
                        p3.Vida = 100;
                        p3.anim.SetTrigger("spawn");
                        p3.transform.position = modo2v2spawnTeam1_1.position;
                    }
                    else if (p3.equipo == 2)
                    {
                        p3.gameObject.SetActive(true);
                        p3.enabled = true;
                        p3.Vida = 100;
                        p3.anim.SetTrigger("spawn");
                        p3.transform.position = modo2v2spawnTeam2_1.position;
                    }
                }

                else if (player.equipo == p4.equipo)
                {
                    if (p4.equipo == 1)
                    {
                        p4.gameObject.SetActive(true);
                        p4.enabled = true;
                        p4.Vida = 100;
                        p4.anim.SetTrigger("spawn");
                        p4.transform.position = modo2v2spawnTeam1_2.position;
                    }
                    else if (p4.equipo == 2)
                    {
                        p4.gameObject.SetActive(true);
                        p4.enabled = true;
                        p4.Vida = 100;
                        p4.anim.SetTrigger("spawn");
                        p4.transform.position = modo2v2spawnTeam2_2.position;
                    }
                }
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
    #endregion DEAD EVENT PLAYER MHS

    #region SPAWN DE ENEMIGOS
    [Header("Spawn Enemy Core")]
    [SerializeField] private GameObject[] prefabsEnemigos;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemigosMaximosActivos = 4;
    [SerializeField] private float minSpawnTime = 3f; // Tiempo mínimo entre spawns
    [SerializeField] private float maxSpawnTime = 5f; // Tiempo máximo entre spawns
    [SerializeField] private bool deadEnemy = false;

    private List<GameObject> enemigosInstanciados = new List<GameObject>();

    void InicializarEnemySpawn()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME" && !deadEnemy)
        {
            Debug.Log("Se inicio el Spawn de Enemigos");
            print(deadEnemy);

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
        if (enemigosInstanciados.Count >= enemigosMaximosActivos) return; // No spawnear más si se ha alcanzado el límite

        // Seleccionamos un enemigo aleatoriamente y lo agregamos al objeto
        GameObject enemyPrefab = prefabsEnemigos[Random.Range(0, prefabsEnemigos.Length)];

        // Seleccionamos un punto de spawn aleatorio que no esté ocupado
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
            }
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

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
        if (SceneManager.GetActiveScene().name != "ANDYINGAME")
            return;

        CheckAndRemoveDeadEnemies();

        if (enemigosInstanciados.Count < enemigosMaximosActivos)
        {
            // Generar un nuevo enemigo en un punto de spawn aleatorio
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnPointIndex];

            // Seleccionar un enemigo aleatorio del array
            GameObject enemyPrefab = prefabsEnemigos[Random.Range(0, prefabsEnemigos.Length)];

            // Instanciar el enemigo
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemigosInstanciados.Add(newEnemy);
        }

        // Planificar la siguiente llamada a este método
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
