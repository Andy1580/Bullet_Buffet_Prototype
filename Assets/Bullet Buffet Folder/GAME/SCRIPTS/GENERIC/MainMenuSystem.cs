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
    [SerializeField] private TMP_Text tiempoActualMDSText;
    [SerializeField] private TMP_Text tiempoActualMHSText;

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
        tiempoActualMDSText.text = "1:00";
        tiempoActualMHSText.text = "1:00";
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

        //AudioManager.instance.PlaySound("");
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
        panelMapasMDS.SetActive(false);
        play = false;

        GameManager.Instance.ResetiarVariables();

        rondaActualText.text = "1";
        tiempoActualMDSText.text = "1:00";
        tiempoActualMHSText.text = "1:00";

        //AudioManager.instance.PlaySound("");

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

        //AudioManager.instance.PlaySound("");
    }

    public void GoToSchemeControl()
    {
        panelSchemeControl.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        //panelInicio.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelMapasMHS.SetActive(false);

        //AudioManager.instance.PlaySound("");
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

        //AudioManager.instance.PlaySound("");
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

        //AudioManager.instance.PlaySound("");
    }

    public void MapaStreetMHS()
    {
        GameManager.boolMapaStreetMHS = true;
        GameManager.boolMapaDungeonMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    public void MapaDungeonMHS()
    {
        GameManager.boolMapaDungeonMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    public void MapaRestaurantMHS()
    {
        GameManager.boolMapaRestaurantMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaDungeonMHS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    public void MapaStreetMDS()
    {
        GameManager.boolMapaStreetMDS = true;
        GameManager.boolMapaDungeonMDS = false;
        GameManager.boolMapaRestaurantMDS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    public void MapaDungeonMDS()
    {
        GameManager.boolMapaDungeonMDS = true;
        GameManager.boolMapaStreetMDS = false;
        GameManager.boolMapaRestaurantMDS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    public void MapaRestaurantMDS()
    {
        GameManager.boolMapaRestaurantMDS = true;
        GameManager.boolMapaStreetMDS = false;
        GameManager.boolMapaDungeonMDS = false;
        play = true;

        //AudioManager.instance.PlaySound("");
    }

    #region CAMBIOS DE RONDA

    public void AumentarRondas()
    {
        //AudioManager.instance.PlaySound("");

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
            GameManager.Instance.puntosParaGanar = 4;
            rondaActualText.text = "4";
        }
        else if (ronda == 4)
        {
            GameManager.Instance.puntosParaGanar = 5;
            rondaActualText.text = "5";
        }
        else if (ronda == 5)
        {
            GameManager.Instance.puntosParaGanar = 6;
            rondaActualText.text = "6";
        }
        else if (ronda == 6)
        {
            GameManager.Instance.puntosParaGanar = 7;
            rondaActualText.text = "7";
        }
        else if (ronda == 7)
        {
            GameManager.Instance.puntosParaGanar = 8;
            rondaActualText.text = "8";
        }
        else if (ronda == 8)
        {
            GameManager.Instance.puntosParaGanar = 9;
            rondaActualText.text = "9";
        }
        else if (ronda == 9)
        {
            GameManager.Instance.puntosParaGanar = 10;
            rondaActualText.text = "10";
        }
        else if(ronda == 10)
        {
            return;
        }
    }

    public void DisminuirRonda()
    {
        //AudioManager.instance.PlaySound("");

        int ronda = GameManager.Instance.puntosParaGanar;

        if(ronda == 10)
        {
            GameManager.Instance.puntosParaGanar = 9;
            rondaActualText.text = "9";
        }
        else if (ronda == 9)
        {
            GameManager.Instance.puntosParaGanar = 8;
            rondaActualText.text = "8";
        }
        else if(ronda == 8)
        {
            GameManager.Instance.puntosParaGanar = 7;
            rondaActualText.text = "7";
        }
        else if (ronda == 7)
        {
            GameManager.Instance.puntosParaGanar = 6;
            rondaActualText.text = "6";
        }
        else if (ronda == 6)
        {
            GameManager.Instance.puntosParaGanar = 5;
            rondaActualText.text = "5";
        }
        else if (ronda == 5)
        {
            GameManager.Instance.puntosParaGanar = 4;
            rondaActualText.text = "4";
        }
        else if (ronda == 4)
        {
            GameManager.Instance.puntosParaGanar = 3;
            rondaActualText.text = "3";
        }
        else if (ronda == 3)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = "2";
        }
        else if (ronda == 2)
        {
            GameManager.Instance.puntosParaGanar = 1;
            rondaActualText.text = "1";
        }
        else if (ronda == 1)
        {
            return;
        }
    }
    #endregion CAMBIOS DE RONDA

    #region CAMBIOS DE TIEMPO

    public void AumentarTiempo()
    {
        //AudioManager.instance.PlaySound("");

        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 60f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualMDSText.text = "1:30";
            tiempoActualMHSText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualMDSText.text = "2:00";
            tiempoActualMHSText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualMDSText.text = "2:30";
            tiempoActualMHSText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualMDSText.text = "3:00";
            tiempoActualMHSText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualMDSText.text = "3:30";
            tiempoActualMHSText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualMDSText.text = "4:00";
            tiempoActualMHSText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualMDSText.text = "4:30";
            tiempoActualMHSText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 300f;
            tiempoActualMDSText.text = "5:00";
            tiempoActualMHSText.text = "5:00";
        }
        else if (tiempo == 300f)
        {
            return;
        }
    }

    public void DisminuirTiempo()
    {
        //AudioManager.instance.PlaySound("");

        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 300f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualMDSText.text = "4:30";
            tiempoActualMHSText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualMDSText.text = "4:00";
            tiempoActualMHSText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualMDSText.text = "3:30";
            tiempoActualMHSText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualMDSText.text = "3:00";
            tiempoActualMHSText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualMDSText.text = "2:30";
            tiempoActualMHSText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualMDSText.text = "2:00";
            tiempoActualMHSText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualMDSText.text = "1:30";
            tiempoActualMHSText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 60f;
            tiempoActualMDSText.text = "1:00";
            tiempoActualMHSText.text = "1:00";
        }
        else if (tiempo == 60f)
        {
            return;
        }
    }

    #endregion CAMBIOS DE TIEMPO

    public void CargarLobby()
    {
        if (!play) return;
        //AudioManager.instance.PlaySound("");
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

        //AudioManager.instance.PlaySound("");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
