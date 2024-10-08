using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoExplosion : MonoBehaviour
{
    public int dañoExplosion = 50;

    [HideInInspector]
    public PlayerController jugadorInvocador;

    public void RecibirJugador(PlayerController jugador)
    {
        jugadorInvocador = jugador;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();
        EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();

        if (other.gameObject == jugadorInvocador)
        {
            return;
        }
        if (other.gameObject.layer == 8 || other.gameObject.layer == 7) //8 jugadores / 7 enemigos
        {
            if (player != null)
            {
                player.Vida -= dañoExplosion;
            }

            if (eM != null)
            {
                eM.VidaEnemigo -= dañoExplosion;
            }

            if (eF != null)
            {
                eF.VidaEnemigo -= dañoExplosion;
            }
        }
    }
}
