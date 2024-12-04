using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DañoHabilidadEscopeta : MonoBehaviour
{
    [SerializeField] private int daño;
    [SerializeField] private GameObject vfxImpacto;
    private PlayerController propietario;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) //layer 10 = obstaculo
        {
            return;
            //Vector3 puntoImpacto = other.ClosestPoint(transform.position);
            //Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);
        }

        else if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if (jugador.equipo == propietario.equipo)
            {
                Debug.Log("Es del mismo equipoJugador, no puedes hacerle daño");
                return;
            }
            else
            {
                if (jugador.Vida > 0)
                {
                    if (!jugador.isInvulnerable || !jugador.muerto)
                    {
                        jugador.Vida -= daño;
                        Vector3 puntoImpacto = other.ClosestPoint(transform.position);
                        Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
                    }
                }
                else return;
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


    }
}
