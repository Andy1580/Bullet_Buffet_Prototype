using System.Collections;
using UnityEngine;

public class DañoVida : MonoBehaviour
{
    public int damage;
    public float invulnerabilityDuration = 1.5f;

    private GameObject next;

    private void Start()
    {
        //next = GameObject.FindGameObjectWithTag("NextLevelSystem");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Vida vida = other.GetComponent<Vida>();
            if (vida != null && !vida.isInvulnerable)
            {
                vida.salud -= damage;
                //next.GetComponent<NextLevel>().tiempoActual = next.GetComponent<NextLevel>().tiempoInicial;
                vida.StartCoroutine(ActivateInvulnerability(vida));
                //gameObject.SetActive(false);
            }
        }
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.CompareTag("Player"))
    //    {
    //        Vida vida = col.gameObject.GetComponent<Vida>();
    //        if (vida != null && !vida.isInvulnerable)
    //        {
    //            vida.salud -= damage;
    //            vida.StartCoroutine(ActivateInvulnerability(vida));
    //            gameObject.SetActive(false);
    //        }
    //    }
    //}

    IEnumerator ActivateInvulnerability(Vida vida)
    {
        vida.isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        vida.isInvulnerable = false;
    }
}
