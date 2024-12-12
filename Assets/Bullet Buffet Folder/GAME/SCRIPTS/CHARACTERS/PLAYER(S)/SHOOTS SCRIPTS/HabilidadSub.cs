using System.Collections;
using UnityEngine;

public class HabilidadSub : MonoBehaviour
{
    private PlayerController propietario;
    [SerializeField] private float areaDaño = 5f;
    [SerializeField] private int daño = 10;
    [SerializeField] private float duracionHabilidad = 3f;
    [SerializeField] private float intervaloDaño = 0.5f;
    [SerializeField] private float velicidadEnHabilidad = 5.5f;
    [SerializeField] private LayerMask capas;
    [SerializeField] private ParticleSystem vfxHabilidadSub;
    [SerializeField] private GameObject vfxImpacto;

    private bool muertoJugador = false;

    private void Awake()
    {
        propietario = GetComponent<PlayerController>();
    }

    public void ActivarHabilidad()
    {
        if(!propietario.muerto && !propietario.escudo.gameObject.activeSelf)
        {
            propietario.animator.SetTrigger("habilidad");
            propietario.playerSpeed = velicidadEnHabilidad;
            StartCoroutine(DañoConstanteEnArea());
        }
    }

    private void FixedUpdate()
    {
        if(propietario.muerto && !muertoJugador)
        {
            StopAllCoroutines();
            vfxHabilidadSub.Stop();
            AudioManager.instance.StopSound("habilidadNOVA");
            muertoJugador = true;
            Invoke("DesactivarMuerte", 5f);
            return;
        }
    }

    void DesactivarMuerte()
    {
        muertoJugador = false;
    }

    IEnumerator DañoConstanteEnArea()
    {
        vfxHabilidadSub.Play();

        float tiempoRestante = duracionHabilidad;

        while (tiempoRestante > 0f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, areaDaño, capas);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == propietario.gameObject)
                {
                    continue;  // Saltar al siguiente collider
                }

                if (collider.gameObject.layer == 8)  // layer 8 = Players
                {
                    PlayerController player = collider.gameObject.GetComponent<PlayerController>();

                    if (player.equipo == propietario.equipo) continue;

                    if (player != null && !player.muerto && player.equipo != propietario.equipo)
                    {
                        if (!player.isInvulnerable)
                        {
                            player.Vida -= daño;
                            Vector3 puntoImpacto = collider.ClosestPoint(transform.position);
                            Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
                        }
                    }
                }

                if (collider.gameObject.layer == 7) // layer 7 = Enemy
                {
                    EnemyAI_Flying eF = collider.gameObject.GetComponent<EnemyAI_Flying>();
                    EnemyAI_Meele eM = collider.gameObject.GetComponent<EnemyAI_Meele>();

                    if (eM != null)
                    {
                        eM.VidaEnemigo -= daño;
                    }

                    if (eF != null)
                    {
                        eF.VidaEnemigo -= daño;
                    }
                }
            }

            yield return new WaitForSeconds(intervaloDaño);

            tiempoRestante -= intervaloDaño;  // Reducir el tiempo restante de la habilidad
        }
        propietario.playerSpeed = propietario.actualSpeed;
        propietario.animator.SetTrigger("mov");
        propietario.habilidadActiva = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaDaño);
    }
}
