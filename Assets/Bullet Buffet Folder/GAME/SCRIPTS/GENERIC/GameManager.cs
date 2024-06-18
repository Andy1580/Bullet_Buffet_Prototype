using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        MakeSingleton();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        oneVone = false;
        twoVtwo = false;
        modoHechizos = false;

        Start_Marcador();
        Start_Temporizador();
        Start_Pesonajes();
        Start_Jugadores();
        TestCarga();
    }

    private void Update()
    {
        Update_CheckScene();
        Update_Marcador();
    }

    private void FixedUpdate()
    {
        FixUpdate_Temporizador();
        FixUpdate_CheckScene();
        FixUpdate_Temporizador();
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #region GAME MANAGER
    public static GameManager Instance;

    public static bool oneVone;
    public static bool twoVtwo;
    public static bool modoHechizos;

    PartidaTest partida;

    //En este metodo se pone todo lo que quieras que pase al cargar una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetVariables();
        //Initialize_Spawn();
        FixUpdate_CheckScene();
        Update_Marcador();
    }

    private void ResetVariables()
    {
        //if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        //{
        //    //Booleanos
        //    oneVone = false;
        //    twoVtwo = false;
        //    modoHechizos = false;

        //    //Temporizador
        //    remainingTime = totalTime;
        //    inGame = false;
        //    isRunning = false;
        //}
    }

    private void Update_CheckScene()
    {

        //if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        //{
        //    if (!inGame)
        //    {
        //        inGame = true;
        //    }

        //    if (!isRunning)
        //    {
        //        isRunning = true;
        //    }
        //}

    }

    private void FixUpdate_CheckScene()
    {
        //if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        //{
        //    panelTiempoAgotado.SetActive(false);
        //    panelTemporizador.SetActive(false);
        //    panelMarcador.SetActive(false);
        //    panelVictoria.SetActive(false);

        //}

        //if (SceneManager.GetActiveScene().name == "ANDYCLASH" && inGame)
        //{
        //    if (isRunning)
        //    {
        //        Start_Temporizador();
        //    }

        //    panelMarcador.SetActive(true);
        //}
    }

    public void TestCarga()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count < 2)
        {
            Debug.LogError("Se necesitan al menos 2 gamepads conectados.");
            return;
        }

        //Aqui creamos un nuevo JugadorInfo para cada control conectado
        JugadorInfo jugador1 = new JugadorInfo(gamepads[0].deviceId, "J1");
        JugadorInfo jugador2 = new JugadorInfo(gamepads[1].deviceId, "J2");

        if (jugador2 != null)
        {
            print("Si se asigno al gampad 2" + jugador2);
        }
        else
        {
            print("No se asigno al gampad 2" + jugador2);
        }


        //Se crea una nueva partida de la clase partida y se le agrega el JugadoInfo correspondiente
        PartidaTest partida = new PartidaTest();
        partida.AgregarJugadorAlEquipo1(jugador1);
        partida.AgregarJugadorAlEquipo2(jugador2);


        IniciarPartida(partida);
    }


    public void IniciarPartida(PartidaTest partida)
    {
        this.partida = partida;

        if (equipo1 == null)
            equipo1 = new List<PlayerController>();
        if (equipo2 == null)
            equipo2 = new List<PlayerController>();

        equipo1.Clear();
        equipo2.Clear();

        if (partida.Equipo1.Count == 1)
        {
            p1 = SpawnJugador(partida.Equipo1[0], staticmodo1v1spawn1);
            equipo1.Add(p1);
            p1.equipo = 1;
            jugadores.Add(p1);

            if (partida.Equipo2.Count == 1)
            {
                p2 = SpawnJugador(partida.Equipo2[1], modo1v1spawn2); //No supe corregir este error
                equipo2.Add(p2);
                p2.equipo = 2;
                jugadores.Add(p2);
            }
        }
        else
        {
            p1 = SpawnJugador(partida.Equipo1[0], modo2v2spawn1);
            p2 = SpawnJugador(partida.Equipo1[1], modo2v2spawn2);
            p3 = SpawnJugador(partida.Equipo2[2], modo2v2spawn3);
            p4 = SpawnJugador(partida.Equipo2[3], modo2v2spawn4);

            equipo1.Add(p1);
            equipo1.Add(p2);
            equipo2.Add(p3);
            equipo2.Add(p4);

        }

    }


    public void ResetGame()
    {
        SceneManager.LoadScene("ANDYCLASH");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    #endregion GAME MANAGER

    #region PAUSA
    private static bool enPausa;
    public static bool EnPausa => enPausa;

    public static void Pausa(PlayerController player)
    {
        //Se invierten entre ellos
        enPausa = !enPausa;

        if (enPausa)
        {
            Time.timeScale = 0;

            foreach (PlayerController j in jugadores)
            {
                j.BloquearMovimiento = true;
            }
        }
        else
        {
            Time.timeScale = 1;

            foreach (PlayerController j in jugadores)
            {
                j.BloquearMovimiento = false;
            }

        }

    }
    #endregion PAUSA

    #region MARCADOR

    [Header("Marcador Core")]
    [SerializeField] private GameObject panelMarcador;
    [SerializeField] private static GameObject panelEstaticoVictoria;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private TMP_Text puntajeTeam1;
    [SerializeField] private TMP_Text puntajeTeam2;
    [SerializeField] private TMP_Text playerWinText;
    private static int staticPuntosAGanarTeam1 = 0;
    private static int staticPuntosAGanarTeamr2 = 0;
    private int puntosAGanarTeam1 = 0;
    private int puntosAGanarTeam2 = 0;

    void Start_Marcador()
    {
        panelEstaticoVictoria = panelVictoria;
        panelVictoria.SetActive(false);
        panelMarcador.SetActive(false);
        panelEstaticoVictoria.SetActive(false);

        puntajeTeam1.text = staticPuntosAGanarTeam1.ToString();
        puntajeTeam2.text = staticPuntosAGanarTeamr2.ToString();
    }

    public static void Marcador(PlayerController player)
    {

        panelEstaticoVictoria.SetActive(true);

        foreach (PlayerController j in jugadores)
        {
            if (j.name == "J1")
            {
                if (j.salud <= 0)
                {
                    panelEstaticoVictoria.SetActive(true);
                    staticPuntosAGanarTeamr2++;
                    CambioDeRonda();
                }
            }
            else if (j.name == "J2")
            {
                if (j.salud <= 0)
                {
                    panelEstaticoVictoria.SetActive(true);
                    staticPuntosAGanarTeam1++;
                    CambioDeRonda();
                }
            }
        }
    }

    private static void CambioDeRonda()
    {
        //prefabPlayer1.SetActive(false);
        //prefabPlayer2.SetActive(false);
        //prefabPlayer1.GetComponent<PlayerController>().enabled = false;
        //prefabPlayer2.GetComponent<PlayerController>().enabled = false;



        //prefabPlayer1.transform.position = modo1v1spawn1.localPosition;
        //prefabPlayer2.transform.position = modo1v1spawn2.localPosition;



        //prefabPlayer1.SetActive(true);
        //prefabPlayer2.SetActive(true);
        //prefabPlayer1.GetComponent<PlayerController>().enabled = true;
        //prefabPlayer2.GetComponent<PlayerController>().enabled = true;
        staticPfPersonaje1.gameObject.SetActive(false);
        staticPfPersonaje2.gameObject.SetActive(false);
        p1.enabled = false;
        p2.enabled = false;

        staticPfPersonaje1.transform.position = staticmodo1v1spawn1.transform.position;
        staticPfPersonaje2.transform.position = staticmodo1v1spawn2.transform.position;


    }

    void Update_Marcador()
    {
        if (staticPuntosAGanarTeam1 >= 3)
        {
            panelEstaticoVictoria.SetActive(true);
            playerWinText.text = staticPfPersonaje1.gameObject.name;
        }
        else
        {
            panelEstaticoVictoria.SetActive(true);
            playerWinText.text = staticPfPersonaje2.gameObject.name;

        }


    }
    #endregion MARCADOR

    #region TEMPORIZADOR

    [Header("Temporizador")]
    [SerializeField] private GameObject panelTemporizador;
    [SerializeField] private GameObject panelTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool inGame = false;
    private static float RemainingTime;
    private float remainingTime;
    private bool isRunning = false;

    private void Start_Temporizador()
    {
        panelTemporizador.SetActive(true);
        remainingTime = totalTime;
        Start_TimerText();
    }

    private void FixUpdate_Temporizador()
    {
        if (remainingTime <= 0)
        {
            panelTiempoAgotado.SetActive(true);
            panelTemporizador.SetActive(false);
            panelMarcador.SetActive(false);
            isRunning = false;
            TimerEnded();

            foreach (PlayerController p in jugadores)
            {
                if(p.salud <= 0)
                {
                    //Ir agregando la Logica de la asignacion de puntos
                    if(p.equipo == 1)
                    {
                        panelEstaticoVictoria.SetActive(true);
                    }
                    else if(p.equipo == 2)
                    {

                    }
                    
                    
                }
            }
        }
        else
        {
            remainingTime -= Time.fixedDeltaTime;
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
    [SerializeField] private static PlayerController staticPfPersonaje1;
    [SerializeField] private static PlayerController staticPfPersonaje2;
    [SerializeField] private static PlayerController staticPfPersonaje3;
    [SerializeField] private static PlayerController staticPfPersonaje4;
    [SerializeField] private PlayerController pfPersonaje1;
    [SerializeField] private PlayerController pfPersonaje2;
    [SerializeField] private PlayerController pfPersonaje3;
    [SerializeField] private PlayerController pfPersonaje4;

    void Start_Pesonajes()
    {
        pfPersonaje1 = staticPfPersonaje1;
        pfPersonaje2 = staticPfPersonaje2;
        pfPersonaje3 = staticPfPersonaje3;
        pfPersonaje4 = staticPfPersonaje4;
    }


    #endregion PERSONAJES

    #region JUGADORES

    private static PlayerController prefabJuador;
    private static List<PlayerController> jugadores;

    [Header("Spawn Points")]
    [SerializeField] private Transform modo1v1spawn1;
    [SerializeField] private Transform modo1v1spawn2;
    [SerializeField] private Transform modo2v2spawn1;
    [SerializeField] private Transform modo2v2spawn2;
    [SerializeField] private Transform modo2v2spawn3;
    [SerializeField] private Transform modo2v2spawn4;

    [SerializeField] private static Transform staticmodo1v1spawn1;
    [SerializeField] private static Transform staticmodo1v1spawn2;
    [SerializeField] private static Transform staticmodo2v2spawn1;
    [SerializeField] private static Transform staticmodo2v2spawn2;
    [SerializeField] private static Transform staticmodo2v2spawn3;
    [SerializeField] private static Transform staticmodo2v2spawn4;

    private static List<PlayerController> equipo1;
    private static List<PlayerController> equipo2;

    private static PlayerController p1, p2, p3, p4;

    void Start_Jugadores()
    {
        modo1v1spawn1 = staticmodo1v1spawn1;
    }

    private PlayerController SpawnJugador(JugadorInfo jugadorInfo, Transform spawnPoint)
    {
        PlayerController personaje = null;

        //if (personaje.name == "J2")
        //{
        //    print("Si se asigno el" + personaje + "J2 a " + staticPfPersonaje2);

        //}
        switch (jugadorInfo.Personaje)
        {
            case "J1": personaje = staticPfPersonaje1; break;
            case "J2": personaje = staticPfPersonaje2; break;
            case "J3": personaje = staticPfPersonaje3; break;
            case "J4": personaje = staticPfPersonaje4; break;
            default:
                Debug.LogError("Personaje no reconocido: " + jugadorInfo.Personaje);
                break;
        }

        if (personaje != null)
        {
            return Instantiate(personaje, spawnPoint.position, spawnPoint.rotation);
        }

        return null;
    }


    #endregion JUGADORES
}
