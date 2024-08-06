using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    [Header("Main Menu Core")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelMapasMHS;
    [SerializeField] private GameObject panelMapasMDS;
    [SerializeField] private GameObject panelSetings;
    [SerializeField] private GameObject panelSchemeControl;
    [SerializeField] private GameObject panelConfimarSalir;

    [Header("Rondas MHS")]
    [SerializeField] private TMP_Text rondaActualText;

    [Header("Tiempo MDS")]   //minutos
    [SerializeField] private TMP_Text tiempoActualText;

    //[SerializeField] private GameObject eventSystem;
    //private EventSystem eventS;

    private bool play;

    GameManager gM;

    private void Start()
    {
        if (gM == null)
        {
            gM = FindFirstObjectByType<GameManager>();
        }



        //if (eventSystem == null)
        //{
        //    Debug.LogWarning("Nose encontro el Event System");
        //    eventSystem = GameObject.Find("EventSystem");
        //}
        //else
        //{
        //    Debug.LogWarning("Se encontro el Event System");
        //}

        //eventS = eventSystem.GetComponent<EventSystem>();

        play = false;
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelMapasMHS.SetActive(false);
        panelMapasMDS.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelConfimarSalir.SetActive(false);

        rondaActualText.text = "1";
        tiempoActualText.text = "1:00";
        //eventS.firstSelectedGameObject = bModes;
    }

    private void Update()
    {
        //Update_CheckBooleans();
    }

    //private void Update_CheckBooleans()
    //{
    //    if (SceneManager.GetActiveScene().name != "ANDYMENUTEST")
    //        return;

    //    if (GameManager.modoHS && GameManager.boolMapaStreetMHS)
    //    {
    //        Invoke("CargarLobby", 0.25f);
    //    }
    //    else if (GameManager.modoDS && GameManager.boolMapaStreetMHS)
    //    {
    //        Invoke("CargarLobby", 0.25f);
    //    }
    //}

    public void GoToModos()
    {
        panelModos.SetActive(true);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelMapasMHS.SetActive(false);

        //eventS.firstSelectedGameObject = bHechizos;
    }

    public void Back()
    {
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelMapasMHS.SetActive(false);
        play = false;

        GameManager.Instance.ResetiarVariables();

        rondaActualText.text = "1";
        tiempoActualText.text = "1:00";

        //rondaActualText.text = GameManager.Instance.puntosParaGanar.ToString();
        //eventS.firstSelectedGameObject = bModes;
    }

    public void GoToSettings()
    {
        panelSetings.SetActive(true);
        panelModos.SetActive(false);
        //panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelMapasMHS.SetActive(false);
    }

    public void GoToSchemeControl()
    {
        panelSchemeControl.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        //panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelMapasMHS.SetActive(false);
    }

    public void ModoHechizosSazonados()
    {
        GameManager.modoHS = true;
        GameManager.modoDS = false;
        panelMapasMHS.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelSchemeControl.SetActive(false);
    }

    public void ModoDueloDeSalsas()
    {
        GameManager.modoDS = true;
        GameManager.modoHS = false;
        panelMapasMDS.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelSchemeControl.SetActive(false);
    }

    public void MapaStreetMHS()
    {
        GameManager.boolMapaStreetMHS = true;
        GameManager.boolMapaDungeonMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;
    }

    public void MapaDungeonMHS()
    {
        GameManager.boolMapaDungeonMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;
    }

    public void MapaRestaurantMHS()
    {
        GameManager.boolMapaRestaurantMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaDungeonMHS = false;
        play = true;
    }

    #region CAMBIOS DE RONDA MHS
    
    public void AumentarRondas()
    {
        int ronda = GameManager.Instance.puntosParaGanar;

        if(ronda == 1)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = "2";
        }
        else if(ronda == 2)
        {
            GameManager.Instance.puntosParaGanar = 3;
            rondaActualText.text = "3";
        }
        else if(ronda == 3)
        {
            return;
        }
    }

    public void DisminuirRonda()
    {
        int ronda = GameManager.Instance.puntosParaGanar;

        if(ronda == 3)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = "2";
        }
        else if (ronda == 2)
        {
            GameManager.Instance.puntosParaGanar = 1;
            rondaActualText.text = "1";
        }
        else if(ronda == 1)
        {
            return;
        }
    }
    #endregion CAMBIOS DE RONDA MHS

    #region CAMBIOS DE TIEMPO MDS

    public void AumentarTiempo()
    {
        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 60f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 300f;
            tiempoActualText.text = "5:00";
        }
        else if (tiempo == 300f)
        {
            return;
        }
    }

    public void DisminuirTiempo()
    {
        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 300f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 60f;
            tiempoActualText.text = "1:00";
        }
        else if (tiempo == 60f)
        {
            return;
        }
    }

    #endregion CAMBIOS DE TIEMPO MHS

    public void CargarLobby()
    {
        if (!play) return;

        SceneManager.LoadScene("LOBBY");
    }

    public void ConfirmacionSalir()
    {
        panelConfimarSalir.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelMapasMHS.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
