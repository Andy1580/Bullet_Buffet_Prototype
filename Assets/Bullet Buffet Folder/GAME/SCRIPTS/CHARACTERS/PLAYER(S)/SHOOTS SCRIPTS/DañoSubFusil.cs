using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoSubFusil : MonoBehaviour
{
    [SerializeField] private int damage;
    private PlayerController propietario;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        print("Dentro del area de daño: " + other.gameObject.name);

        if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if (jugador.equipo == propietario.equipo)
            {
                Debug.Log("Es del mismo equipo, no puedes hacerle daño");
            }
            else
            {
                if (!jugador.isInvulnerable)
                {
                    if (jugador.Vida != 0)
                    {
                        jugador.Vida -= damage;
                        jugador.anim.SetTrigger("daño");
                        Vector3 puntoImpacto = other.ClosestPoint(transform.position);
                        //Instantiate(vfxImpactoJugador, puntoImpacto, Quaternion.identity);

                    }
                    else return;
                }
            }

        }

        else if (other.gameObject.layer == 7) //Layer Enemy = 7
        {
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();

            if (eM != null)
            {
                eM.VidaEnemigo -= damage;
                eM.animator.SetTrigger("daño");
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= damage;
                eF.animator.SetTrigger("daño");
            }
        }
    }
}
