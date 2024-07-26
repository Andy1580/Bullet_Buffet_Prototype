using System.Collections.Generic;
using UnityEngine.InputSystem;

[System.Serializable]
public class InfoLobby
{
    public Dictionary<int, int> equipos = new Dictionary<int, int>(); // gamepadId -> equipo
    public Dictionary<int, string> personajes = new Dictionary<int, string>(); // gamepadId -> personaje
}
