using UnityEngine;

public class DañoVida : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject vfxImpactoJugador;
    [SerializeField] private GameObject vfxImpactoObjeto;

    private PlayerController jugadorImpactoBala;

    public void Inicializar(PlayerController player)
    {
        jugadorImpactoBala = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerController jugador = other.GetComponent<PlayerController>();

            if(jugador.equipo == jugadorImpactoBala.equipo)
            {
                Debug.Log("Es del mismo equipo, no puedes hacerle daño");
                Destroy(this.gameObject);
                return;
            }
            else if(jugador.equipo != jugadorImpactoBala.equipo)
            {
                if (!jugador.isInvulnerable)
                {
                    if (jugador.Vida != 0)
                    {
                        jugador.Vida -= damage;
                        jugador.anim.SetTrigger("daño");
                        Vector3 puntoImpacto = other.ClosestPoint(transform.position);
                        Instantiate(vfxImpactoJugador, puntoImpacto, Quaternion.identity);
                        Destroy(this.gameObject);

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
                Destroy(this.gameObject);
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= damage;
                eF.animator.SetTrigger("daño");
                Destroy(this.gameObject);
            }
        }

        else if (other.gameObject.layer == 0)
        {
            Vector3 puntoImpacto = other.ClosestPoint(transform.position);
            Instantiate(vfxImpactoObjeto, puntoImpacto,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
