using System.Collections;
using UnityEngine;

public class HabilidadRayo : MonoBehaviour
{
    [Header("Rayo Core")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float distanciaMaxima = 100f;
    [SerializeField] private int daño = 50;
    [SerializeField] private Transform origenRayCast; //Boca del arma
    [SerializeField] private LayerMask capas;
    [SerializeField] private ParticleSystem rayVFX;
    [SerializeField] private float duracionHabilidad = 3f;
    //[SerializeField] private float velocidadVFX = 5f;

    private bool habilidadActiva = false;

    public void ActivarHabilidad()
    {
        if (!habilidadActiva)
        {
            StartCoroutine(DispararRayContinuamente());
        }
    }

    void Start()
    {
        lineRenderer = origenRayCast.GetComponentInChildren<LineRenderer>();
        rayVFX = GetComponent<ParticleSystem>();

        lineRenderer.enabled = false;
        //lineRenderer.positionCount = 2; // puntos (origen y final)
        //lineRenderer.SetPosition(0, origenRayCast.position);
        //lineRenderer.SetPosition(1, origenRayCast.position);
    }

    void FireRay()
    {
        
        Ray ray = new Ray(origenRayCast.position, origenRayCast.forward);
        RaycastHit[] hit = Physics.RaycastAll(ray, distanciaMaxima, capas);

        foreach (RaycastHit i in hit)
        {
            AplicarDaño(i.collider);
        }

    }

    IEnumerator DispararRayContinuamente()
    {
        habilidadActiva = true;
        lineRenderer.enabled = true;

        float tiempoRestante = duracionHabilidad;

        while (tiempoRestante > 0f)
        {
            //FireRay();
            Invoke("FireRay", 1.35f);
            tiempoRestante -= Time.deltaTime;

            yield return null;
        }

        lineRenderer.enabled = false;
        habilidadActiva = false;
    }
    /*
    IEnumerator PlayRayVFX(Vector3 endPosition)
    {
        float vfxProgress = 0f;
        float totalDistance = Vector3.Distance(origenRayCast.position, endPosition);



        rayVFX.transform.position = origenRayCast.position;
        rayVFX.Play();

        while (vfxProgress < 1f)
        {
            vfxProgress += Time.deltaTime * velocidadVFX / totalDistance;
            Vector3 currentPosition = Vector3.Lerp(origenRayCast.position, endPosition, vfxProgress);
            rayVFX.transform.position = currentPosition;

            yield return null;
        }
        rayVFX.Stop();
    }
    */
    void AplicarDaño(Collider target)
    {
        if (target.gameObject.layer == 7 || target.gameObject.layer == 8)
        {
            PlayerController player = target.GetComponent<PlayerController>();

            EnemyAI_Flying eF = target.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = target.GetComponent<EnemyAI_Meele>();

            if (player != null)
            {
                if (player.muerto) return;

                player.Vida -= daño;
            }

            if (eF != null)
            {
                eF.VidaEnemigo -= daño;
            }
            else if (eM != null)
            {
                eM.VidaEnemigo -= daño;
            }

        }
    }
}
