using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem instance;


    [Header("Main Menu Core")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelMapasMHS;
    [SerializeField] private GameObject panelMapasMDS;
    [SerializeField] private GameObject panelSchemeControl;
    [SerializeField] private GameObject panelCreditos;
    [SerializeField] private GameObject panelConfimarSalir;
    [SerializeField] private GameObject panelComoJugarHS;
    [SerializeField] private GameObject panelComoJugarDS;

    [Header("Imagenes de Mapas")]
    [SerializeField] private GameObject cafeMHS;
    [SerializeField] private GameObject ciudadMHS;
    [SerializeField] private GameObject dungeonMHS;
    [SerializeField] private GameObject cafeMDS;
    [SerializeField] private GameObject ciudadMDS;
    [SerializeField] private GameObject dungeonMDS;

    [Header("Rondas MHS")]
    [SerializeField] private TMP_Text rondaActualText;

    [Header("Tiempo MDS")]   //minutos
    [SerializeField] private TMP_Text tiempoActualMDSText;

    [SerializeField] private Button botonJugarMenu;
    [SerializeField] private Button botonInicio;
    [SerializeField] private Button botonModos;
    [SerializeField] private Button botonConfiguracion;
    [SerializeField] private Button botonSalir;
    [SerializeField] private Button botonControles;
    [SerializeField] private Button botonCreditos;
    [SerializeField] private Button botonEmpezarPartidaMHS;
    [SerializeField] private Button botonEmpezarPartidaMDS;
    [SerializeField] private Toggle togglePantallaCompleta;
    [SerializeField] private Button botonMHS;
    [SerializeField] private Button botonMDS;
    [SerializeField] private Button botonMHSCalle;
    [SerializeField] private Button botonMDSCalle;
    [SerializeField] private Button botonComoJugarHS;
    [SerializeField] private Button botonComoJugarDS;

    private bool play;

    public static GameObject ultimoBotonSeleccionadoStatic;
    private GameObject ultimoBotonSeleccionado;

    private static bool panelInicioActivado = false;
    private static bool panelMapasMHSStatic = false;
    private static bool panelMapasMDSStatic = false;

    private void Awake()
    {
        panelMenuPrincipal.SetActive(true);
        instance = this;
    }

    private void Start()
    {
        if (panelInicioActivado)
        {
            panelInicio.SetActive(false);
            EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
            ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
        }
        else
        {
            panelInicio.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonInicio.gameObject);
            ultimoBotonSeleccionadoStatic = botonInicio.gameObject;
        }

        if (panelMapasMHSStatic)
        {
            panelMapasMHS.SetActive(true);
            panelMapasMDS.SetActive(false);
            GameManager.modoHS = true;
            GameManager.modoDS = false;
            EventSystem.current.SetSelectedGameObject(botonMHSCalle.gameObject);
            ultimoBotonSeleccionadoStatic = botonMHSCalle.gameObject;
        }
        else if (panelMapasMDSStatic)
        {
            panelMapasMDS.SetActive(true);
            panelMapasMHS.SetActive(false);
            GameManager.modoDS = true;
            GameManager.modoHS = false;
            EventSystem.current.SetSelectedGameObject(botonMDSCalle.gameObject);
            ultimoBotonSeleccionadoStatic = botonMDSCalle.gameObject;
        }
        //else
        //{
        //    panelMapasMHS.SetActive(false);
        //    panelMapasMDS.SetActive(false);
        //    GameManager.modoHS = false;
        //    GameManager.modoDS = false;
        //    EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
        //    ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
        //}

        Debug.Log("Se ejecuto otra vez el Start del: " + this.gameObject.name);

        ultimoBotonSeleccionado = ultimoBotonSeleccionadoStatic;
        play = false;
        ResetAllPanels();
        SetMenuNavigation();
    }

    private void OnEnable()
    {
        SetMenuNavigation();
    }

    public void ResetarBooleanosImportantesMS()
    {
        panelMapasMHSStatic = false;
        panelMapasMDSStatic = false;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Recuperar el último botón seleccionado en el menú
            if (!GameManager.Instance.inGame && ultimoBotonSeleccionadoStatic != null)
            {
                EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
            }
        }
    }

    private void SetMenuNavigation()
    {
        // Conectar OnSubmit y OnCancel del InputManager a la función de retroceso
        InputManager.Instance.SetActivePanel(null, Back);
    }

    private void ResetAllPanels()
    {
        panelModos.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelCreditos.SetActive(false);
        panelComoJugarHS.SetActive(false);
        panelComoJugarDS.SetActive(false);

        cafeMHS.SetActive(false);
        ciudadMHS.SetActive(false);
        dungeonMHS.SetActive(false);
        cafeMDS.SetActive(false);
        ciudadMDS.SetActive(false);
        dungeonMDS.SetActive(false);

        rondaActualText.text = "1";
        tiempoActualMDSText.text = "1:00";
    }

    public void ResetearInput()
    {
        InputManager.Instance.SetActivePanel(null, Back);
    }

    public void GoToMenu()
    {
        Invoke("DesactivarPanelInicio", 0.25f);
    }

    void DesactivarPanelInicio()
    {
        panelInicio.SetActive(false);
        panelInicioActivado = true;
        EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
        ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
    }

    public void GoToModos()
    {
        ResetAllPanels();
        panelModos.SetActive(true);
        EventSystem.current.SetSelectedGameObject(botonModos.gameObject);
        AudioManager.instance.PlaySound("botonmenu");
        ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
    }

    public void Back()
    {

        if (panelModos.activeSelf)
        {
            panelMenuPrincipal.SetActive(true);
            panelModos.SetActive(false);
            EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
            ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
            // Restaura el comportamiento normal de Submit en el menú principal
            InputManager.Instance.SetActivePanel(null, Back);
        }
        else if (panelMapasMHS.activeSelf)
        {
            panelComoJugarHS.SetActive(true);
            panelMapasMHS.SetActive(false);
            panelMapasMHSStatic = false;
            print(panelMapasMHSStatic);
            EventSystem.current.SetSelectedGameObject(botonComoJugarHS.gameObject);
            ultimoBotonSeleccionadoStatic = botonComoJugarHS.gameObject;
            GameManager.Instance.ResetarBooleanosImportantesGM();
        }
        else if (panelMapasMDS.activeSelf)
        {
            panelComoJugarDS.SetActive(true);
            panelMapasMDS.SetActive(false);
            panelMapasMDSStatic = false;
            print(panelMapasMDSStatic);
            EventSystem.current.SetSelectedGameObject(botonComoJugarDS.gameObject);
            ultimoBotonSeleccionadoStatic = botonComoJugarDS.gameObject;
            GameManager.Instance.ResetarBooleanosImportantesGM();
        }
        else if (panelSchemeControl.activeSelf)
        {
            panelMenuPrincipal.SetActive(true);
            panelSchemeControl.SetActive(false);
            EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
            // Restaura el comportamiento normal de Submit en el menú principal
            InputManager.Instance.SetActivePanel(null, Back);
        }
        else if (panelConfimarSalir.activeSelf)
        {
            // Cierra el panel de confirmación y vuelve al menú principal
            panelConfimarSalir.SetActive(false);
            panelMenuPrincipal.SetActive(true);
            EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
            // Restaura el comportamiento normal de Submit en el menú principal
            InputManager.Instance.SetActivePanel(null, Back);
        }
        else if (panelCreditos.activeSelf)
        {
            panelCreditos.SetActive(false);
            panelMenuPrincipal.SetActive(true);
            EventSystem.current.SetSelectedGameObject(ultimoBotonSeleccionadoStatic);
        }
        else if(panelComoJugarHS.activeSelf)
        {
            panelComoJugarHS.SetActive(false);
            panelModos.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonMHS.gameObject);
            ultimoBotonSeleccionadoStatic = botonMHS.gameObject;
        }
        else if(panelComoJugarDS.activeSelf)
        {
            panelComoJugarDS.SetActive(false);
            panelModos.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonMDS.gameObject);
            ultimoBotonSeleccionadoStatic = botonMDS.gameObject;
        }

        play = false;

        GameManager.Instance.ResetiarVariables();
        AudioManager.instance.PlaySound("botonBack");
    }

    public void GoToSettings()
    {
        ResetAllPanels();
        InputManager.Instance.SetActivePanel(null, null);
        ConfiguracionManager.Instance.PrenderConfiguracion();
        ConfiguracionManager.Instance.ResetarInputCM();
        ultimoBotonSeleccionadoStatic = botonConfiguracion.gameObject;
        AudioManager.instance.PlaySound("botonmenu");
    }

    public void GoToSchemeControl()
    {
        ResetAllPanels();
        panelSchemeControl.SetActive(true);
        ultimoBotonSeleccionadoStatic = botonControles.gameObject;
        AudioManager.instance.PlaySound("botonmenu");


    }

    public void GoToCreditos()
    {
        ResetAllPanels();
        panelCreditos.SetActive(true);
        AudioManager.instance.PlaySound("botonmenu");
        ultimoBotonSeleccionadoStatic = botonCreditos.gameObject;
        // Restaura el comportamiento normal de Submit en el menú principal
        InputManager.Instance.SetActivePanel(null, Back);
    }

    public void ModoHechizosSazonados()
    {
        GameManager.modoHS = true;
        GameManager.modoDS = false;
        ResetAllPanels();
        panelMapasMHS.SetActive(true);
        panelMapasMHSStatic = true;
        panelMapasMDSStatic = false;
        print(panelMapasMHSStatic);
        AudioManager.instance.PlaySound("botonmenu");

        EventSystem.current.SetSelectedGameObject(botonMHSCalle.gameObject);
        ultimoBotonSeleccionadoStatic = botonMHS.gameObject;
    }

    public void ModoDueloDeSalsas()
    {
        GameManager.modoDS = true;
        GameManager.modoHS = false;
        ResetAllPanels();
        panelMapasMDS.SetActive(true);
        panelMapasMDSStatic = true;
        panelMapasMHSStatic = false;
        print(panelMapasMDSStatic);
        AudioManager.instance.PlaySound("botonmenu");

        EventSystem.current.SetSelectedGameObject(botonMDSCalle.gameObject);
        ultimoBotonSeleccionadoStatic = botonMDS.gameObject;
    }

    public void ComoJugarHS()
    {
        panelComoJugarHS.SetActive(true);
        panelModos.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonComoJugarHS.gameObject);
        AudioManager.instance.PlaySound("botonmenu");
        ultimoBotonSeleccionadoStatic = botonComoJugarHS.gameObject;
    }

    public void ComoJugarDS()
    {
        panelComoJugarDS.SetActive(true);
        panelModos.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonComoJugarDS.gameObject);
        AudioManager.instance.PlaySound("botonmenu");
        ultimoBotonSeleccionadoStatic = botonComoJugarDS.gameObject;
    }

    public void ConfirmacionSalir()
    {
        ResetAllPanels();
        panelConfimarSalir.SetActive(true);
        AudioManager.instance.PlaySound("botonmenu");

        // Restaura el comportamiento normal de Submit en el menú principal
        InputManager.Instance.SetActivePanel(Salir, Back);

        EventSystem.current.SetSelectedGameObject(null);
        ultimoBotonSeleccionadoStatic = botonSalir.gameObject;
    }

    public void Salir()
    {
        Debug.Log("Se cerro la aplicacion");
        Application.Quit();
    }

    public void MapaStreetMHS()
    {
        GameManager.boolMapaStreetMHS = true;
        GameManager.boolMapaDungeonMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        ciudadMHS.SetActive(true);
        cafeMHS.SetActive(false);
        dungeonMHS.SetActive(false);
    }

    public void MapaDungeonMHS()
    {
        GameManager.boolMapaDungeonMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaRestaurantMHS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        dungeonMHS.SetActive(true);
        cafeMHS.SetActive(false);
        ciudadMHS.SetActive(false);
    }

    public void MapaRestaurantMHS()
    {
        GameManager.boolMapaRestaurantMHS = true;
        GameManager.boolMapaStreetMHS = false;
        GameManager.boolMapaDungeonMHS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        cafeMHS.SetActive(true);
        ciudadMHS.SetActive(false);
        dungeonMHS.SetActive(false);
    }

    public void MapaStreetMDS()
    {
        GameManager.boolMapaStreetMDS = true;
        GameManager.boolMapaDungeonMDS = false;
        GameManager.boolMapaRestaurantMDS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        ciudadMDS.SetActive(true);
        cafeMDS.SetActive(false);
        dungeonMDS.SetActive(false);
    }

    public void MapaDungeonMDS()
    {
        GameManager.boolMapaDungeonMDS = true;
        GameManager.boolMapaStreetMDS = false;
        GameManager.boolMapaRestaurantMDS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        dungeonMDS.SetActive(true);
        cafeMDS.SetActive(false);
        ciudadMDS.SetActive(false);
    }

    public void MapaRestaurantMDS()
    {
        GameManager.boolMapaRestaurantMDS = true;
        GameManager.boolMapaStreetMDS = false;
        GameManager.boolMapaDungeonMDS = false;
        play = true;

        AudioManager.instance.PlaySound("botonmenu");

        cafeMDS.SetActive(true);
        ciudadMDS.SetActive(false);
        dungeonMDS.SetActive(false);
    }

    #region CAMBIOS DE RONDA

    public void AumentarRondas()
    {
        AudioManager.instance.PlaySound("botonmenu");

        int ronda = GameManager.Instance.puntosParaGanar;

        if (ronda == 1)
        {
            GameManager.Instance.puntosParaGanar = 2;
            rondaActualText.text = "2";
        }
        else if (ronda == 2)
        {
            GameManager.Instance.puntosParaGanar = 3;
            rondaActualText.text = "3";
        }
        else if (ronda == 3)
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
        else if (ronda == 10)
        {
            return;
        }
    }

    public void DisminuirRonda()
    {
        AudioManager.instance.PlaySound("botonmenu");

        int ronda = GameManager.Instance.puntosParaGanar;

        if (ronda == 10)
        {
            GameManager.Instance.puntosParaGanar = 9;
            rondaActualText.text = "9";
        }
        else if (ronda == 9)
        {
            GameManager.Instance.puntosParaGanar = 8;
            rondaActualText.text = "8";
        }
        else if (ronda == 8)
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
        AudioManager.instance.PlaySound("botonmenu");

        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 60f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualMDSText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualMDSText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualMDSText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualMDSText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualMDSText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualMDSText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualMDSText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 300f;
            tiempoActualMDSText.text = "5:00";
        }
        else if (tiempo == 300f)
        {
            return;
        }
    }

    public void DisminuirTiempo()
    {
        AudioManager.instance.PlaySound("botonmenu");

        float tiempo = GameManager.Instance.totalTime;

        if (tiempo == 300f)
        {
            GameManager.Instance.totalTime = 270f;
            tiempoActualMDSText.text = "4:30";
        }
        else if (tiempo == 270f)
        {
            GameManager.Instance.totalTime = 240f;
            tiempoActualMDSText.text = "4:00";
        }
        else if (tiempo == 240f)
        {
            GameManager.Instance.totalTime = 210f;
            tiempoActualMDSText.text = "3:30";
        }
        else if (tiempo == 210f)
        {
            GameManager.Instance.totalTime = 180f;
            tiempoActualMDSText.text = "3:00";
        }
        else if (tiempo == 180f)
        {
            GameManager.Instance.totalTime = 150f;
            tiempoActualMDSText.text = "2:30";
        }
        else if (tiempo == 150f)
        {
            GameManager.Instance.totalTime = 120f;
            tiempoActualMDSText.text = "2:00";
        }
        else if (tiempo == 120f)
        {
            GameManager.Instance.totalTime = 90f;
            tiempoActualMDSText.text = "1:30";
        }
        else if (tiempo == 90f)
        {
            GameManager.Instance.totalTime = 60f;
            tiempoActualMDSText.text = "1:00";
        }
        else if (tiempo == 60f)
        {
            return;
        }
    }

    #endregion CAMBIOS DE TIEMPO

    public void CargarLobbyMHS()
    {
        if (!play) return;
        AudioManager.instance.PlaySound("botonJugar");
        ultimoBotonSeleccionadoStatic = botonEmpezarPartidaMHS.gameObject;
        SceneManager.LoadScene("LOBBY");
    }

    public void CargarLobbyMDS()
    {
        if (!play) return;
        AudioManager.instance.PlaySound("botonJugar");
        ultimoBotonSeleccionadoStatic = botonEmpezarPartidaMDS.gameObject;
        SceneManager.LoadScene("LOBBY");
    }

}
