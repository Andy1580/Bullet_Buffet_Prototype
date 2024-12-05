using System.Collections;
using UnityEngine;

public class HabilidadEnArea : MonoBehaviour
{
    [SerializeField] private float areaDaño = 3f;
    [SerializeField] private int daño = 50;
    [SerializeField] private GameObject vfxHabilidadArea;
    [SerializeField] private GameObject vfxImpacto;
    private PlayerController propietario;
    public bool propietarioMuerto = false;

    private void Awake()
    {
        propietario = GetComponent<PlayerController>();
    }

    private void Start()
    {
        vfxHabilidadArea.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (propietario.muerto && !propietarioMuerto)
        {
            propietarioMuerto = true;
            propietario.DeshabilitarMovimiento();
            Invoke("ResetearBool", 5f);
            return;
        }
    }

    void ResetearBool()
    {
        propietarioMuerto = false;
    }

    public void ActivarHabilidad()
    {
        ActivarExplosion();
    }

    void ActivarExplosion()
    {
        propietario.animator.SetTrigger("habilidad");

        StartCoroutine(ActivarVfxHabilidad());

        Collider[] colliders = Physics.OverlapSphere(transform.position, areaDaño);

        foreach (Collider collider in colliders)
        {
            print(collider.gameObject);

            if (collider.gameObject == propietario.gameObject)
            {
                continue;
            }

            if (collider.gameObject.layer == 8) //8 jugadores
            {
                PlayerController player = collider.gameObject.GetComponent<PlayerController>();

                if (player.equipo == propietario.equipo)
                {

                }
                else
                {
                    if (player.muerto) return;
                    player.Vida -= daño;
                    Vector3 puntoImpacto = collider.ClosestPoint(transform.position);
                    Instantiate(vfxImpacto, puntoImpacto, Quaternion.identity);
                }

            }

            if (collider.gameObject.layer == 7) //7 enemigos
            {
                EnemyAI_Flying eF = collider.gameObject.GetComponent<EnemyAI_Flying>();
                EnemyAI_Meele eM = collider.gameObject.GetComponent<EnemyAI_Meele>();

                if (eF != null)
                {
                    eF.VidaEnemigo -= daño;
                }

                if (eM != null)
                {
                    eM.VidaEnemigo -= daño;
                }
            }

        }



    }

    IEnumerator ActivarVfxHabilidad()
    {
        vfxHabilidadArea.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        vfxHabilidadArea.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, areaDaño);
    }
}
