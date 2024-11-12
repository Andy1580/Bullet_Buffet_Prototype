using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador
{
    public int indice;
    public int gamepadId;
    public string personaje;
    public int equipo;

    public Jugador(int indice,int gamepadId, string personaje, int equipo)
    {
        this.indice = indice;
        this.gamepadId = gamepadId;
        this.personaje = personaje;
        this.equipo = equipo;
    }
}
