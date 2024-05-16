using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Meele : MonoBehaviour
{
    [SerializeField] private Transform player; 
    private float stoppingDistance = 2f; // Distancia a la que el enemigo se detiene del jugador
    private float attackCooldown = 2f; // Tiempo entre cada ataque
    private GameObject attackCollider; // Collider de ataque

    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent
    private bool isAttacking = false; // Indica si el enemigo está atacando

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackCollider.SetActive(false); // Desactivar collider de ataque al inicio
    }

    private void Update()
    {
        if (!isAttacking)
        {
            // Si no estamos atacando, perseguir al jugador
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // Configurar la velocidad del NavMeshAgent para que se mueva hacia el jugador
        navMeshAgent.SetDestination(player.position);

        // Si la distancia al jugador es menor que la distancia de parada, detenerse
        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            navMeshAgent.isStopped = true;

            // Activar el collider de ataque cuando estamos cerca del jugador
            attackCollider.SetActive(true);
            isAttacking = true;

            // Iniciar el temporizador de enfriamiento del ataque
            Invoke("ResetAttack", attackCooldown);
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void ResetAttack()
    {
        // Reiniciar el ataque después del tiempo de enfriamiento
        isAttacking = false;
        attackCollider.SetActive(false); // Desactivar collider de ataque
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            // Si entra en el trigger del jugador, dejar de perseguirlo y atacarlo
            navMeshAgent.isStopped = true;
            isAttacking = true;
            attackCollider.SetActive(true);
            // No hay necesidad de iniciar el temporizador de enfriamiento aquí,
            // ya que el ataque se desactivará cuando el jugador salga del trigger.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador sale del trigger, volver a perseguirlo
            navMeshAgent.isStopped = false;
            isAttacking = false;
            attackCollider.SetActive(false);
        }
    }
}
