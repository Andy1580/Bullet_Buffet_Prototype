using System.Collections;
using UnityEngine;

public class HabilidadEscopeta : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform bocaDeArma;
    public GameObject objetoDa�o;
    public int numVFX = 6;
    public float dispersion = 0.8f;
    public float velocidadVfx = 20f;
    public float vidaVfx = 1f;

    public bool cantShoot;

    private void Start()
    {
        cantShoot = true;
        objetoDa�o.SetActive(false);
    }

    public void ActivarHabilidad()
    {
        if(cantShoot)
        {
            Fire();
        }
    }

    void Fire()
    {
        cantShoot = false;
        objetoDa�o.SetActive(true);
        StartCoroutine(DesactivarObjetoDeDa�o());
        InstanciarVFX();
    }

    IEnumerator DesactivarObjetoDeDa�o()
    {
        yield return new WaitForSeconds(1f);

        objetoDa�o.SetActive(false);

        cantShoot = true;
    }

    void InstanciarVFX()
    {
        for (int i = 0; i < numVFX; i++)
        {
            Vector3 direccionDeDispersion = ObtenerDispersion(bocaDeArma.forward, dispersion);

            GameObject vfx = Instantiate(vfxPrefab, bocaDeArma.position, Quaternion.identity);
            Rigidbody vfxRb = vfx.GetComponent<Rigidbody>();

            vfxRb.velocity = direccionDeDispersion * velocidadVfx;

            Destroy(vfx, vidaVfx);
        }
    }

    private Vector3 ObtenerDispersion(Vector3 originalDirection, float spread)
    {
        return originalDirection + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)).normalized;
    }
}
