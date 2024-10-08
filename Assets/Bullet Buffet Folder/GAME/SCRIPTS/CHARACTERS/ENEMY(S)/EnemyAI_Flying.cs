using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI_Flying : MonoBehaviour
{

    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform balaSpawn;
    [SerializeField] private float distanciaMinima = 5f;
    [SerializeField] private int maxVida = 45;
    [SerializeField] internal int vida;
    [SerializeField] private SkinnedMeshRenderer renderer;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject vfxRespawn;
    [SerializeField] private Image barraVida;
    [SerializeField] private string nombre;
    [SerializeField] private GameObject[] objetosParaInstanciar;

    List<PlayerController> players;
    private NavMeshAgent agente;
    private PlayerController jugadorObjetivo;
    public float fuerzaSeparacion = 5f;
    public float minimaSeparacion = 4f;
    public float radioSeparacion = 2f;

    Rigidbody rb;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.stoppingDistance = distanciaMinima;
        vida = maxVida;

        

        GameObject clone = Instantiate(vfxRespawn, transform.position, transform.rotation);
        Destroy(clone, 1.5f);
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (jugadorObjetivo == null)
        {
            BuscarJugadorCercano();

        }

        barraVida.color = Color.yellow;

        rb = GetComponent<Rigidbody>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radioSeparacion;
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
                barraVida.fillAmount = (float)vida / maxVida;
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

            barraVida.fillAmount = (float)vida / maxVida;
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
        switch(nombre)
        {
            case "Medusa Alfa":
                InstanciarObjetoAleatorio();
                break;
        }

        Destroy(gameObject, 0.05f);
    }

    void InstanciarObjetoAleatorio()
    {
        if (objetosParaInstanciar.Length > 0)
        {
            int randomIndex = Random.Range(0, objetosParaInstanciar.Length);
            GameObject objetoSeleccionado = objetosParaInstanciar[randomIndex];



            Instantiate(objetoSeleccionado, transform.position, transform.rotation);
        }
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
        AudioManager.instance.PlaySound("disparomedusa");
        GameObject clone = Instantiate(balaPrefab, balaSpawn.position, balaSpawn.rotation);
        Destroy(clone, 8.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Vector3 posicionComplice = other.transform.position - transform.position;
            float distancia = posicionComplice.magnitude;

            if(distancia < minimaSeparacion)
            {
                Vector3 separacionDireccion = -posicionComplice.normalized;
                rb.AddForce(separacionDireccion * fuerzaSeparacion * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }
}
