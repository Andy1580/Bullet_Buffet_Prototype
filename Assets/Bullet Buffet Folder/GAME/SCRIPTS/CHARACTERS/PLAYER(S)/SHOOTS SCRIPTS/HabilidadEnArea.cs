using UnityEngine;

public class HabilidadEnArea : MonoBehaviour
{
    [SerializeField] private float areaDaño = 3f;
    [SerializeField] private int daño = 50;
    private PlayerController jugadorInvocador;


    public void ActivarHabilidad(PlayerController jugador)
    {
        jugadorInvocador = jugador;
        ActivarExplosion();
    }

    void ActivarExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaDaño);

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

                player.Vida -= daño;
            }

            if (collider.gameObject.layer == 7) //7 enemigos
            {
                EnemyAI_Flying eF = collider.gameObject.GetComponent<EnemyAI_Flying>();
                EnemyAI_Meele eM = collider.gameObject.GetComponent<EnemyAI_Meele>();

                if (eF != null)
                {
                    eF.VidaEnemigo -= daño;
                }

                if (eM != null)
                {
                    eM.VidaEnemigo -= daño;
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, areaDaño);
    }
}
