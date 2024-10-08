using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[System.Serializable]
public class InfoLobby
{
    [System.Serializable]
    public class PlayerInfo
    {
        public int gamepadId;
        public int equipo;
        public string personaje;
    }

    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    public void AddPlayerInfo(int gamepad, int equipo, string personaje)
    {
        playerInfos.Add(new PlayerInfo
        {
            gamepadId = gamepad,
            equipo = equipo,
            personaje = personaje
        });
        
        //Debug.Log($"PlayerInfo agregado: GamepadId {gamepad}, Equipo {equipo}, Personaje {personaje}");
        
    }
}
