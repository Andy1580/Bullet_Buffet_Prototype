using System;
using UnityEngine;

public class DañoEscopeta : MonoBehaviour
{
    [SerializeField] private int daño;
    private PlayerController propietario;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Colision con: " + other.gameObject.name);

        if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if(jugador.equipo == propietario.equipo)
            {
                Debug.Log("Es del mismo equipo, no puedes hacerle daño");
            }
            else
            {
                if (!jugador.isInvulnerable)
                {
                    if (jugador.Vida != 0)
                    {
                        jugador.Vida -= daño;
                        //jugador.anim.SetTrigger("daño");
                        //Vector3 puntoImpacto = other.ClosestPoint(transform.position);
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
                eM.VidaEnemigo -= daño;
                //eM.animator.SetTrigger("daño");
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= daño;
                //eF.animator.SetTrigger("daño");
            }
        }

        //else if (other.gameObject.layer == 0)
        //{
        //    Vector3 puntoImpacto = other.ClosestPoint(transform.position);
        //    //Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);
        //}
    }

    private void OnDestroy()
    {
        Debug.LogWarning("Se destruyo el Collider de la Escopeta");
    }
}
