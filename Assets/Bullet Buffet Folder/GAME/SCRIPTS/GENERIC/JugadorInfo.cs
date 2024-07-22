using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorInfo : MonoBehaviour
{
    public int? gamepadId {  get; set; }
    public string Personaje { get; set; }

    public int Equipo { get; set; }

    public JugadorInfo(int gamepadId, string personaje)
    {
        this.gamepadId = gamepadId;
        Personaje = personaje;
    }


}
