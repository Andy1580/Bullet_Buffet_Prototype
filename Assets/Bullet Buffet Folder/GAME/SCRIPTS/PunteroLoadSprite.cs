using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunteroLoadSprite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Registrar entrada en el gestor
        GestorPuntero.Instance?.RegistrarEntrada(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Registrar salida en el gestor
        GestorPuntero.Instance?.RegistrarSalida(other.gameObject);
    }
}
