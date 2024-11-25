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
    [SerializeField] private float duracionHabilidad = 3f;
    [SerializeField] private ParticleSystem vfxRayo;
    //[SerializeField] private ParticleSystem rayVFX;
    //[SerializeField] private float velocidadVFX = 5f;

    private bool habilidadActiva = false;

    private PlayerController propietario;

    //private void Awake()
    //{
    //    propietario = transform.parent.GetComponent<PlayerController>();
    //}

    public void ActivarHabilidad(PlayerController player)
    {
        propietario = player;

        if (!habilidadActiva)
        {
            //StartCoroutine(DispararRayContinuamente());
            StartCoroutine(ActivarRayo());
        }
    }

    void Start()
    {
        lineRenderer = origenRayCast.GetComponentInChildren<LineRenderer>();
        //vfxRayo = GetComponent<ParticleSystem>();
        lineRenderer.enabled = false;
        //lineRenderer.positionCount = 2; // puntos (origen y final)
        //lineRenderer.SetPosition(0, origenRayCast.position);
        //lineRenderer.SetPosition(1, origenRayCast.position);
    }

    void FireRay()
    {
        propietario.animator.SetTrigger("habilidad");
        habilidadActiva = true;

        Ray ray = new Ray(origenRayCast.position, origenRayCast.forward);
        RaycastHit[] hit = Physics.RaycastAll(ray, distanciaMaxima, capas);

        if(habilidadActiva)
        {
            foreach (RaycastHit i in hit)
            {
                AplicarDaño(i.collider);
            }
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
        vfxRayo.Stop();
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

    IEnumerator ActivarRayo()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(1f);
        FireRay();
        vfxRayo.Play();
        yield return new WaitForSeconds(0.7f);
        habilidadActiva = false;
        yield return new WaitForSeconds(1f);
        lineRenderer.enabled = false;
        vfxRayo.Stop();
    }

    void AplicarDaño(Collider target)
    {
        if (target.gameObject.layer == 7 || target.gameObject.layer == 8)
        {
            PlayerController player = target.GetComponent<PlayerController>();

            EnemyAI_Flying eF = target.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = target.GetComponent<EnemyAI_Meele>();

            if (player != null)
            {
                if (player.equipo == propietario.equipo)
                {

                }
                else
                {
                    if (!player.muerto && !player.isInvulnerable)
                    {
                        player.Vida -= daño;
                    }
                    else return;
                }

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
