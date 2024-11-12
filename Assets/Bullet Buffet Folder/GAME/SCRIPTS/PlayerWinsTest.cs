using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWinsTest : MonoBehaviour
{
    public Transform[] posicionesWin;  // Posiciones para ganadores
    public Transform[] posicionesLose; // Posiciones para perdedores

    public GameObject pfCRIM;
    public GameObject pfKAI;
    public GameObject pfNOVA;
    public GameObject pfSKYIE;

    private void Start()
    {
        InstanciarJugadores();
    }

    private void InstanciarJugadores()
    {
        int equipoGanador = PlayerPrefs.GetInt("EquipoGanador");
        int totalJugadores = PlayerPrefs.GetInt("TotalJugadores");

        List<GameObject> ganadores = new List<GameObject>();
        List<GameObject> perdedores = new List<GameObject>();

        // Clasificar jugadores en ganadores y perdedores
        for (int i = 0; i < totalJugadores; i++)
        {
            int equipoJugador = PlayerPrefs.GetInt("Jugador_" + i + "_Equipo");
            string personaje = PlayerPrefs.GetString("Jugador_" + i + "_Personaje");

            GameObject prefab = SeleccionarPrefab(personaje);
            if (equipoJugador == equipoGanador)
                ganadores.Add(prefab);
            else
                perdedores.Add(prefab);
        }

        // Instanciar ganadores en posicionesWin y perdedores en posicionesLose
        InstanciarListaEnPosiciones(ganadores, posicionesWin, "ganador");
        InstanciarListaEnPosiciones(perdedores, posicionesLose, "perdedor");
    }

    private GameObject SeleccionarPrefab(string personaje)
    {
        // Devuelve el prefab correspondiente según el nombre del personaje
        switch (personaje)
        {
            case "CRIM": return pfCRIM;
            case "KAI": return pfKAI;
            case "NOVA": return pfNOVA;
            case "SKYIE": return pfSKYIE;
            default: return null;
        }
    }

    private void InstanciarListaEnPosiciones(List<GameObject> lista, Transform[] posiciones, string trigger)
    {
        for (int i = 0; i < lista.Count && i < posiciones.Length; i++)
        {
            GameObject instancia = Instantiate(lista[i], posiciones[i].position, Quaternion.Euler(0f, 180f, 0f));

            // Activar el trigger correspondiente en el Animator
            Animator animator = instancia.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }
    }
}
