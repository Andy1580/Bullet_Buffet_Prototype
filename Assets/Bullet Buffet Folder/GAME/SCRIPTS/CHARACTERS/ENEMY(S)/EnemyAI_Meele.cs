using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Meele : MonoBehaviour
{
    [SerializeField] private Transform player; 
    private float stoppingDistance = 2f; 
    private float attackCooldown = 2f; 
    public GameObject attackCollider;

    private NavMeshAgent navMeshAgent; 
    private bool isAttacking = false; 

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackCollider.SetActive(false); 
    }

    private void Update()
    {
        if (!isAttacking)
        {   
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

            attackCollider.SetActive(true);
            isAttacking = true;

            Invoke("ResetAttack", attackCooldown);
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        attackCollider.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            navMeshAgent.isStopped = true;
            isAttacking = true;
            attackCollider.SetActive(true);
            // No hay necesidad de iniciar el temporizador de enfriamiento aquí,
            // ya que el ataque se desactivará cuando el jugador salga del trigger.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            navMeshAgent.isStopped = false;
            isAttacking = false;
            attackCollider.SetActive(false);
        }
    }
}
