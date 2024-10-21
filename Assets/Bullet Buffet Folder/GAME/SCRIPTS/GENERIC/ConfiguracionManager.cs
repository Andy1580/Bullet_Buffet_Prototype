using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionManager : MonoBehaviour
{
    private void OnEnable()
    {
        ConfigurarResolucionesLimitadas();
        CargarValoresDePantalla();
        CargarValoresDeFPS();

        if (panelFPS == null)
        {
            panelFPS = GameObject.FindWithTag("PanelFPS");
        }
    }
    private void Awake()
    {

        CargarValoresDeAudio();

        // Añadir listeners a los elementos UI
        pantallaCompletaToggle.onValueChanged.AddListener(CambiarPantallaCompleta);
        resolucionDropdown.onValueChanged.AddListener(CambiarResolucion);

        //CargarValoresDeFPS();
        CargarVsync();

        if (panelFPS == null)
        {
            panelFPS = GameObject.FindWithTag("PanelFPS");
        }

    }

    private void Start()
    {


        CargarValoresDeAudio();

        pantallaCompletaToggle.onValueChanged.AddListener(CambiarPantallaCompleta);
        resolucionDropdown.onValueChanged.AddListener(CambiarResolucion);

        CargarVsync();

    }

    #region CONFIGURACION MANAGER
    [Header("Canvas Core")]
    public GameObject panelConfirmar;
    public GameObject panelConfiguracion;

    public void AceptarCambios()
    {
        if (cambiosAudioRealizados)
        {
            AceptarCambiosDeAudio();
        }

        if (cambiosPantallaRealizados)
        {
            AceptarCambiosDePantalla();
        }
        
        if (cambiosFPSRealizados)
        {
            AceptarCambiosDeFPS();
        }
        
        cambiosAudioRealizados = false;
        cambiosPantallaRealizados = false;
        cambiosFPSRealizados = false;

        panelConfiguracion.SetActive(false);
        panelConfirmar.SetActive(false);
    }

    public void ConfirmarCancelarCambios(bool guardar)
    {
        if (guardar)
        {
            AceptarCambios();
        }
        else
        {
            ConfirmarCancelarCambiosDeAudio(false);
            ConfirmarCancelarCambiosDePantalla(false);
            ConfirmarCancelarCambiosDeFPS(false);
            panelConfiguracion.SetActive(false);
        }
        panelConfirmar.SetActive(false);
    }

    public void CancelarCambios(bool guardar)
    {
        if (guardar)
        {
            AceptarCambios();
        }
        else if (cambiosAudioRealizados || cambiosPantallaRealizados || cambiosFPSRealizados)
        {
            panelConfirmar.SetActive(true);
        }
        else
        {
            panelConfiguracion.SetActive(false);
        }
    }

    #endregion 

    #region AUDIO CORE

    [Header("Slider Core")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderSFX;
    private float ultimoVolumenGeneral;
    private float ultimoVolumenMusica;
    private float ultimoVolumenSFX;

    public bool cambiosAudioRealizados;

    private void CargarValoresDeAudio()
    {
        sliderGeneral.value = PlayerPrefs.GetFloat("volumenGeneral", 1f);
        sliderMusica.value = PlayerPrefs.GetFloat("volumenMusica", 1f);
        sliderSFX.value = PlayerPrefs.GetFloat("volumenSFX", 1f);




        ultimoVolumenGeneral = sliderGeneral.value;
        ultimoVolumenMusica = sliderMusica.value;
        ultimoVolumenSFX = sliderSFX.value;

        ModificarVolumenGeneral(sliderGeneral.value);
        ModificarVolumenMusica(sliderMusica.value);
        ModificarVolumenSFX(sliderSFX.value);

        cambiosAudioRealizados = false;
    }

    public void ModificarVolumenGeneral(float volumen)
    {
        print("Volumen General: " + volumen);
        AudioListener.volume = volumen;
        cambiosAudioRealizados = true;
    }

    public void ModificarVolumenMusica(float volumen)
    {
        print("Volumen Musica: " + volumen);
        AudioManager.instance.AjustarVolumenMusica(volumen);
        cambiosAudioRealizados = true;
    }

    public void ModificarVolumenSFX(float volumen)
    {
        print("Volumen SFX: " + volumen);
        AudioManager.instance.AjustarVolumenSFX(volumen);
        cambiosAudioRealizados = true;
    }

    public void AceptarCambiosDeAudio()
    {
        PlayerPrefs.SetFloat("volumenGeneral", sliderGeneral.value);
        PlayerPrefs.SetFloat("volumenMusica", sliderMusica.value);
        PlayerPrefs.SetFloat("volumenSFX", sliderSFX.value);
        PlayerPrefs.Save();



        ultimoVolumenGeneral = sliderGeneral.value;
        ultimoVolumenMusica = sliderMusica.value;
        ultimoVolumenSFX = sliderSFX.value;

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
            sliderGeneral.value = ultimoVolumenGeneral;
            sliderMusica.value = ultimoVolumenMusica;
            sliderSFX.value = ultimoVolumenSFX;
            ModificarVolumenGeneral(ultimoVolumenGeneral);
            ModificarVolumenMusica(ultimoVolumenMusica);
            ModificarVolumenSFX(ultimoVolumenSFX);

            cambiosAudioRealizados = false;
        }
    }
    #endregion

    #region PANTALLA CORE

    [Header("Pantalla Core")]
    [SerializeField] private Toggle pantallaCompletaToggle;
    private const string PantallaCompletaKey = "PantallaCompleta";

    [Header("Resolucion Core")]
    [SerializeField] private TMP_Dropdown resolucionDropdown;
    private Resolution[] resolucionesDisponibles;
    private const string ResolucionKey = "Resolucion";

    public bool cambiosPantallaRealizados;
    private bool ultimaPantallaCompleta;
    private int ultimaResolucion;

    private void CargarValoresDePantalla()
    {
        bool pantallaCompletaGuardada = PlayerPrefs.GetInt(PantallaCompletaKey, 1) == 1;
        pantallaCompletaToggle.isOn = pantallaCompletaGuardada;

        int resolucionGuardada = PlayerPrefs.GetInt(ResolucionKey, resolucionDropdown.value);
        resolucionDropdown.value = resolucionGuardada;

        ultimaPantallaCompleta = pantallaCompletaGuardada;
        ultimaResolucion = resolucionGuardada;

        CambiarPantallaCompleta(pantallaCompletaGuardada);
        CambiarResolucion(resolucionGuardada);

        cambiosPantallaRealizados = false;
    }

    void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        cambiosPantallaRealizados = true;
        if (pantallaCompleta)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
    }

    void CambiarResolucion(int indiceResolucion)
    {
        cambiosPantallaRealizados = true;

        Resolution resolucionSeleccionada = resolucionesDisponibles[indiceResolucion];
        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);
    }

    public void AceptarCambiosDePantalla()
    {
        PlayerPrefs.SetInt(PantallaCompletaKey, pantallaCompletaToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt(ResolucionKey, resolucionDropdown.value);
        PlayerPrefs.Save();

        ultimaPantallaCompleta = pantallaCompletaToggle.isOn;
        ultimaResolucion = resolucionDropdown.value;
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
            pantallaCompletaToggle.isOn = ultimaPantallaCompleta;
            resolucionDropdown.value = ultimaResolucion;
            CambiarPantallaCompleta(ultimaPantallaCompleta);
            CambiarResolucion(ultimaResolucion);

            cambiosPantallaRealizados = false;
        }
    }

    void ConfigurarResolucionesLimitadas()
    {
        resolucionesDisponibles = new Resolution[]
        {
            new Resolution { width = 3840, height = 2160 },  // 2160p (4K)
            new Resolution { width = 2560, height = 1440 },  // 1440p (2K)
            new Resolution { width = 1920, height = 1080 },  // 1080p (HD)
            new Resolution { width = 1280, height = 720 }    // 720p (HD)
        };

        resolucionDropdown.ClearOptions();
        List<string> opcionesResoluciones = new List<string>();
        foreach (Resolution resolucion in resolucionesDisponibles)
        {
            string opcion = resolucion.width + "x" + resolucion.height;
            opcionesResoluciones.Add(opcion);
        }
        resolucionDropdown.AddOptions(opcionesResoluciones);
    }

    #endregion

    #region V-SYNC
    [Header("Sincronización Vertical")]
    [SerializeField] private Toggle toggleVSync;

    public void CambiarVSync(bool activado)
    {
        QualitySettings.vSyncCount = activado ? 1 : 0;
        PlayerPrefs.SetInt("VSyncActivado", activado ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void CargarVsync()
    {
        int vsyncActivado = PlayerPrefs.GetInt("VSyncActivado", 0);
        toggleVSync.isOn = (vsyncActivado == 1);
        QualitySettings.vSyncCount = vsyncActivado == 1 ? 1 : 0;
        toggleVSync.onValueChanged.AddListener(delegate { CambiarVSync(toggleVSync.isOn); });
    }
    #endregion

    #region FPS Manager

    [Header("FPS Core")]
    [SerializeField] private GameObject panelFPS;  
    [SerializeField] private Toggle toggleFPS;     
    [SerializeField] private TMP_Dropdown dropdownFPS;  
    private int[] limitesFPS = { 60, 90, 120, -1 }; 
    public bool cambiosFPSRealizados = false;

    private void CargarValoresDeFPS()
    {
        int limiteFPS = PlayerPrefs.GetInt("FPSLimite", 60);
        dropdownFPS.value = limiteFPS == 0 ? 0 : (limiteFPS == 60 ? 1 : (limiteFPS == 90 ? 2 : (limiteFPS == 120 ? 3 : 4)));
        SetearLimiteFPS(limiteFPS);

        bool panelFPSActivo = PlayerPrefs.GetInt("FPSPanelActivo", 0) == 1;
        toggleFPS.isOn = panelFPSActivo; 
        panelFPS.SetActive(panelFPSActivo);
    }

    public void CambiarLimiteFPS(int indiceLimite)
    {
        int limite = limitesFPS[indiceLimite];
        SetearLimiteFPS(limite);
        cambiosFPSRealizados = true;
    }

    private void SetearLimiteFPS(int limite)
    {
        Application.targetFrameRate = limite;
        Debug.Log("Límite de FPS establecido en: " + (limite == -1 ? "Desbloqueado" : limite.ToString()));
    }

    public void ActivarPanelFPS(bool estado)
    {
        panelFPS.SetActive(estado); 
        cambiosFPSRealizados = true;
    }

    public void AceptarCambiosDeFPS()
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
            dropdownFPS.value = PlayerPrefs.GetInt("FPSLimite", 0); 
            toggleFPS.isOn = PlayerPrefs.GetInt("FPSPanelActivo", 0) == 1;  

            SetearLimiteFPS(limitesFPS[dropdownFPS.value]);
            ActivarPanelFPS(toggleFPS.isOn);
            cambiosFPSRealizados = false;
        }
    }

    #endregion
}
