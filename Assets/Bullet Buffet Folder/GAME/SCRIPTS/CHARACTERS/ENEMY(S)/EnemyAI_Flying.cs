using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI_Flying : MonoBehaviour
{
    [SerializeField] private float velocidadPersecucion = 5f;
    [SerializeField] private float rangoAtaque = 10f;
    [SerializeField] private float ataqueEnfriamiento = 2f;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform balaSpawn;
    [SerializeField] private float velocidadBala = 10f;
    [SerializeField] private float vidaBala = 3f;
    [SerializeField] private float distanciaMinima = 5f;
    [SerializeField] internal int vida = 200;

    private NavMeshAgent agente;
    private PlayerController jugadorObjetivo;
    private bool atacando = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = distanciaMinima;
        BuscarJugadorCercano();
    }

    void Update()
    {
        if (jugadorObjetivo != null)
        {
            if (!atacando)
            {
                PerseguirJugador(jugadorObjetivo);
            }
            ChecarVidaJugador();
        }

        else
        {
            agente.isStopped = false;
            agente.SetDestination(jugadorObjetivo.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.remainingTime <= 0)
        {
            agente.isStopped = true;
            atacando = false;
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {
            if (jugadorObjetivo.Vida <= 0)
            {
                jugadorObjetivo = null;
                agente.isStopped = true;
                Invoke("BuscarJugadorCercano", 5f);
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
            atacando = true;


            transform.LookAt(player.transform.localPosition);


            Invoke("Atacar", ataqueEnfriamiento);
        }
        else
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
        }
    }

    private void Atacar()
    {
        transform.LookAt(jugadorObjetivo.transform.position);

        Instantiate(balaPrefab, balaSpawn.position, balaSpawn.rotation);

        atacando = false;
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
}
