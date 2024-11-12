using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnviarPlayerWins : MonoBehaviour
{
    public List<PlayerController> players; // Simula la lista de jugadores activos del Game Manager

    public void EnviarTeam1()
    {
        GuardarJugadoresPorEquipo(1); // Guarda el equipo 1 como ganador
    }

    public void EnviarTeam2()
    {
        GuardarJugadoresPorEquipo(2); // Guarda el equipo 2 como ganador
    }

    private void GuardarJugadoresPorEquipo(int equipoGanador)
    {
        // Guardar el equipo ganador en PlayerPrefs
        PlayerPrefs.SetInt("EquipoGanador", equipoGanador);

        // Guardar jugadores de cada equipo
        for (int i = 0; i < players.Count; i++)
        {
            PlayerPrefs.SetInt("Jugador_" + i + "_Equipo", players[i].equipo);
            PlayerPrefs.SetString("Jugador_" + i + "_Personaje", players[i].name);
        }

        PlayerPrefs.SetInt("TotalJugadores", players.Count); // Guarda el total de jugadores
        PlayerPrefs.Save();

        // Cargar la escena de victoria
        SceneManager.LoadScene("TESTVICTORY2");
    }
}
