using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControladorFPS : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;  // Referencia al texto que muestra los FPS
    [SerializeField] private float frecuenciaActualizacion = 0.5f;  // Cada cuántos segundos se actualiza el texto de FPS
    private float deltaTime = 0.0f;
    private float tiempoTranscurrido = 0.0f;  // Lleva cuenta del tiempo entre actualizaciones

    private void Update()
    {
        // Calcular el tiempo transcurrido para calcular FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;  // Suaviza el cambio de FPS

        tiempoTranscurrido += Time.deltaTime;

        // Solo actualizar el texto de FPS cada "frecuenciaActualizacion" segundos
        if (tiempoTranscurrido >= frecuenciaActualizacion)
        {
            MostrarFPS();  // Actualizar el texto de FPS
            tiempoTranscurrido = 0.0f;  // Reiniciar contador de tiempo
        }
    }

    // Mostrar los FPS en el texto (más lento para que no cambie tan rápido)
    private void MostrarFPS()
    {
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.RoundToInt(fps);  // Redondear para mostrar FPS
    }
}
