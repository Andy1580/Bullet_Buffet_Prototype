using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            //Booleanos
            oneVone = false;
            twoVtwo = false;
            modoHechizos = false;

            //Temporizador
            remainingTime = totalTime;
            inGame = false;
            isRunning = false;
        }
    }

    private void Update_CheckScene()
    {

        if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        {
            if (!inGame)
            {
                inGame = true;
            }

            if (!isRunning)
            {
                isRunning = true;
            }
        }

    }

    private void FixUpdate_CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            panelTiempoAgotado.SetActive(false);
            panelTemporizador.SetActive(false);
            panelMarcador.SetActive(false);
            panelVictoria.SetActive(false);

        }

        if (SceneManager.GetActiveScene().name == "ANDYCLASH" && inGame)
        {
            if (isRunning)
            {
                Start_Temporizador();
            }

            panelMarcador.SetActive(true);
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

    //public static void Pausa(PlayerController player)
    //{
    //    //Se invierten entre ellos
    //    enPausa = !enPausa;

    //    if (enPausa)
    //    {
    //        Time.timeScale = 0;

    //        foreach (PlayerController j in jugadores)
    //        {
    //            j.BloquearMovimiento = true;
    //        }
    //    }
    //    else
    //    {
    //        Time.timeScale = 1;

    //        foreach (PlayerController j in jugadores)
    //        {
    //            j.BloquearMovimiento= false;
    //        }

    //    }

    //}
    #endregion PAUSA

    #region MARCADOR
    [SerializeField] private GameObject panelMarcador;
    [SerializeField] private static GameObject panelEstaticoVictoria;
    [SerializeField] private GameObject panelVictoria;
    private int puntosAGanarPlayer1 = 0;
    private int puntosAGanarPlayer2 = 0;
    //[SerializeField] private TMP_Text puntajePlayer1;
    //[SerializeField] private TMP_Text puntajePlayer2;
    //[SerializeField] private TMP_Text playerWinText;

    void Start_Marcador()
    {
        panelEstaticoVictoria = panelVictoria;
        panelVictoria.SetActive(false);
        panelMarcador.SetActive(false);
        //panelEstaticoVictoria.SetActive(false);

        //puntajePlayer1.text = puntosAGanarPlayer1.ToString();
        //puntajePlayer2.text = puntosAGanarPlayer2.ToString();
    }

    public static void Marcador(PlayerController player)
    {


        panelEstaticoVictoria.SetActive(true);

        ////Preguntar al profe por otra opcion porque no me deja usar el Invoke ó una Corutina
        //if (player.gameObject.tag == "Player1")
        //{
        //    puntosAGanarPlayer2++;
        //    CambioDeRonda1v1();
        //}

        //else if (player.gameObject.tag == "Player2")
        //{
        //    puntosAGanarPlayer1++;
        //    CambioDeRonda1v1();
        //}
    }

    private static void CambioDeRonda1v1()
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



    }

    void Update_Marcador()
    {
        //if (puntosAGanarPlayer1 >= 3)
        //{
        //    panelEstaticoVictoria.SetActive(true);
        //    playerWinText.text = prefabPlayer1.gameObject.name;
        //}

        //else if (puntosAGanarPlayer2 >= 3)
        //{

        //}

    }
    #endregion MARCADOR

    #region SPAWN
    

    [Header("Spawn Points")]
    [SerializeField] private Transform modo1v1spawn1;
    [SerializeField] private Transform modo1v1spawn2;
    [SerializeField] private Transform modo2v2spawn1;
    [SerializeField] private Transform modo2v2spawn2;
    [SerializeField] private Transform modo2v2spawn3;
    [SerializeField] private Transform modo2v2spawn4;

    //private void Initialize_Spawn()
    //{
    //    if (SceneManager.GetActiveScene().name == "ANDYCLASH")
    //    {
    //        if (oneVone == true)
    //        {


    //            j1 = Instantiate(pfJugador, modo1v1spawn1.localPosition, Quaternion.identity);
    //            PlayerController j2 = Instantiate(pfJugador, modo1v1spawn2.localPosition, Quaternion.identity);
    //        }
    //        else if (twoVtwo == true)
    //        {
    //            PlayerController j1 = Instantiate(pfJugador, modo2v2spawn1.localPosition, Quaternion.identity);
    //            PlayerController j2 = Instantiate(pfJugador, modo2v2spawn2.localPosition, Quaternion.identity);
    //            PlayerController j3 = Instantiate(pfJugador, modo2v2spawn3.localPosition, Quaternion.identity);
    //            PlayerController j4 = Instantiate(pfJugador, modo2v2spawn4.localPosition, Quaternion.identity);
    //        }
    //    }
    //}
    #endregion SPAWN

    #region TEMPORIZADOR

    [Header("Temporizador")]
    [SerializeField] private GameObject panelTemporizador;
    [SerializeField] private GameObject panelTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool inGame = false;
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

    #region JUGADORES
    //public static PlayerController pfJugador;
    //public static List<PlayerController> jugadores;

    //void Start_Jugadores()
    //{
    //    jugadores = new List<PlayerController>();
    //}

    //public static void RegistrarJugadores(Lobby lobby)
    //{
    //    jugadores.Add(j1);
    //}


    #endregion JUGADORES
}
