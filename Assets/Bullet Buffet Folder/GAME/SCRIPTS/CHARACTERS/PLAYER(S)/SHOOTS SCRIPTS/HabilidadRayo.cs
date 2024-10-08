using System.Collections;
using UnityEngine;

public class HabilidadRayo : MonoBehaviour
{
    [Header("Rayo Core")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private int damage = 50;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private LayerMask damageableLayers;
    [SerializeField] private ParticleSystem rayVFX;
    [SerializeField] private float vfxSpeed = 5f;

    public void ActivarHabilidad()
    {
        FireRay();
    }

    void Start()
    {
        lineRenderer = rayOrigin.GetComponentInChildren<LineRenderer>();
        rayVFX = GetComponent<ParticleSystem>();

        lineRenderer.enabled = false;
        //lineRenderer.positionCount = 2; // puntos (origen y final)
        //lineRenderer.SetPosition(0, rayOrigin.position);
        //lineRenderer.SetPosition(1, rayOrigin.position);
    }

    void FireRay()
    {
        lineRenderer.enabled = true;
        //lineRenderer.SetPosition(0, rayOrigin.position);

        //Vector3 endPosition;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit[] hit = Physics.RaycastAll(ray, maxDistance, damageableLayers);

        foreach (RaycastHit i in hit)
        {
            ApplyDamage(i.collider);
        }

        //if (Physics.RaycastAll(ray, out hit, maxDistance, damageableLayers))
        //{
        //    print("Le dio el rayo a: " + hit.collider.gameObject.name);
        //    //endPosition = hit.point;
        //    ApplyDamage(hit.collider);
        //}
        ////else
        ////{
        ////    endPosition = rayOrigin.position + rayOrigin.forward * maxDistance;
        ////}

        //lineRenderer.SetPosition(1, endPosition);
        //lineRenderer.SetPosition(1, rayOrigin.position * maxDistance);


        //StartCoroutine(PlayRayVFX(endPosition));
    }

    IEnumerator PlayRayVFX(Vector3 endPosition)
    {
        float vfxProgress = 0f;
        float totalDistance = Vector3.Distance(rayOrigin.position, endPosition);



        rayVFX.transform.position = rayOrigin.position;
        rayVFX.Play();

        while (vfxProgress < 1f)
        {
            vfxProgress += Time.deltaTime * vfxSpeed / totalDistance;
            Vector3 currentPosition = Vector3.Lerp(rayOrigin.position, endPosition, vfxProgress);
            rayVFX.transform.position = currentPosition;

            yield return null;
        }
        rayVFX.Stop();
    }

    void ApplyDamage(Collider target)
    {
        if (target.gameObject.layer == 7 || target.gameObject.layer == 8)
        {
            PlayerController player = target.GetComponent<PlayerController>();

            EnemyAI_Flying eF = target.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = target.GetComponent<EnemyAI_Meele>();

            if (player != null)
            {
                player.Vida -= damage;
            }

            if (eF != null)
            {
                eF.VidaEnemigo -= damage;
            }
            else if (eM != null)
            {
                eM.VidaEnemigo -= damage;
            }

        }
    }
}
