using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinsTest : MonoBehaviour
{
    public Transform[] posicionesWin;  // Posiciones para ganadores
    public Transform[] posicionesLose; // Posiciones para perdedores

    public Transform[] posicioneesTrasformsWins;
    public Transform[] posicioneesTrasformsLose;

    public GameObject pfCRIM;
    public GameObject pfKAI;
    public GameObject pfNOVA;
    public GameObject pfSKYIE;

    public Button botonMenu;
    public Button botonMapas;
    public Button botonLobby;

    int totalPlayers;

    private bool opcionSeleccionada;

    private void Awake()
    {
        //Iniciar musica escena victoria
        totalPlayers = GameManager.Instance.nJugadores;

        GameManager.Instance.OcultarElementosImportantes();

        if (totalPlayers == 2)
        {
            posicionesWin[0].position = posicioneesTrasformsWins[0].position;
            posicionesLose[0].position = posicioneesTrasformsLose[0].position;
        }
        else
        {
            posicionesWin[0].position = posicioneesTrasformsWins[1].position;
            posicionesWin[1].position = posicioneesTrasformsWins[2].position;
            posicionesLose[0].position = posicioneesTrasformsLose[1].position;
            posicionesLose[1].position = posicioneesTrasformsLose[2].position;
        }

        AudioManager.instance.StopSound("audiencia");
    }

    private void Start()
    {
        AudioManager.instance.StopSound("hechizos");
        AudioManager.instance.StopSound("duelo");
        AudioManager.instance.PlaySound("victoria");
        InstanciarJugadores();

        opcionSeleccionada = false;

        EventSystem.current.SetSelectedGameObject(botonMenu.gameObject);
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

    public void RegresarAlMenu()
    {
        if (!opcionSeleccionada)
        {
            opcionSeleccionada = true;
            StartCoroutine(CargarMenuPrincipal());
            AudioManager.instance.PlaySound("botonmenu");
        }
    }

    public void RegresarAlLobby()
    {
        if (!opcionSeleccionada)
        {
            opcionSeleccionada = true;
            StartCoroutine(CargarLobby());
            AudioManager.instance.PlaySound("botonmenu");
        }
    }

    public void RegresarSeleccionMapa()
    {
        if (!opcionSeleccionada)
        {
            opcionSeleccionada = true;
            StartCoroutine(CargarSeleccionMapa());
            AudioManager.instance.PlaySound("botonmenu");
        }
    }

    IEnumerator CargarMenuPrincipal()
    {
        Debug.Log("Se selecciono regresar al menu");
        GameManager.Instance.IniciarCorutinaTransicion();
        MainMenuSystem.instance.ResetarBooleanosImportantesMS();
        GameManager.Instance.ResetarBooleanosImportantesGM();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    IEnumerator CargarSeleccionMapa()
    {
        Debug.Log("Se selecciono escoger mapa");
        GameManager.Instance.IniciarCorutinaTransicion();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    IEnumerator CargarLobby()
    {
        Debug.Log("Se selecciono Lobby");
        GameManager.Instance.IniciarCorutinaTransicion();
        GameManager.Instance.ResetearVariablesEnLobby();
        yield return new WaitForSeconds(2f);
        AudioManager.instance.StopSound("victoria");
        AudioManager.instance.PlaySound("menu");
        SceneManager.LoadScene("LOBBY");
    }
}
