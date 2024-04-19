using System.Collections;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    [SerializeField] private Transform respawnPoint1;
    [SerializeField] private Transform respawnPoint2;

    private Vida vida1;
    private Vida vida2;

    private PlayerController playerController1;
    private PlayerController playerController2;

    private void Awake()
    {
        Start_FindPlayers();
    }

    private void Start()
    {
        Awake_CheckComponents();


    }

    private void Update()
    {
        Update_CheckHealthPlayer();
    }

    private void FixedUpdate()
    {
        if (player2 == null)
        {
            Debug.LogWarning("No se encontro p2 para RESPAWN");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            Debug.LogWarning("Se encontro p2 para RESPAWN");

        }
        if (player2 != null)
        {
            if (vida2.salud <= 0)
            {
                Debug.Log("Si se encontro la vida1");
                player2.SetActive(false);
                StartCoroutine(RespawnCorutine2());
            }

        }
    }

    private void Start_FindPlayers()
    {
        if (player1 == null)
        {
            Debug.LogWarning("No se encontro p1 para RESPAWN");
            player1 = GameObject.FindGameObjectWithTag("Player1");
        }
        else
        {
            Debug.LogWarning("Se encontro p1 para RESPAWN");

        }

        if (player2 == null)
        {
            Debug.LogWarning("No se encontro p2 para RESPAWN");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            Debug.LogWarning("Se encontro p2 para RESPAWN");

        }

        //Debug.Log(player2.gameObject.name);
    }

    private void Awake_CheckComponents()
    {
        if (player1 != null)
        {
            Debug.LogWarning("Se encontro p1");
            vida1 = player1.GetComponent<Vida>();
            playerController1 = player1.GetComponent<PlayerController>();
        }

        if (player2 != null)
        {
            Debug.LogWarning("Se encontro p2");
            vida2 = player2.GetComponent<Vida>();
            playerController2 = player2.GetComponent<PlayerController>();
        }
    }

    void Update_CheckHealthPlayer()
    {

        if (vida1.salud <= 0)
        {
            Debug.Log("Si se encontro la vida1");
            player1.SetActive(false);
            StartCoroutine(RespawnCorutine1());
        }


    }

    IEnumerator RespawnCorutine1()
    {
        playerController1.enabled = false;
        Instantiate(player1, respawnPoint1.position, respawnPoint1.rotation);
        //Translate.Equals(player1.transform.position, respawnPoint1.transform.position.normalized);
        yield return new WaitForSeconds(4f);
        player1.SetActive(true);
        vida1.salud = 100;
        yield return new WaitForSeconds(2f);
        playerController1.enabled = true;
        StopAllCoroutines();
    }

    IEnumerator RespawnCorutine2()
    {
        playerController2.enabled = false;
        Instantiate(player2, respawnPoint2.position, respawnPoint2.rotation);
        //Translate.Equals(player2.transform.position, respawnPoint2.transform.position.normalized);
        yield return new WaitForSeconds(4f);
        player2.SetActive(true);
        vida2.salud = 100;
        yield return new WaitForSeconds(2f);
        playerController2.enabled = true;
        StopAllCoroutines();
    }
}
