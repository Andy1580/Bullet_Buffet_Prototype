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

    [Header("Botones de rondas MHS")]
    [SerializeField] private GameObject botonRonda1a2;
    [SerializeField] private GameObject botonRonda2a3;
    [SerializeField] private GameObject botonRonda3a2;
    [SerializeField] private GameObject botonRonda2a1;
    [SerializeField] private TMP_Text rondaActualText;

    [Header("Botonos de tiempo MHS")]   //minutos
    [SerializeField] private GameObject botonTiempo2a3;
    [SerializeField] private GameObject botonTiempo3a4;
    [SerializeField] private GameObject botonTiempo4a5;
    [SerializeField] private GameObject botonTiempo5a4;
    [SerializeField] private GameObject botonTiempo4a3;
    [SerializeField] private GameObject botonTiempo3a2;
    [SerializeField] private TMP_Text tiempoActualText;

    //[SerializeField] private GameObject eventSystem;
    //private EventSystem eventS;

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

        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelMapasMHS.SetActive(false);
        panelMapasMDS.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelConfimarSalir.SetActive(false);
        botonRonda1a2.SetActive(true);
        botonRonda2a1.SetActive(true);
        botonRonda2a3.SetActive(false);
        botonRonda3a2.SetActive(false);

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

        botonRonda1a2.SetActive(true);
        botonRonda2a1.SetActive(true);
        botonRonda2a3.SetActive(false);
        botonRonda3a2.SetActive(false);

        GameManager.Instance.ResetiarVariables();

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
    }

    public void MapaDungeonMHS()
    {
        GameManager.boolMapaDungeonMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
    }

    public void MapaRestaurantMHS()
    {
        GameManager.boolMapaRestaurantMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaDungeonMHS = false;
    }

    public void CambiarNumeroDeRonda1a2()
    {
        if (GameManager.Instance.puntosParaGanar == 1)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = GameManager.Instance.puntosParaGanar.ToString();
            botonRonda1a2.SetActive(false);
            botonRonda2a3.SetActive(true);
            botonRonda2a1.SetActive(true);
        }
    }

    #region CAMBIOS DE RONDA MHS
    
    public void CambiarNumeroDeRonda2a3()
    {
        if (GameManager.Instance.puntosParaGanar == 2)
        {
            GameManager.Instance.puntosParaGanar = 3;
            rondaActualText.text = GameManager.Instance.puntosParaGanar.ToString();
            botonRonda2a3.SetActive(false);
            botonRonda3a2.SetActive(true);
        }
    }

    public void CambiarNumeroDeRonda3a2()
    {
        if (GameManager.Instance.puntosParaGanar == 3)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = GameManager.Instance.puntosParaGanar.ToString();
            botonRonda2a3.SetActive(true);
            botonRonda2a1.SetActive(true);
            botonRonda3a2.SetActive(false);
        }
    }

    public void CambiarNumeroDeRonda2a1()
    {
        if (GameManager.Instance.puntosParaGanar == 2)
        {
            GameManager.Instance.puntosParaGanar = 1;
            rondaActualText.text = GameManager.Instance.puntosParaGanar.ToString();
            botonRonda1a2.SetActive(true);
            botonRonda2a1.SetActive(true);
            botonRonda2a3.SetActive(false);
        }
    }

    #endregion CAMBIOS DE RONDA MHS

    #region CAMBIOS DE TIEMPO MHS

    public void CambiarTiempo1a2()
    {
        GameManager.Instance.totalTime = 120f;
        tiempoActualText.text = "2:00";
    }

    public void CambiarTiempo2a3()
    {
        GameManager.Instance.totalTime = 180f;
        tiempoActualText.text = "3:00";
    }

    public void CambiarTiempo3a4()
    {
        GameManager.Instance.totalTime = 240f;
        tiempoActualText.text = "4:00";
    }

    public void CambiarTiempo4a5()
    {
        GameManager.Instance.totalTime = 300f;
        tiempoActualText.text = "5:00";
    }

    public void CambiarTiempo5a4()
    {
        GameManager.Instance.totalTime = 240f;
        tiempoActualText.text = "4:00";
    }

    public void CambiarTiempo4a3()
    {
        GameManager.Instance.totalTime = 180f;
        tiempoActualText.text = "3:00";
    }

    public void CambiarTiempo3a2()
    {
        GameManager.Instance.totalTime = 120f;
        tiempoActualText.text = "2:00";
    }

    public void CambiarTiempo2a1()
    {
        GameManager.Instance.totalTime = 60f;
        tiempoActualText.text = "1:00";
    }

    #endregion CAMBIOS DE TIEMPO MHS

    public void CargarLobby()
    {
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
