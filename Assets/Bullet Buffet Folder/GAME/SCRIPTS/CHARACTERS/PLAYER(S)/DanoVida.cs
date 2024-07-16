using UnityEngine;

public class DañoVida : MonoBehaviour
{
    [SerializeField] private int damage;
    //[SerializeField] private float invulnerabilityDuration = 1.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pC = other.GetComponent<PlayerController>();
            if (pC != null && !pC.isInvulnerable)
            {
                pC.Vida -= damage;
                Destroy(this.gameObject);
                //pC.StartCoroutine(ActivateInvulnerability(pC));
            }
        }

        else if (other.CompareTag("Enemy"))
        {
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();

            if (eM != null)
            {
                eM.VidaEnemigo -= damage;
                Debug.Log("Le hize daño al eM");
                Destroy(this.gameObject);
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= damage;
                Debug.Log("Le hize daño al eF");
                Destroy(this.gameObject);
            }
        }

        else if (other.CompareTag("Shield"))
        {
            Destroy(this.gameObject);
        }
    }

    //IEnumerator ActivateInvulnerability(PlayerController pC)
    //{
    //    pC.isInvulnerable = true;
    //    yield return new WaitForSeconds(invulnerabilityDuration);
    //    pC.isInvulnerable = false;
    //}
}
