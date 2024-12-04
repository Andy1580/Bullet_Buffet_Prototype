using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscudoController : MonoBehaviour
{
    [SerializeField] private LayerMask layerObjetosDeDaño; // Capas de objetos de daño
    [SerializeField] private float tiempoBloqueoEscudo = 0.5f; // Tiempo entre bloqueos

    private bool escudoBloqueado = false; // Si el escudo ha bloqueado recientemente
    private float tiempoUltimoBloqueo = -1f; // Último tiempo en que el escudo bloqueó

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto pertenece a las capas de daño
        if (((1 << other.gameObject.layer) & layerObjetosDeDaño) == 0)
            return;

        // Verificar si el escudo ya bloqueó recientemente
        if (escudoBloqueado && Time.time - tiempoUltimoBloqueo < tiempoBloqueoEscudo)
            return;

        // Si el escudo está activo, bloquear el daño
        Debug.Log($"El escudo bloqueó el ataque de {other.gameObject.name}");

        escudoBloqueado = true; // Registrar bloqueo
        tiempoUltimoBloqueo = Time.time; // Registrar el tiempo del bloqueo

        // Si necesitas un efecto visual o sonido, agrégalo aquí
    }

    private void Update()
    {
        // Resetear el estado del escudo después del tiempo de bloqueo
        if (escudoBloqueado && Time.time - tiempoUltimoBloqueo >= tiempoBloqueoEscudo)
        {
            escudoBloqueado = false; // Permitir otro bloqueo
        }
    }
}
