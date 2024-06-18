using System.Collections.Generic;
using UnityEngine.InputSystem;

public class JugadorInfo
{
    public int gamepadId {  get; set; }
    public string Personaje { get; set; }

    public JugadorInfo(int gamepadId, string personaje)
    {
        this.gamepadId = gamepadId;
        Personaje = personaje;
    }

}
