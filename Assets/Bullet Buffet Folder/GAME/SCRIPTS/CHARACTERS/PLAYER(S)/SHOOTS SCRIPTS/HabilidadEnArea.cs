using UnityEngine;

public class HabilidadEnArea : MonoBehaviour
{
    [SerializeField] private float areaDa�o = 3f;
    [SerializeField] private int da�o = 50;
    private PlayerController jugadorInvocador;


    public void ActivarHabilidad(PlayerController jugador)
    {
        jugadorInvocador = jugador;
        ActivarExplosion();
    }

    void ActivarExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaDa�o);

        foreach (Collider collider in colliders)
        {
            print(collider.gameObject);

            if (collider.gameObject == jugadorInvocador.gameObject)
            {
                continue;
            }

            if (collider.gameObject.layer == 8) //8 jugadores
            {
                PlayerController player = collider.gameObject.GetComponent<PlayerController>();

                player.Vida -= da�o;
            }

            if (collider.gameObject.layer == 7) //7 enemigos
            {
                EnemyAI_Flying eF = collider.gameObject.GetComponent<EnemyAI_Flying>();
                EnemyAI_Meele eM = collider.gameObject.GetComponent<EnemyAI_Meele>();

                if (eF != null)
                {
                    eF.VidaEnemigo -= da�o;
                }

                if (eM != null)
                {
                    eM.VidaEnemigo -= da�o;
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, areaDa�o);
    }
}
