using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Flying : MonoBehaviour
{

    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform balaSpawn;
    [SerializeField] private float distanciaMinima = 5f;
    [SerializeField] private int maxVida = 45;
    [SerializeField] internal int vida;
    [SerializeField] private SkinnedMeshRenderer renderer;
    [SerializeField] public Animator animator;

    List<PlayerController> players;
    private NavMeshAgent agente;
    private PlayerController jugadorObjetivo;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = distanciaMinima;
        vida = maxVida;

        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (jugadorObjetivo == null)
        {
            BuscarJugadorCercano();

        }
    }

    void Update()
    {
        //if (jugadorObjetivo != null)
        //{
        //    if (!atacando)
        //    {
        //        StartCoroutine(PerseguirJugador());
        //        PerseguirJugador(jugadorObjetivo);
        //    }
        //    ChecarVidaJugador();
        //}

        //else
        //{
        //    agente.isStopped = false;
        //    agente.SetDestination(jugadorObjetivo.transform.position);
        //}
    }

    private void FixedUpdate()
    {
        if (jugadorObjetivo != null)
        {
            transform.LookAt(jugadorObjetivo.transform);
        }
        else
        {
            BuscarJugadorCercano();
        }

        //if (GameManager.remainingTime <= 0)
        //{
        //    agente.isStopped = true;
        //    atacando = false;
        //    Destroy(gameObject);
        //}

        if (jugadorObjetivo.Vida <= 0)
        {
            JugadorMuerto();
        }
    }

    private void BuscarJugadorCercano()
    {
        players = GameManager.activePlayers;
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
        StartCoroutine(PerseguirJugador());
        StartCoroutine(AtacarJugador());
    }

    //private void PerseguirJugador(PlayerController player)
    //{
    //    if (Vector3.Distance(transform.position, player.transform.position) <= distanciaMinima)
    //    {
    //        agente.isStopped = true;
    //        atacando = true;


    //        transform.LookAt(player.transform.localPosition);


    //        Invoke("Atacar", ataqueEnfriamiento);
    //    }
    //    else
    //    {
    //        agente.isStopped = false;
    //        agente.SetDestination(player.transform.position);
    //    }
    //}

    //private void Atacar()
    //{
    //    transform.LookAt(jugadorObjetivo.transform.position);

    //    Instantiate(balaPrefab, balaSpawn.position, balaSpawn.rotation);

    //    atacando = false;
    //}

    //private void ChecarVidaJugador()
    //{
    //    if (players.Count <= 1)
    //    {
    //        BuscarJugadorCercano();
    //    }
    //}

    private void JugadorMuerto()
    {
        if (players.Count <= 1)
        {
            StopAllCoroutines();
            return;
        }

        if (players[0].Vida > 0)
        {
            jugadorObjetivo = players[0];
        }

        if (players[1].Vida > 0)
        {
            jugadorObjetivo = players[1];
        }
    }

    public int VidaEnemigo
    {
        get => vida;
        set
        {
            if (value < vida)
            {
                StartCoroutine(DañoEmisivo());
            }

            if (value <= 0)
            {
                vida = 0;
                DeadEvent();
            }
            else if (value >= maxVida)
            {
                vida = maxVida;
            }
            else
            {
                vida = value;
            }
        }
    }

    private IEnumerator DañoEmisivo()
    {
        renderer.material.SetColor("_EmissionColor", Color.white * 2);
        yield return new WaitForSeconds(0.1f);
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    void DeadEvent()
    {
        Destroy(gameObject);
    }

    IEnumerator PerseguirJugador()
    {
    Inicio:
        agente.SetDestination(jugadorObjetivo.transform.position);
        animator.SetBool("perseguir", true);
        yield return new WaitForSeconds(1);
        goto Inicio;
    }

    IEnumerator AtacarJugador()
    {
    Inicio:

        float distancia = Vector3.Distance(transform.position, jugadorObjetivo.transform.position);

        if (distancia <= distanciaMinima)
        {
            transform.LookAt(jugadorObjetivo.transform.position);
            animator.SetBool("ataque", true);
            Invoke("Ataque", 0.30f);
        }
        else
        {
            animator.SetBool("ataque", false);
        }
        yield return new WaitForSeconds(2f);
        goto Inicio;
    }

    void Ataque()
    {
        GameObject clone = Instantiate(balaPrefab, balaSpawn.position, balaSpawn.rotation);
        Destroy(clone, 8.0f);
    }
}
