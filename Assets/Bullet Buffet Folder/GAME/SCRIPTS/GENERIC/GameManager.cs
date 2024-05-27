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
            canvasTiempoAgotado.SetActive(false);
            canvasTemporizador.SetActive(false);
            canvasMarcador.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        {
            Start_Temporizador();
            Start_Marcador();
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

        if(enPausa)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion PAUSA

    #region MARCADOR
    [SerializeField] private GameObject canvasMarcador;
    [SerializeField] private GameObject canvasVictoria;
    private static int puntosAGanarPlayer1 = 0;
    private static int puntosAGanarPlayer2 = 0;
    [SerializeField] private TMP_Text puntajePlayer1;
    [SerializeField] private TMP_Text puntajePlayer2;

    void Start_Marcador()
    {
        canvasMarcador.SetActive(true);
        canvasVictoria.SetActive(false);

        puntajePlayer1.text = puntosAGanarPlayer1.ToString();
    }

    public static void Marcador(PlayerController player)
    {
        //Preguntar al profe por otra opcion porque no me deja usar el Invoke ó una Corutina
        if(player.gameObject.tag == "Player1")
        {
            puntosAGanarPlayer1++;
        }
    }

    private void CambioDeRonda()
    {
        prefabPlayer1.GetComponent<PlayerController>().enabled = false;
        prefabPlayer2.GetComponent<PlayerController>().enabled = false;
        
    }

    void Update_Marcador()
    {
        if(puntosAGanarPlayer1 >= 3)
        {
            canvasVictoria.SetActive(true);
        }

    }
    #endregion MARCADOR

    #region SPAWN
    [Header("Jugadores")]
    [SerializeField] private static GameObject prefabPlayer1;
    [SerializeField] private static GameObject prefabPlayer2;
    [SerializeField] private static GameObject prefabPlayer3;
    [SerializeField] private GameObject prefabPlayer4;

    [Header("Spawn Points")]
    [SerializeField] private static Transform respawnPoint1;
    [SerializeField] private static Transform respawnPoint2;
    [SerializeField] private static Transform respawnPoint3;
    [SerializeField] private static Transform respawnPoint4;
    [SerializeField] private static Transform respawnPoint5;
    [SerializeField] private static Transform respawnPoint6;

    void Start_Spawn()
    {
        if(SceneManager.GetActiveScene().name == "ANDYCLASH")
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

    [SerializeField] private GameObject canvasTemporizador;
    [SerializeField] private GameObject canvasTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    private float remainingTime;
    private bool isRunning = false;

    private void Start_Temporizador()
    {
        canvasTemporizador.SetActive(true);
        isRunning = true;
        remainingTime = totalTime;
        Start_TimerText();
        StartCoroutine(Countdown());
    }

    private void Update_Temporizador()
    {
        if (remainingTime <= 0)
        {
            canvasTiempoAgotado.SetActive(true);
            canvasTemporizador.SetActive(false);
            canvasMarcador.SetActive(false);
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
