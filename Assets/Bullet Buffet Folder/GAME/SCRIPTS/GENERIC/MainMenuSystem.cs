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
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelSchemeControl;
    [SerializeField] private GameObject panelCreditos;
    [SerializeField] private GameObject panelConfimarSalir;

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
    [SerializeField] private TMP_Text tiempoActualMHSText;

    [SerializeField] private Button botonJugarMenu;
    [SerializeField] private Button botonInicio;
    [SerializeField] private Button botonModos;
    [SerializeField] private Toggle togglePantallaCompleta;
    [SerializeField] private Button botonMHSCalle;
    [SerializeField] private Button botonMDSCalle;

    private bool play;

    private static GameObject ultimoBotonSeleccionadoStatic;
    private GameObject ultimoBotonSeleccionado;

    private static bool panelInicioActivado = false;

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

        ultimoBotonSeleccionado = ultimoBotonSeleccionadoStatic;
        play = false;
        ResetAllPanels();
        SetMenuNavigation();
    }

    private void OnEnable()
    {
        SetMenuNavigation();
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
        panelSettings.SetActive(false);
        panelMapasMHS.SetActive(false);
        panelMapasMDS.SetActive(false);
        panelSchemeControl.SetActive(false);
        panelConfimarSalir.SetActive(false);
        panelCreditos.SetActive(false);

        cafeMHS.SetActive(false);
        ciudadMHS.SetActive(false);
        dungeonMHS.SetActive(false);
        cafeMDS.SetActive(false);
        ciudadMDS.SetActive(false);
        dungeonMDS.SetActive(false);

        rondaActualText.text = "1";
        tiempoActualMDSText.text = "1:00";
        tiempoActualMHSText.text = "1:00";
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
        ultimoBotonSeleccionadoStatic = botonModos.gameObject; 
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
            panelModos.SetActive(true);
            panelMapasMHS.SetActive(false);
            EventSystem.current.SetSelectedGameObject(botonModos.gameObject);
            ultimoBotonSeleccionadoStatic = botonModos.gameObject;
        }
        else if (panelMapasMDS.activeSelf)
        {
            panelModos.SetActive(true);
            panelMapasMDS.SetActive(false);
            EventSystem.current.SetSelectedGameObject(botonModos.gameObject);
            ultimoBotonSeleccionadoStatic = botonModos.gameObject;
        }
        else if (panelSchemeControl.activeSelf)
        {
            panelMenuPrincipal.SetActive(true);
            panelSchemeControl.SetActive(false);
            EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
            ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
            // Restaura el comportamiento normal de Submit en el menú principal
            InputManager.Instance.SetActivePanel(null, Back);
        }
        else if (panelConfimarSalir.activeSelf)
        {
            // Cierra el panel de confirmación y vuelve al menú principal
            panelConfimarSalir.SetActive(false);
            panelMenuPrincipal.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
            ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
            // Restaura el comportamiento normal de Submit en el menú principal
            InputManager.Instance.SetActivePanel(null, Back);
        }
        else if (panelCreditos.activeSelf)
        {
            panelCreditos.SetActive(false);
            panelMenuPrincipal.SetActive(true);
            EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
            ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
        }

        play = false;

        GameManager.Instance.ResetiarVariables();
        AudioManager.instance.PlaySound("botonmenu");
    }

    public void GoToSettings()
    {
        ResetAllPanels();
        InputManager.Instance.SetActivePanel(null, null);
        panelSettings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(togglePantallaCompleta.gameObject);
        ultimoBotonSeleccionadoStatic = togglePantallaCompleta.gameObject;
        AudioManager.instance.PlaySound("botonmenu");
    }

    public void GoToSchemeControl()
    {
        ResetAllPanels();
        panelSchemeControl.SetActive(true);
        AudioManager.instance.PlaySound("botonmenu");

    }

    public void GoToCreditos()
    {
        ResetAllPanels();
        panelCreditos.SetActive(true);
        AudioManager.instance.PlaySound("botonmenu");

        // Restaura el comportamiento normal de Submit en el menú principal
        InputManager.Instance.SetActivePanel(null, Back);
    }

    public void ModoHechizosSazonados()
    {
        GameManager.modoHS = true;
        GameManager.modoDS = false;
        ResetAllPanels();
        panelMapasMHS.SetActive(true);

        AudioManager.instance.PlaySound("botonmenu");

        EventSystem.current.SetSelectedGameObject(botonMHSCalle.gameObject);
        ultimoBotonSeleccionadoStatic = botonMHSCalle.gameObject;
    }

    public void ModoDueloDeSalsas()
    {
        GameManager.modoDS = true;
        GameManager.modoHS = false;
        ResetAllPanels();
        panelMapasMDS.SetActive(true);

        AudioManager.instance.PlaySound("botonmenu");

        EventSystem.current.SetSelectedGameObject(botonMDSCalle.gameObject);
        ultimoBotonSeleccionadoStatic = botonMDSCalle.gameObject;
    }

    public void ConfirmacionSalir()
    {
        ResetAllPanels();
        panelConfimarSalir.SetActive(true);
        AudioManager.instance.PlaySound("botonmenu");

        // Restaura el comportamiento normal de Submit en el menú principal
        InputManager.Instance.SetActivePanel(Salir, Back);

        EventSystem.current.SetSelectedGameObject(botonJugarMenu.gameObject);
        ultimoBotonSeleccionadoStatic = botonJugarMenu.gameObject;
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
        AudioManager.instance.PlaySound("botonmenu");

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
        AudioManager.instance.PlaySound("botonmenu");
        SceneManager.LoadScene("LOBBY");
    }

}
