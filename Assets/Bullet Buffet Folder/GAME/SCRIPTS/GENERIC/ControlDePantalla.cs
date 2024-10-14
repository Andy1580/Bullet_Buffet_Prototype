using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlDePantalla : MonoBehaviour
{
    [Header("Pantalla Core")]
    [SerializeField] private Toggle pantallaCompletaToggle;
    private const string PantallaCompletaKey = "PantallaCompleta";

    [Header("Resolucion Core")]
    [SerializeField] private TMP_Dropdown resolucionDropdown;
    private Resolution[] resolucionesDisponibles;
    private const string ResolucionKey = "Resolucion";
    public bool cambiosRealizados;
    private bool ultimaPantallaCompleta;
    private int ultimaResolucion;

    // Evento para notificar cambios
    public event System.Action<bool> OnCambiosRealizados;


    void Start()
    {
        ConfigurarResolucionesLimitadas();

        bool pantallaCompletaGuardada = PlayerPrefs.GetInt(PantallaCompletaKey, 1) == 1;
        pantallaCompletaToggle.isOn = pantallaCompletaGuardada;
        int resolucionGuardada = PlayerPrefs.GetInt(ResolucionKey, resolucionDropdown.value);
        resolucionDropdown.value = resolucionGuardada;

        ultimaPantallaCompleta = pantallaCompletaGuardada;
        ultimaResolucion = resolucionGuardada;

        CambiarPantallaCompleta(pantallaCompletaGuardada);
        CambiarResolucion(resolucionGuardada);

        pantallaCompletaToggle.onValueChanged.AddListener(CambiarPantallaCompleta);
        resolucionDropdown.onValueChanged.AddListener(CambiarResolucion);

        cambiosRealizados = false;
    }

    void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        cambiosRealizados = true;
        OnCambiosRealizados?.Invoke(true);
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
        cambiosRealizados = true;
        OnCambiosRealizados?.Invoke(true);

        Resolution resolucionSeleccionada = resolucionesDisponibles[indiceResolucion];
        Screen.SetResolution(resolucionSeleccionada.width, resolucionSeleccionada.height, Screen.fullScreen);
    }

    public void AceptarCambios()
    {
        PlayerPrefs.SetInt(PantallaCompletaKey, pantallaCompletaToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt(ResolucionKey, resolucionDropdown.value);
        PlayerPrefs.Save();

        ultimaPantallaCompleta = pantallaCompletaToggle.isOn;
        ultimaResolucion = resolucionDropdown.value;
        cambiosRealizados = false;
        OnCambiosRealizados?.Invoke(false);
    }

    public void ConfirmarCancelarCambios(bool guardar)
    {
        if (guardar)
        {
            AceptarCambios();
        }
        else
        {
            pantallaCompletaToggle.isOn = ultimaPantallaCompleta;
            resolucionDropdown.value = ultimaResolucion;

            CambiarPantallaCompleta(ultimaPantallaCompleta);
            CambiarResolucion(ultimaResolucion);

            cambiosRealizados = false;
            OnCambiosRealizados?.Invoke(false);
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
}

