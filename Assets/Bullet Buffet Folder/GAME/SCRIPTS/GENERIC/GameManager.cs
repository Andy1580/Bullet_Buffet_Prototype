using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GAME MANAGER
    public static GameManager Instance;

    public bool oneVone;
    public bool twoVtwo;
    public bool modoHechizos;

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
        Start_Spawn();
    }

    private void Update()
    {
        Update_Temporizador();
        Update_Marcador();
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetVariables();
        Update_CheckScene();
        Start_Spawn();

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
        }
    }

    private void Update_CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            panelTiempoAgotado.SetActive(false);
            panelTemporizador.SetActive(false);
            panelMarcador.SetActive(false);
            panelVictoria.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        {
            Start_Temporizador();
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

    public static void Pausa(PlayerController player)
    {
        //Se invierten entre ellos
        enPausa = !enPausa;

        if (enPausa)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        print(Time.timeScale);
    }
    #endregion PAUSA

    #region MARCADOR
    [SerializeField] private GameObject panelMarcador;
    [SerializeField] private static GameObject panelEstaticoVictoria;
    [SerializeField] private  GameObject panelVictoria;
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



        //prefabPlayer1.transform.position = respawnPoint1.localPosition;
        //prefabPlayer2.transform.position = respawnPoint2.localPosition;



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
    [Header("Jugadores")]
    [SerializeField] private GameObject prefabPlayer1;
    [SerializeField] private GameObject prefabPlayer2;
    [SerializeField] private GameObject prefabPlayer3;
    [SerializeField] private GameObject prefabPlayer4;

    [Header("Spawn Points")]
    [SerializeField] private Transform respawnPoint1;
    [SerializeField] private Transform respawnPoint2;
    [SerializeField] private Transform respawnPoint3;
    [SerializeField] private Transform respawnPoint4;
    [SerializeField] private Transform respawnPoint5;
    [SerializeField] private Transform respawnPoint6;

    void Start_Spawn()
    {
        if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        {
            Debug.Log("Si entro el start spawn");
            if (oneVone == true)
            {
                Instantiate(prefabPlayer1, respawnPoint1.localPosition, Quaternion.identity);
                Instantiate(prefabPlayer2, respawnPoint2.localPosition, Quaternion.identity);
            }
            else if (twoVtwo == true)
            {
                Instantiate(prefabPlayer1, respawnPoint3.localPosition, Quaternion.identity);
                Instantiate(prefabPlayer2, respawnPoint4.localPosition, Quaternion.identity);
                Instantiate(prefabPlayer3, respawnPoint5.localPosition, Quaternion.identity);
                Instantiate(prefabPlayer4, respawnPoint6.localPosition, Quaternion.identity);
            }
        }
    }
    #endregion SPAWN

    #region TEMPORIZADOR

    [SerializeField] private GameObject panelTemporizador;
    [SerializeField] private GameObject panelTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    private float remainingTime;
    private bool isRunning = false;

    private void Start_Temporizador()
    {
        panelTemporizador.SetActive(true);
        isRunning = true;
        remainingTime = totalTime;
        Start_TimerText();
        StartCoroutine(Countdown());
    }

    private void Update_Temporizador()
    {
        if (remainingTime <= 0)
        {
            panelTiempoAgotado.SetActive(true);
            panelTemporizador.SetActive(false);
            panelMarcador.SetActive(false);
        }
    }

    private IEnumerator Countdown()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            Start_TimerText();
        }
        isRunning = false;
        TimerEnded();
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

}
