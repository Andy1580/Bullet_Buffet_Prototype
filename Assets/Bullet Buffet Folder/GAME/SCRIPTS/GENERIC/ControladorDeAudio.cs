using UnityEngine;
using UnityEngine.UI;

public class ControladorDeAudio : MonoBehaviour
{
    [Header("Slider Core")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public bool cambiosRealizados;
    private float ultimoVolumenGeneral;
    private float ultimoVolumenMusica;
    private float ultimoVolumenSFX;


    // Evento para notificar cambios
    public event System.Action<bool> OnCambiosRealizados;

    private void Start()
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

        cambiosRealizados = false;
    }

    public void ModificarVolumenGeneral(float volumen)
    {
        AudioListener.volume = volumen;
        cambiosRealizados = true;
        OnCambiosRealizados?.Invoke(true);
    }

    public void ModificarVolumenMusica(float volumen)
    {
        AudioManager.instance.AjustarVolumenMusica(volumen);
        cambiosRealizados = true;
        OnCambiosRealizados?.Invoke(true);
    }

    public void ModificarVolumenSFX(float volumen)
    {
        AudioManager.instance.AjustarVolumenSFX(volumen);
        cambiosRealizados = true;
        OnCambiosRealizados?.Invoke(true);
    }

    public void AceptarCambios()
    {
        PlayerPrefs.SetFloat("volumenGeneral", sliderGeneral.value);
        PlayerPrefs.SetFloat("volumenMusica", sliderMusica.value);
        PlayerPrefs.SetFloat("volumenSFX", sliderSFX.value);
        PlayerPrefs.Save();
        ultimoVolumenGeneral = sliderGeneral.value;
        ultimoVolumenMusica = sliderMusica.value;
        ultimoVolumenSFX = sliderSFX.value;

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
            sliderGeneral.value = ultimoVolumenGeneral;
            sliderMusica.value = ultimoVolumenMusica;
            sliderSFX.value = ultimoVolumenSFX;

            ModificarVolumenGeneral(ultimoVolumenGeneral);
            ModificarVolumenMusica(ultimoVolumenMusica);
            ModificarVolumenSFX(ultimoVolumenSFX);

            cambiosRealizados = false;
            OnCambiosRealizados?.Invoke(false);
        }
    }
}

