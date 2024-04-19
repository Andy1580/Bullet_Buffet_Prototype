using System.Collections;
using UnityEngine;

public class DeadPlayerEvent : MonoBehaviour
{
    private GameObject playerReference1;
    private GameObject playerReference2;

    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;

    private GameObject playerATrasladar;

    private Vida vida1;
    private Vida vida2;

    PlayerController pla1;
    PlayerController pla2;

    private void Start()
    {
        playerReference1 = GameObject.FindGameObjectWithTag("Player1");
        
        vida1 = playerReference1.GetComponent<Vida>();
        pla1 = playerReference1.GetComponent<PlayerController>();

        if (playerReference1 != null)
        {
            Debug.LogWarning($"Se encontro el GameObject: {playerReference1}");
        }
        else
        {
            Debug.LogWarning($"No se encontro el GameObject: {playerReference1}");
        }

        if (vida1 != null)
        {
            Debug.LogWarning($"Se obtuvo el componente para: {vida1}");
        }
        else
        {
            Debug.LogWarning($"No se obtuvo el componente para: {vida1}");
        }

        if (pla1 != null)
        {
            Debug.LogWarning($"Se obtuvo el componente para: {pla1}");
        }
        else
        {
            Debug.LogWarning($"No se obtuvo el componente para: {pla1}");
        }

    }

    private void Update()
    {
        CheckVida();
        CheckPlayer2();
    }

    private void Awake()
    {
        
    }

    void CheckPlayer2()
    {
        playerReference2 = GameObject.FindGameObjectWithTag("Player2");
        vida2 = playerReference2.GetComponent<Vida>();
        pla2 = playerReference2.GetComponent<PlayerController>();

        if (playerReference2 != null)
        {
            Debug.LogWarning($"Se encontro el GameObject: {playerReference2}");
        }
        else
        {
            Debug.LogWarning($"No se encontro el GameObject: {playerReference2}");
        }

        if (vida2 != null)
        {
            Debug.LogWarning($"Se obtuvo el componente para: {vida2}");
        }
        else
        {
            Debug.LogWarning($"No se obtuvo el componente para: {vida2}");
        }

        if (pla2 != null)
        {
            Debug.LogWarning($"Se obtuvo el componente para: {pla2}");
        }
        else
        {
            Debug.LogWarning($"No se obtuvo el componente para: {pla2}");
        }
    }

    void CheckVida()
    {
        if (vida1 != null && vida2 != null)
        {
            if (vida1.salud <= 0)
            {
                Debug.LogWarning("El player 1 si murio, se agrego al player a trasladar y se inicio la corutina");
                playerATrasladar = playerReference1;
                StartCoroutine(PlayerTranslation());
            }
            else
            {
                Debug.LogWarning("No funciono el metodo de CheckVida con player 1");
            }
            if (vida2.salud <= 0)
            {
                Debug.LogWarning("El player 1 si murio, se agrego al player a trasladar y se inicio la corutina");
                playerATrasladar = playerReference2;
                StartCoroutine(PlayerTranslation());
            }
            else
            {
                Debug.LogWarning("No funciono el metodo de CheckVida con player 2");
            }
        }
    }

    void TrasladarPlayer()
    {
        if (playerATrasladar = playerReference1)
        {
            pla1.enabled = false;
            playerReference1.SetActive(false);
            Instantiate(playerATrasladar, spawn1.transform.position, Quaternion.identity);
        }

        else if (playerATrasladar = playerReference2)
        {
            pla2.enabled = false;
            playerReference2.SetActive(false);
            Instantiate(playerATrasladar, spawn2.transform.position, Quaternion.identity);
        }
    }

    IEnumerator PlayerTranslation()
    {
        TrasladarPlayer();
        vida1.salud = 100;
        vida2.salud = 100;
        yield return new WaitForSeconds(3f);
    }
}
