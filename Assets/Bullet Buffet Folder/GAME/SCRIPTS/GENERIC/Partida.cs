using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Partida
{
    public int[] controlesId;
    public string[] personaje;

    public Partida()
    {
        controlesId = new int[4];
        personaje = new string[4];
    }
}
