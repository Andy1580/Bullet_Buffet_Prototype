using UnityEngine;

public class ConfiguracionManager : MonoBehaviour
{
    [Header("Controladores Core")]
    public ControladorDeAudio controladorDeAudio;
    public ControlDePantalla controlDePantalla;

    [Header("Canvas Core")]
    public GameObject panelConfirmacion;
    public GameObject panelConfiguracion;

    private bool cambiosRealizadosAudio;
    private bool cambiosRealizadosPantalla;

    private void Start()
    {
        controladorDeAudio.OnCambiosRealizados += CambiosEnAudio;
        controlDePantalla.OnCambiosRealizados += CambiosEnPantalla;

        cambiosRealizadosAudio = false;
        cambiosRealizadosPantalla = false;
    }

    private void CambiosEnAudio(bool cambios)
    {
        cambiosRealizadosAudio = cambios;
    }

    private void CambiosEnPantalla(bool cambios)
    {
        cambiosRealizadosPantalla = cambios;
    }

    public void AceptarCambios()
    {
        if (cambiosRealizadosAudio)
        {
            controladorDeAudio.AceptarCambios();
        }

        if (cambiosRealizadosPantalla)
        {
            controlDePantalla.AceptarCambios();
        }

        panelConfiguracion.SetActive(false);
        cambiosRealizadosAudio = false;
        cambiosRealizadosPantalla = false;
    }

    public void CancelarCambios()
    {
        if (cambiosRealizadosAudio || cambiosRealizadosPantalla)
        {
            panelConfirmacion.SetActive(true);
        }
        else
        {
            panelConfiguracion.SetActive(false);
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
            if (cambiosRealizadosAudio)
            {
                controladorDeAudio.ConfirmarCancelarCambios(false);
            }

            if (cambiosRealizadosPantalla)
            {
                controlDePantalla.ConfirmarCancelarCambios(false);
            }

            panelConfiguracion.SetActive(false);
        }
        panelConfirmacion.SetActive(false);
    }
}
