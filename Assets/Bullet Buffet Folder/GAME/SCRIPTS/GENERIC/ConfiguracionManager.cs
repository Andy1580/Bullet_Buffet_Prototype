using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfiguracionManager : MonoBehaviour
{
    public static ConfiguracionManager Instance;

    [Header("Canvas Core")]
    public GameObject panelConfirmar;
    public GameObject panelConfiguracion;
    public GameObject panelFPS;

    [Header("Sliders y Toggles")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public Toggle pantallaCompletaToggle;
    public Toggle toggleFPS;

    public bool cambiosAudioRealizados;
    public bool cambiosPantallaRealizados;
    public bool cambiosFPSRealizados;

    private int indiceResolucionActual = 0;
    private Resolution[] resolucionesDisponibles;

    private int[] fpsOpciones = { 60, 90, 120 }; // Opciones de FPS disponibles
    private int indiceFPSActual = 0; // Índice actual en el array de opciones

    public TMP_Text ResolucionText;
    public TMP_Text fpsText;

    private void Awake()
    {
        //string escena = SceneManager.GetActiveScene().name;

        //if (escena == "ANDYMENUTEST" && gameObject.name.Contains("PANEL CONFIGURACION"))
        //{
        //    Debug.Log("ConfiguracionManager del GameManager desactivado en el menú principal.");
        //    enabled = false;
        //}
        //else if(escena != "ANDYMENUTEST" && gameObject.name.Contains("PANEL CONFIGURACION"))
        //{
        //    Debug.Log("ConfiguracionManager del Canvas del menú principal desactivado en el juego.");
        //    enabled = false;
        //}

        Instance = this;

        //if (!PlayerPrefs.HasKey("FPSLimite"))
        //{
        //    PlayerPrefs.SetInt("FPSLimite", 60);
        //    PlayerPrefs.Save();
        //}

        //// Asegúrate de sincronizar el valor del Dropdown con los FPS guardados
        //int limiteFPS = PlayerPrefs.GetInt("FPSLimite");
        //dropdownFPS.value = limiteFPS == 60 ? 1 : (limiteFPS == 90 ? 2 : (limiteFPS == 120 ? 3 : 0));


        ConfigurarResolucionesLimitadas();
        CargarValoresIniciales();
    }

    private void Start()
    {

    }

    private IEnumerator EsperarYBuscarPanelFPS()
    {
        yield return new WaitForEndOfFrame(); // Espera hasta que la escena esté completamente cargada.
        //panelFPS = GameManager.PanelFPS;
        CargarValoresIniciales();

        if (panelFPS == null)
        {
            Debug.LogError("panelFPS aún no encontrado después de esperar.");
        }
    }

    public void PrenderConfiguracion()
    {
        panelConfiguracion.SetActive(true);
        panelConfirmar.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pantallaCompletaToggle.gameObject);
    }

    private void OnEnable()
    {
        // Configurar valores iniciales y listeners para eventos de UI

        //if (panelFPS == null)
        //{
        //    StartCoroutine(EsperarYBuscarPanelFPS());
        //}

        //CargarValoresIniciales();

        //// Activar el manejo de input para este panel
        //if (InputManager.Instance != null)
        //{
        //    InputManager.Instance.SetActivePanel(OnSubmitConfiguracion, OnCancelConfiguracion);
        //    Debug.Log("Se activo configuracion manager y se designo el Input");
        //}
    }

    private void OnDisable()
    {
        // Limpiar las acciones de Submit y Cancel cuando el panel se desactiva
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SetActivePanel(null, null);
        }
    }

    public void CargarValoresIniciales()
    {
        CargarValoresDePantalla();
        CargarValoresDeFPS();
        CargarVsync();
        CargarValoresDeAudio();
    }

    public void ResetarInputCM()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SetActivePanel(OnSubmitConfiguracion, OnCancelConfiguracion);
            //Debug.Log("Se activo configuracion manager y se designo el Input");
        }
    }

    private void RecetearInput()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            MainMenuSystem.instance.ResetearInput();
        }
        else
        {
            GameManager.Instance.ResetearInputGM();
        }
    }
    #region Manejo de Submit y Cancel

    private void OnSubmitConfiguracion()
    {
        // Si el panel de confirmación está activo, manejar el Submit como confirmación
        if (panelConfirmar.activeSelf)
        {
            ConfirmarCancelarCambios(true); // Confirmar los cambios
        }
        else
        {
            // Si el panel de confirmación no está activo, aplicar las configuraciones del control seleccionado
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == sliderGeneral.gameObject)
            {
                ModificarVolumenGeneral(sliderGeneral.value);
            }
            else if (currentSelected == sliderMusica.gameObject)
            {
                ModificarVolumenMusica(sliderMusica.value);
            }
            else if (currentSelected == sliderSFX.gameObject)
            {
                ModificarVolumenSFX(sliderSFX.value);
            }
            //else if (currentSelected == pantallaCompletaToggle.gameObject)
            //{
            //    CambiarPantallaCompleta(pantallaCompletaToggle.isOn);
            //}
            //else if (currentSelected == resolucionDropdown.gameObject)
            //{
            //    CambiarResolucion(resolucionDropdown.value);
            //}
            //else if (currentSelected == dropdownFPS.gameObject)
            //{
            //    CambiarLimiteFPS(dropdownFPS.value);
            //}
        }
    }

    private void OnCancelConfiguracion()
    {
        if (panelConfirmar.activeSelf)
        {
            // Si el panel de confirmación está activo, manejar Cancel como cancelación de los cambios
            ConfirmarCancelarCambios(false); // Cancelar los cambios
            AudioManager.instance.PlaySound("botonBack");
        }
        else if (cambiosAudioRealizados || cambiosPantallaRealizados || cambiosFPSRealizados)
        {
            // Desactivar todos los Dropdowns dinámicos (Dropdown List instanciados)
            GameObject dropdownList = GameObject.Find("Dropdown List");
            if (dropdownList != null)
            {
                dropdownList.SetActive(false); // Desactivamos si está activo
            }

            // Desactivar cualquier Template activo en los Dropdowns
            Dropdown[] dropdowns = GetComponentsInChildren<Dropdown>(true);
            foreach (Dropdown dropdown in dropdowns)
            {
                if (dropdown.template.gameObject.activeSelf)
                {
                    dropdown.template.gameObject.SetActive(false);
                }
            }

            // Si hay cambios pendientes, activar el panel de confirmación
            panelConfirmar.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null); // Desactivar selección de UI
        }
        else
        {
            // Si no hay cambios pendientes, cerrar el panel de configuración
            Debug.Log("Se pulso la B");
            AudioManager.instance.PlaySound("botonBack");
            StartCoroutine(CerrarConfiguracionConDelay());
        }

    }

    public void ConfirmarCancelarCambios(bool guardar)
    {
        if (guardar)
        {
            AceptarCambios();
        }
        else
        {
            CancelarCambios();
        }

        // Cerrar ambos paneles después de confirmar o cancelar
        StartCoroutine(CerrarConfiguracionConDelay());
    }

    #endregion

    #region Métodos para Cambios y Aceptar/Cancelar

    private void AceptarCambios()
    {
        if (cambiosAudioRealizados)
            AceptarCambiosDeAudio();

        if (cambiosPantallaRealizados)
            AceptarCambiosDePantalla();

        if (cambiosFPSRealizados)
            AceptarCambiosDeFPS();

        cambiosAudioRealizados = cambiosPantallaRealizados = cambiosFPSRealizados = false;
        //CerrarConfiguracion();
        StartCoroutine(CerrarConfiguracionConDelay());

        AudioManager.instance.PlaySound("botonmenu");
    }

    private void CancelarCambios()
    {
        ConfirmarCancelarCambiosDeAudio(false);
        ConfirmarCancelarCambiosDePantalla(false);
        ConfirmarCancelarCambiosDeFPS(false);

        AudioManager.instance.PlaySound("botonBack");
    }

    private IEnumerator CerrarConfiguracionConDelay()
    {
        yield return null; // Espera un cuadro para asegurar que se aplica SetActive(false)
        Debug.Log("Se cerro Configuracion");
        panelConfiguracion.SetActive(false);
        panelConfirmar.SetActive(false);

        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            GameObject objetoSeleccionado = MainMenuSystem.ultimoBotonSeleccionadoStatic;
            EventSystem.current.SetSelectedGameObject(objetoSeleccionado);
            print(objetoSeleccionado);
        }
        else
        {
            GameObject objetoSeleccionado = GameManager.ultimoBotonSeleccionadoStatic;
            EventSystem.current.SetSelectedGameObject(objetoSeleccionado);
            print(objetoSeleccionado);
        }
        RecetearInput();
    }

    /*
    private void CerrarConfiguracion()
    {
        Debug.Log("Se cerro Configuracion");
        panelConfiguracion.SetActive(false);
        panelConfirmar.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonConfiguracion.gameObject);
    }
    */
    #endregion

    #region Configuración de Audio

    private void CargarValoresDeAudio()
    {
        if (sliderGeneral == null)
        {
            Debug.LogError("sliderGeneral no está asignado.");
            return;
        }
        sliderGeneral.value = PlayerPrefs.GetFloat("volumenGeneral", 1f);

        if (sliderMusica == null)
        {
            Debug.LogError("sliderMusica no está asignado.");
            return;
        }

        //print(sliderMusica);
        //print(PlayerPrefs.GetFloat("volumenMusica", 1f));
        sliderMusica.value = PlayerPrefs.GetFloat("volumenMusica", 1f);

        if (sliderSFX == null)
        {
            Debug.LogError("sliderSFX no está asignado.");
            return;
        }
        sliderSFX.value = PlayerPrefs.GetFloat("volumenSFX", 1f);

        cambiosAudioRealizados = false;
    }

    public void ModificarVolumenGeneral(float volumen)
    {
        AudioListener.volume = volumen;
        cambiosAudioRealizados = true;
    }

    public void ModificarVolumenMusica(float volumen)
    {
        AudioManager.instance.AjustarVolumenMusica(volumen);
        cambiosAudioRealizados = true;
    }

    public void ModificarVolumenSFX(float volumen)
    {
        AudioManager.instance.AjustarVolumenSFX(volumen);
        cambiosAudioRealizados = true;
    }

    private void AceptarCambiosDeAudio()
    {
        PlayerPrefs.SetFloat("volumenGeneral", sliderGeneral.value);
        PlayerPrefs.SetFloat("volumenMusica", sliderMusica.value);
        PlayerPrefs.SetFloat("volumenSFX", sliderSFX.value);
        PlayerPrefs.Save();
        cambiosAudioRealizados = false;
    }

    public void ConfirmarCancelarCambiosDeAudio(bool guardar)
    {
        if (guardar)
        {
            AceptarCambiosDeAudio();
        }
        else
        {
            sliderGeneral.value = PlayerPrefs.GetFloat("volumenGeneral", 1f);
            sliderMusica.value = PlayerPrefs.GetFloat("volumenMusica", 1f);
            sliderSFX.value = PlayerPrefs.GetFloat("volumenSFX", 1f);
            cambiosAudioRealizados = false;
        }
    }

    #endregion

    #region Configuración de Pantalla

    private void CargarValoresDePantalla()
    {
        // Cargar la configuración de PlayerPrefs
        bool pantallaCompleta = PlayerPrefs.GetInt("PantallaCompleta", 1) == 1;
        pantallaCompletaToggle.isOn = pantallaCompleta; // Sincroniza el toggle
        Screen.fullScreen = pantallaCompleta; // Aplica el estado inicial
        Debug.Log($"Estado inicial: Pantalla Completa = {pantallaCompleta}");
        indiceResolucionActual = PlayerPrefs.GetInt("Resolucion", resolucionesDisponibles.Length - 1); // Última resolución por defecto

        // Aplicar la configuración inicial
        CambiarResolucion(indiceResolucionActual);
        CambiarPantallaCompleta(pantallaCompletaToggle.isOn);

        Debug.Log($"Cargados valores: Pantalla Completa = {pantallaCompletaToggle.isOn}, Resolución = {indiceResolucionActual}");
        cambiosPantallaRealizados = false;
    }

    public void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        Debug.Log($"Pantalla Completa: {pantallaCompleta}");

        // Guardar cambios en PlayerPrefs
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void CambiarResolucion(int indiceResolucion)
    {
        if (indiceResolucion < 0 || indiceResolucion >= resolucionesDisponibles.Length)
        {
            Debug.LogWarning("Índice de resolución inválido");
            return;
        }

        indiceResolucionActual = indiceResolucion;
        Resolution resolucionSeleccionada = resolucionesDisponibles[indiceResolucionActual];
        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);

        // Actualizar el texto de resolución
        ResolucionText.text = $"{resolucionSeleccionada.width}x{resolucionSeleccionada.height}";
        Debug.Log($"Resolución actualizada a: {ResolucionText.text}");
        Debug.Log($"Resolución cambiada a: {resolucionSeleccionada.width}x{resolucionSeleccionada.height}");

        // Guardar cambios en PlayerPrefs
        PlayerPrefs.SetInt("Resolucion", indiceResolucionActual);
        PlayerPrefs.Save();
    }

    public void AumentarResolucion()
    {
        if (indiceResolucionActual < resolucionesDisponibles.Length - 1)
        {
            indiceResolucionActual++;
            CambiarResolucion(indiceResolucionActual);
            //AudioManager.instance.PlaySound("aumento");
            cambiosPantallaRealizados = true;
        }
        else
        {
            Debug.Log("Ya está en la resolución máxima");
        }
    }

    public void DisminuirResolucion()
    {
        if (indiceResolucionActual > 0)
        {
            indiceResolucionActual--;
            CambiarResolucion(indiceResolucionActual);
            //AudioManager.instance.PlaySound("disminucion");
            cambiosPantallaRealizados = true;
        }
        else
        {
            Debug.Log("Ya está en la resolución mínima");
        }
    }

    private void AceptarCambiosDePantalla()
    {
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompletaToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Resolucion", indiceResolucionActual);
        PlayerPrefs.Save();
        cambiosPantallaRealizados = false;
    }

    public void ConfirmarCancelarCambiosDePantalla(bool guardar)
    {
        if (guardar)
        {
            AceptarCambiosDePantalla();
        }
        else
        {
            // Revertir a los valores guardados
            CargarValoresDePantalla();
            Debug.Log("Cambios de pantalla cancelados y revertidos");
        }
    }

    private void ConfigurarResolucionesLimitadas()
    {
        resolucionesDisponibles = new Resolution[]
    {
        new Resolution { width = 3840, height = 2160 }, // 4K
        new Resolution { width = 2560, height = 1440 }, // 2K
        new Resolution { width = 1920, height = 1080 }, // 1080p
        new Resolution { width = 1280, height = 720 }   // 720p
    };

        Debug.Log("Resoluciones configuradas:");
        foreach (var resolucion in resolucionesDisponibles)
        {
            Debug.Log($"{resolucion.width}x{resolucion.height}");
        }
    }

    #endregion

    #region Configuración de FPS

    private void CargarValoresDeFPS()
    {
        // Obtener el límite de FPS desde PlayerPrefs o establecer un valor predeterminado
        int limiteFPS = PlayerPrefs.GetInt("FPSLimite", 60);
        Debug.Log($"FPS cargados desde PlayerPrefs: {limiteFPS}");

        bool fpsPanel = PlayerPrefs.GetInt("FPSPanelActivo", 1) == 1;
        toggleFPS.isOn = fpsPanel; // Sincroniza el toggle

        // Encontrar el índice correspondiente en el array de opciones
        indiceFPSActual = System.Array.IndexOf(fpsOpciones, limiteFPS);
        if (indiceFPSActual == -1) // Si el valor no es válido, configurar a 60 FPS
        {
            Debug.LogWarning($"FPS inválidos en PlayerPrefs: {limiteFPS}, configurando a 60 por defecto.");
            indiceFPSActual = 3; // Por defecto, usar la primera opción (60 FPS)
            PlayerPrefs.SetInt("FPSLimite", fpsOpciones[indiceFPSActual]);
            PlayerPrefs.Save();
        }

        // Aplicar el límite de FPS
        Application.targetFrameRate = fpsOpciones[indiceFPSActual];
        fpsText.text = $"{fpsOpciones[indiceFPSActual]}"; // Actualizar el texto
        Debug.Log($"FPS configurados inicialmente a: {fpsText.text}");
        Debug.Log($"FPS configurados inicialmente a: {Application.targetFrameRate}");
        cambiosFPSRealizados = false;
    }

    public void AumentarFPS()
    {
        if (indiceFPSActual < fpsOpciones.Length - 1)
        {
            indiceFPSActual++;
            CambiarFPS();
            Debug.Log($"FPS aumentados a: {fpsOpciones[indiceFPSActual]}");
        }
        else
        {
            Debug.Log("Ya estás en el límite máximo de FPS.");
        }
    }

    public void DisminuirFPS()
    {
        if (indiceFPSActual > 0)
        {
            indiceFPSActual--;
            CambiarFPS();
            Debug.Log($"FPS disminuidos a: {fpsOpciones[indiceFPSActual]}");
        }
        else
        {
            Debug.Log("Ya estás en el límite mínimo de FPS.");
        }
    }

    private void CambiarFPS()
    {
        Application.targetFrameRate = fpsOpciones[indiceFPSActual];

        fpsText.text = $"{fpsOpciones[indiceFPSActual]}"; // Actualizar el texto
        Debug.Log($"FPS configurados inicialmente a: {fpsText.text}");

        // Guardar el cambio en PlayerPrefs
        PlayerPrefs.SetInt("FPSLimite", fpsOpciones[indiceFPSActual]);
        PlayerPrefs.Save();

        cambiosFPSRealizados = true;
        Debug.Log($"FPS cambiados y guardados: {Application.targetFrameRate}");
    }

    void CambiarLimiteFPS(int indiceLimite)
    {
        // Cambiar el límite de FPS según el índice del dropdown
        // Índice 0 = 60 FPS, Índice 1 = 90 FPS, Índice 2 = 120 FPS
        Application.targetFrameRate =
            (indiceLimite == 0) ? 60 :
            (indiceLimite == 1) ? 90 :
            120; // Si no es 0 o 1, se asume 120

        PlayerPrefs.SetInt("FPSLimite", indiceLimite);
        PlayerPrefs.Save();

        Debug.Log($"FPS configurados a: {Application.targetFrameRate}");
        cambiosFPSRealizados = true;
    }

    public void ActivarPanelFPS(bool estado)
    {
        // Activar o desactivar el panel y guardar el cambio en PlayerPrefs
        panelFPS.SetActive(estado);
        cambiosFPSRealizados = true;
        PlayerPrefs.SetInt("FPSPanelActivo", estado ? 1 : 0);
    }

    private void AceptarCambiosDeFPS()
    {
        PlayerPrefs.SetInt("FPSLimite", fpsOpciones[indiceFPSActual]);
        PlayerPrefs.SetInt("FPSPanelActivo", toggleFPS.isOn ? 1 : 0);
        PlayerPrefs.Save();

        cambiosFPSRealizados = false;
    }

    public void ConfirmarCancelarCambiosDeFPS(bool guardar)
    {
        if (guardar)
        {
            AceptarCambiosDeFPS();
        }
        else
        {
            // Revertir los valores a lo guardado
            CargarValoresDeFPS();
            Debug.Log("Cambios de FPS cancelados y revertidos.");
        }
    }

    #endregion

    #region Configuración de VSync

    [SerializeField] private Toggle toggleVSync;

    public void CambiarVSync(bool activado)
    {
        QualitySettings.vSyncCount = activado ? 1 : 0;
        PlayerPrefs.SetInt("VSyncActivado", activado ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void CargarVsync()
    {
        toggleVSync.isOn = PlayerPrefs.GetInt("VSyncActivado", 0) == 1;
    }

    #endregion
}

