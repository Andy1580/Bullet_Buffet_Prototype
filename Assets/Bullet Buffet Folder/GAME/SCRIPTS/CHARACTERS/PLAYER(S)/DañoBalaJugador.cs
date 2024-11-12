using UnityEngine;

public class DañoBalaJugador : MonoBehaviour
{
    [SerializeField] private int daño;

    private void OnTriggerEnter(Collider other)
    {
        //print("Colision con: " + other.gameObject.name);

        if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if (!jugador.isInvulnerable || jugador.muerto)
            {
                jugador.Vida -= daño;
                jugador.animator.SetTrigger("daño");
                //Vector3 puntoImpacto = other.ClosestPoint(transform.position);
                //Instantiate(vfxImpactoJugador, puntoImpacto, Quaternion.identity);
            }

        }

        else if (other.gameObject.layer == 7) //Layer Enemy = 7
        {
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();

            if (eM != null)
            {
                eM.VidaEnemigo -= daño;
                eM.animator.SetTrigger("daño");
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= daño;
                eF.animator.SetTrigger("daño");
            }
        }

        //else if (other.gameObject.layer == 0)
        //{
        //    Vector3 puntoImpacto = other.ClosestPoint(transform.position);
        //    //Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);
        //}
    }
}
