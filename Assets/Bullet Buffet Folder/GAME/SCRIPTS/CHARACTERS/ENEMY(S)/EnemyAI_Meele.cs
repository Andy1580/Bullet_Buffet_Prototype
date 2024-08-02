using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Meele : MonoBehaviour
{
    [SerializeField] private float distanciaMinima = 2f;
    [SerializeField] private int maxVida = 200;
    [SerializeField] public GameObject attackCollider;
    [SerializeField] internal int vida;
    [SerializeField] private SkinnedMeshRenderer renderer;

    List<PlayerController> players;
    private NavMeshAgent agente;
    private PlayerController jugadorObjetivo;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = distanciaMinima;
        vida = maxVida;
        attackCollider.SetActive(false);

        if (renderer == null)
        {
            renderer = GetComponent<SkinnedMeshRenderer>();

        }
        BuscarJugadorCercano();
    }

    void Update()
    {
        //if (jugadorObjetivo != null)
        //{
        //    if (!atacando)
        //    {
        //        PerseguirJugador(jugadorObjetivo);
        //    }
        //}

        //if (GameManager.remainingTime <= 0)
        //{
        //    agente.isStopped = true;
        //    DeadEvent();
        //}

        //if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        //{
        //    if (jugadorObjetivo.Vida <= 0)
        //    {
        //        jugadorObjetivo = null;
        //        agente.isStopped = true;
        //        Invoke("BuscarJugadorCercano", 2.5f);
        //    }
        //}
    }

    private void FixedUpdate()
    {
        transform.LookAt(jugadorObjetivo.transform);

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

    //        AtacarJugador(player);
    //    }
    //    else
    //    {
    //        agente.isStopped = false;
    //        agente.SetDestination(player.transform.position);
    //        atacando = false;
    //    }
    //}

    //private void AtacarJugador(PlayerController player)
    //{
    //    if (Vector3.Distance(transform.position, player.transform.position) <= rangoAtaque)
    //    {
    //        agente.isStopped = true;

    //        atacando = true;

    //        transform.LookAt(player.transform.position);

    //        //Aqui ira la animacion de ataque
    //        attackCollider.SetActive(true);

    //        Invoke("ResetAttack", ataqueEnfriamiento);

    //    }
    //}

    //private void ResetAttack()
    //{
    //    atacando = false;
    //    attackCollider.SetActive(false);
    //}

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
                StopAllCoroutines();
                Invoke("DeadEvent", 3.5f);
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

    private void DeadEvent()
    {
        Destroy(gameObject);
    }

    IEnumerator PerseguirJugador()
    {
    Inicio:
        agente.SetDestination(jugadorObjetivo.transform.position);
        yield return new WaitForSeconds(1);
        goto Inicio;
    }

    IEnumerator AtacarJugador()
    {
    Inicio:

        float distancia = Vector3.Distance(transform.position, jugadorObjetivo.transform.position);

        attackCollider.SetActive(false);

        if (distancia <= distanciaMinima)
        {
            Ataque();
        }
        yield return new WaitForSeconds(2f);
        goto Inicio;
    }

    void Ataque()
    {
        attackCollider.SetActive(true);
    }
}
