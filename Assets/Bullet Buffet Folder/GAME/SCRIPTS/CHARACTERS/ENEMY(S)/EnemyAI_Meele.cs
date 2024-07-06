using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI_Meele : MonoBehaviour
{
    [SerializeField] private float distanciaMinima = 2f;
    [SerializeField] private float ataqueEnfriamiento = 1.5f;
    [SerializeField] private float rangoAtaque = 1.5f;
    [SerializeField] internal int vida = 200;
    [SerializeField] public GameObject attackCollider;

    private bool atacando;
    private NavMeshAgent agente;
    private PlayerController jugadorObjetivo;
    private float proximoAtaque;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        BuscarJugadorCercano();
        attackCollider.SetActive(false);
    }

    void Update()
    {
        if (jugadorObjetivo != null)
        {
            if (!atacando)
            {
                PerseguirJugador(jugadorObjetivo);
                ChecarVidaJugador();

            }
        }

        if (vida <= 0)
        {
            Destroy(gameObject);
        }

        if (GameManager.remainingTime <= 0)
        {
            agente.isStopped = true;
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {
            if (jugadorObjetivo.Vida <= 0)
            {
                jugadorObjetivo = null;
                agente.isStopped = true;
                Invoke("BuscarJugadorCercano", 2.5f);
            }
        }
    }

    private void BuscarJugadorCercano()
    {
        List<PlayerController> players = GameManager.jugadores;
        float closestDistance = Mathf.Infinity;
        PlayerController closestPlayer = null;

        foreach (PlayerController player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        jugadorObjetivo = closestPlayer;
    }

    private void PerseguirJugador(PlayerController player)
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= distanciaMinima)
        {
            agente.isStopped = true;

            AtacarJugador(player);
        }
        else
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            atacando = false;
        }
    }

    private void AtacarJugador(PlayerController player)
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= rangoAtaque)
        {
            agente.isStopped = true;

            atacando = true;

            transform.LookAt(player.transform.position);

            //Aqui ira la animacion de ataque
            attackCollider.SetActive(true);

            Invoke("ResetAttack", ataqueEnfriamiento);

        }
    }

    private void ResetAttack()
    {
        atacando = false;
        attackCollider.SetActive(false);
    }

    private void ChecarVidaJugador()
    {
        if (jugadorObjetivo.Vida <= 0)
        {
            BuscarJugadorCercano();
        }
    }

    public void RecibirDaño(int daño)
    {
        vida -= daño;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si entra en el trigger del jugador, dejar de perseguirlo y atacarlo
            agente.isStopped = true;
            atacando = true;
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
            agente.isStopped = false;
            atacando = false;
            attackCollider.SetActive(false);
        }
    }
}
