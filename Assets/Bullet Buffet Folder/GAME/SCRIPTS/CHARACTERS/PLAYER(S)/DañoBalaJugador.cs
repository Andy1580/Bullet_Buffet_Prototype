using UnityEngine;

public class DañoBalaJugador : MonoBehaviour
{
    [SerializeField] private int daño;
    private PlayerController propietario;
    public GameObject vfxImpacto;

    public void IniciarBala(PlayerController player)
    {
        propietario = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Colision con: " + other.gameObject.name);

        if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if(jugador.equipo == propietario.equipo)
            {
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
                        Destroy(this.gameObject);
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

        else if (other.gameObject.layer == 10) //layer 10 de obstaculo
        {
            Vector3 puntoImpacto = other.ClosestPoint(transform.position);
            Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
            Destroy(this.gameObject);
        }
        //Vector3 puntoImpacto = other.ClosestPoint(transform.position);
        ////Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);
    }
}
