using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfiguracionManager : MonoBehaviour
{
    [Header("Canvas Core")]
    public GameObject panelConfirmar;
    public GameObject panelConfiguracion;
    public GameObject panelFPS;
    public Button botonConfiguracion;

    [Header("Sliders y Toggles")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public Toggle pantallaCompletaToggle;
    public TMP_Dropdown resolucionDropdown;
    public TMP_Dropdown dropdownFPS;
    public Toggle toggleFPS;

    public bool cambiosAudioRealizados;
    public bool cambiosPantallaRealizados;
    public bool cambiosFPSRealizados;

    private Resolution[] resolucionesDisponibles;

    private void Awake()
    {
        if (panelFPS == null)
        {
            panelFPS = GameObject.FindWithTag("PanelFPS");
        }

        CargarValoresIniciales();
    }

    private void OnEnable()
    {
        // Configurar valores iniciales y listeners para eventos de UI
        ConfigurarResolucionesLimitadas();
        CargarValoresIniciales();

        // Activar el manejo de input para este panel
        InputManager.Instance.SetActivePanel(OnSubmitConfiguracion, OnCancelConfiguracion);
    }

    private void OnDisable()
    {
        // Limpiar las acciones de Submit y Cancel cuando el panel se desactiva
        InputManager.Instance.SetActivePanel(null, null);
    }

    private void CargarValoresIniciales()
    {
        CargarValoresDeAudio();
        CargarValoresDePantalla();
        CargarValoresDeFPS();
        CargarVsync();
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
            else if (currentSelected == pantallaCompletaToggle.gameObject)
            {
                CambiarPantallaCompleta(pantallaCompletaToggle.isOn);
            }
            else if (currentSelected == resolucionDropdown.gameObject)
            {
                CambiarResolucion(resolucionDropdown.value);
            }
            else if (currentSelected == dropdownFPS.gameObject)
            {
                CambiarLimiteFPS(dropdownFPS.value);
            }
        }
    }

    private void OnCancelConfiguracion()
    {
        if (panelConfirmar.activeSelf)
        {
            // Si el panel de confirmación está activo, manejar Cancel como cancelación de los cambios
            ConfirmarCancelarCambios(false); // Cancelar los cambios
        }
        else if (cambiosAudioRealizados || cambiosPantallaRealizados || cambiosFPSRealizados)
        {
            // Si hay cambios pendientes, activar el panel de confirmación
            panelConfirmar.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null); // Desactivar selección de UI
        }
        else
        {
            // Si no hay cambios pendientes, cerrar el panel de configuración
            //CerrarConfiguracion();
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
        //CerrarConfiguracion();
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
    }

    private void CancelarCambios()
    {
        ConfirmarCancelarCambiosDeAudio(false);
        ConfirmarCancelarCambiosDePantalla(false);
        ConfirmarCancelarCambiosDeFPS(false);
    }

    private IEnumerator CerrarConfiguracionConDelay()
    {
        yield return null; // Espera un cuadro para asegurar que se aplica SetActive(false)
        Debug.Log("Se cerro Configuracion");
        panelConfiguracion.SetActive(false);
        panelConfirmar.SetActive(false);
        EventSystem.current.SetSelectedGameObject(botonConfiguracion.gameObject);
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
        sliderGeneral.value = PlayerPrefs.GetFloat("volumenGeneral", 1f);
        sliderMusica.value = PlayerPrefs.GetFloat("volumenMusica", 1f);
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
        pantallaCompletaToggle.isOn = PlayerPrefs.GetInt("PantallaCompleta", 1) == 1;
        resolucionDropdown.value = PlayerPrefs.GetInt("Resolucion", 0);
        cambiosPantallaRealizados = false;
    }

    private void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        cambiosPantallaRealizados = true;
    }

    private void CambiarResolucion(int indiceResolucion)
    {
        if (resolucionesDisponibles == null || resolucionesDisponibles.Length == 0) return;

        Resolution resolucionSeleccionada = resolucionesDisponibles[indiceResolucion];
        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);
        cambiosPantallaRealizados = true;
    }

    private void AceptarCambiosDePantalla()
    {
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompletaToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Resolucion", resolucionDropdown.value);
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
            pantallaCompletaToggle.isOn = PlayerPrefs.GetInt("PantallaCompleta", 1) == 1;
            resolucionDropdown.value = PlayerPrefs.GetInt("Resolucion", 0);
            cambiosPantallaRealizados = false;
        }
    }

    private void ConfigurarResolucionesLimitadas()
    {
        resolucionesDisponibles = new Resolution[]
        {
            new Resolution { width = 3840, height = 2160 }, //4k
            new Resolution { width = 2560, height = 1440 }, //2k
            new Resolution { width = 1920, height = 1080 }, //1080p
            new Resolution { width = 1280, height = 720 } //720p
        };

        resolucionDropdown.ClearOptions();
        var opcionesResoluciones = new List<string>();
        foreach (var resolucion in resolucionesDisponibles)
        {
            opcionesResoluciones.Add($"{resolucion.width}x{resolucion.height}");
        }
        resolucionDropdown.AddOptions(opcionesResoluciones);
    }

    #endregion

    #region Configuración de FPS

    private void CargarValoresDeFPS()
    {
        // Cargar el límite de FPS desde PlayerPrefs y establecer el valor del dropdown
        int limiteFPS = PlayerPrefs.GetInt("FPSLimite", 60);
        dropdownFPS.value = limiteFPS == 0 ? 0 : (limiteFPS == 60 ? 1 : (limiteFPS == 90 ? 2 : (limiteFPS == 120 ? 3 : 4)));
        Application.targetFrameRate = limiteFPS;

        // Cargar el estado del toggleFPS desde PlayerPrefs y activar/desactivar panelFPS
        bool isFPSEnabled = PlayerPrefs.GetInt("FPSPanelActivo", 0) == 1;
        toggleFPS.isOn = isFPSEnabled;
        panelFPS.SetActive(isFPSEnabled);

        cambiosFPSRealizados = false;
    }

    public void CambiarLimiteFPS(int indiceLimite)
    {
        // Cambiar el límite de FPS según el índice del dropdown (índice 1 = 60 FPS, índice 0 = 30 FPS)
        Application.targetFrameRate = (indiceLimite == 1) ? 60 : 30;
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
        PlayerPrefs.SetInt("FPSLimite", dropdownFPS.value);
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
            // Restaurar el límite de FPS y el estado del toggleFPS a los valores guardados en PlayerPrefs
            int fpsValue = PlayerPrefs.GetInt("FPSLimite", 60);
            dropdownFPS.value = (fpsValue == 60) ? 1 : 0;
            Application.targetFrameRate = fpsValue;

            bool isFPSEnabled = PlayerPrefs.GetInt("FPSPanelActivo", 0) == 1;
            toggleFPS.isOn = isFPSEnabled;
            panelFPS.SetActive(isFPSEnabled);

            cambiosFPSRealizados = false;
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

