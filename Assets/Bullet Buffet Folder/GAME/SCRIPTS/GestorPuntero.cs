using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPuntero : MonoBehaviour
{
    // Diccionario para rastrear cuántos punteros están en cada objeto
    private Dictionary<GameObject, int> objetoPunteros = new Dictionary<GameObject, int>();

    public static GestorPuntero Instance { get; private set; }

    private void Awake()
    {
        // Asignar la instancia, pero sin persistencia
        Instance = this;
    }

    public void RegistrarEntrada(GameObject objeto)
    {
        if (!objetoPunteros.ContainsKey(objeto))
        {
            objetoPunteros[objeto] = 0;
        }

        objetoPunteros[objeto]++;
        CambiarSprite(objeto, true); // Cambiar sprite al entrar
    }

    public void RegistrarSalida(GameObject objeto)
    {
        if (!objetoPunteros.ContainsKey(objeto)) return;

        objetoPunteros[objeto]--;

        if (objetoPunteros[objeto] <= 0)
        {
            objetoPunteros[objeto] = 0; // Asegurar que no sea negativo
            CambiarSprite(objeto, false); // Restaurar sprite al salir
        }
    }

    private void CambiarSprite(GameObject objeto, bool entrando)
    {
        Image image = objeto.GetComponent<Image>();

        if (image == null) return;

        string objectName = objeto.name;

        if (entrando)
        {
            Sprite newSprite = Resources.Load<Sprite>($"out{objectName}");
            if (newSprite != null)
            {
                image.sprite = newSprite;
            }
        }
        else
        {
            Sprite originalSprite = Resources.Load<Sprite>($"n{objectName}");
            if (originalSprite != null)
            {
                image.sprite = originalSprite;
            }
        }
    }
}
