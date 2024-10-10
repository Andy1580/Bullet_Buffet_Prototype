using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HabilidadSub : MonoBehaviour
{
    private PlayerController jugadorInvocador;
    [SerializeField] private float areaDaño = 5f;       
    [SerializeField] private int daño = 10;          
    [SerializeField] private float duracionHabilidad = 3f;  
    [SerializeField] private float intervaloDaño = 0.5f;    
    [SerializeField] private LayerMask capas;    

    public void ActivarHabilidad(PlayerController jugador)
    {
        jugadorInvocador = jugador;
        StartCoroutine(DañoConstanteEnArea());
    }

    IEnumerator DañoConstanteEnArea()
    {
        float tiempoRestante = duracionHabilidad;

        while (tiempoRestante > 0f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, areaDaño, capas);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == jugadorInvocador.gameObject)
                {
                    continue;  // Saltar al siguiente collider
                }

                if (collider.gameObject.layer == 8)  // layer 8 = Players
                {
                    PlayerController player = collider.gameObject.GetComponent<PlayerController>();

                    if (player != null)
                    {
                        player.Vida -= daño;
                    }
                }

                if(collider.gameObject.layer == 7) // layer 7 = Enemy
                {
                    EnemyAI_Flying eF = collider.gameObject.GetComponent<EnemyAI_Flying>();
                    EnemyAI_Meele eM = collider.gameObject.GetComponent<EnemyAI_Meele>();

                    if(eM != null)
                    {
                        eM.VidaEnemigo -= daño;
                    }
                    
                    if(eF != null)
                    {
                        eF.VidaEnemigo -= daño;
                    }
                }
            }

            yield return new WaitForSeconds(intervaloDaño);

            tiempoRestante -= intervaloDaño;  // Reducir el tiempo restante de la habilidad
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaDaño);
    }
}
