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
                if (pC.Vida != 0)
                {
                    pC.Vida -= damage;
                    Destroy(this.gameObject);

                }
                else return;
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
                eM.animator.SetTrigger("daño");
                Destroy(this.gameObject);
            }
            else if (eF != null)
            {
                eF.VidaEnemigo -= damage;
                eF.animator.SetTrigger("daño");
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
