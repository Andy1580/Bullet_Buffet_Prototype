using System.Collections;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [Header("Explosive Bullet")]
    public float expansionDuration = 2f; // Duraci�n de la expansi�n
    public float maxRadius = 5f; // Radio m�ximo de expansi�n
    public float destructionDelay = 2f; // Tiempo antes de la destrucci�n despu�s de la expansi�n
    private SphereCollider sphereCollider;
    private bool isExpanding = false;
    public float velicidadBala = 50f;
    public int da�oExplosion = 100;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (velicidadBala * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Vida -= da�oExplosion;
                StartCoroutine(ExpandAndDestroy());
            }
        }
        else if(other.CompareTag("Enemy"))
        {
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();

            if (eM != null)
            {
                eM.VidaEnemigo -= da�oExplosion;
                StartCoroutine(ExpandAndDestroy());
            }

            if (eF != null)
            {
                eF.VidaEnemigo -= da�oExplosion;
                StartCoroutine(ExpandAndDestroy());
            }
            
        }
    }

    private IEnumerator ExpandAndDestroy()
    {
        Debug.Log("Empezo expancion");
        isExpanding = true;
        Vector3 initialScale = sphereCollider.transform.localScale;
        Vector3 targetScale = initialScale * maxRadius;

        float elapsedTime = 0f;
        while (elapsedTime < expansionDuration)
        {
            sphereCollider.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / expansionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sphereCollider.transform.localScale = targetScale;

        yield return new WaitForSeconds(destructionDelay);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
    }
}
