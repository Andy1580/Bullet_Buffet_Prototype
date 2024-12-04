using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoBate : MonoBehaviour
{
    [SerializeField] private int daño;
    private PlayerController propietario;
    public bool escudoBloqueado = false;
    public GameObject vfxImpacto;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        // Si el escudo ya bloqueó, ignoramos el resto
        if (escudoBloqueado) return;

        // Verificar si colisionó con el escudo (Layer 10)
        if (other.gameObject.layer == 10)
        {
            escudoBloqueado = true; // Marcar que el escudo bloqueó
            Debug.Log("El escudo bloqueó el daño");
            Vector3 puntoImpacto = other.ClosestPoint(transform.position);
            Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
            return; // Detener el procesamiento
        }

        //Vector3 puntoImpacto = other.ClosestPoint(transform.position);
        //Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);


        // Procesar colisión con jugadores
        if (other.gameObject.layer == 8) // Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if (jugador == null)
                return;

            if (jugador.equipo == propietario.equipo)
            {
                Debug.Log("Es del mismo equipo, no puedes hacerle daño");
                return;
            }
            else
            {
                if (escudoBloqueado) return;

                if (jugador.Vida > 0 && !jugador.isInvulnerable && !jugador.muerto)
                {
                    jugador.Vida -= daño;
                    Vector3 puntoImpacto = other.ClosestPoint(transform.position);
                    Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
                    Debug.Log($"Jugador {jugador.name} recibió daño: {daño}");
                }
            }
        }

        else if (other.gameObject.layer == 7) //Layer Enemy = 7
        {
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();

            if (eM != null)
            {
                eM.VidaEnemigo -= daño;
                //eM.animator.SetTrigger("daño");
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= daño;
                //eF.animator.SetTrigger("daño");
            }
        }

        else if (other.gameObject.layer == 10) // Layer Obstáculos/Escudo
        {
            Debug.Log("El daño fue bloqueado por un obstáculo.");
            Vector3 puntoImpacto = other.ClosestPoint(transform.position);
            Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
            return; // Salir inmediatamente
        }

    }

    private void OnDisable()
    {
        escudoBloqueado = false;
    }

    private void OnDestroy()
    {
        Debug.LogWarning("Se destruyo el Collider del Bate");
    }
}
