using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DañoVida : MonoBehaviour
{
    public int damage;
    public float invulnerabilityDuration = 1.5f;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Player3") || other.CompareTag("Player4"))
        {
            PlayerController pC = other.GetComponent<PlayerController>();
            if (pC != null && !pC.isInvulnerable)
            {
                pC.salud -= damage;
                pC.StartCoroutine(ActivateInvulnerability(pC));
            }
        }

        else if(other.CompareTag("Shield"))
        {
            Destroy(this.gameObject);
            Debug.Log("Le di al escudo");
            return;
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

    IEnumerator ActivateInvulnerability(PlayerController pC)
    {
        pC.isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        pC.isInvulnerable = false;
    }
}
