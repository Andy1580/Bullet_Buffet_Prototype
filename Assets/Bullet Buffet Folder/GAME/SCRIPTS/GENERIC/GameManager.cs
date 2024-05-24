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

        Start_Temporizador();

    }

    private void Update()
    {
        Update_Temporizador();
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
    }

    void ResetVariables()
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

    public void ResetGame()
    {
        SceneManager.LoadScene("ANDYCLASH");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    #endregion GAME MANAGER

    #region TEMPORIZADOR

    [SerializeField] private GameObject canvasTemporizador;
    [SerializeField] private GameObject canvasTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    private float remainingTime;
    private bool isRunning;

    private void Start_Temporizador()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            canvasTiempoAgotado.SetActive(false);
            canvasTemporizador.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "ANDYCLASH")
        {
            canvasTemporizador.SetActive(true);
            isRunning = true;
            remainingTime = totalTime;
            Start_TimerText();
            StartCoroutine(Countdown());
        }

    }

    private void Update_Temporizador()
    {
        if (remainingTime <= 0)
        {
            canvasTiempoAgotado.SetActive(true);
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
