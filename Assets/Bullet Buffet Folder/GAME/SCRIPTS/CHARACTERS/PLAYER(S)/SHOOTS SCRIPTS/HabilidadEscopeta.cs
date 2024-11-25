using System.Collections;
using UnityEngine;

public class HabilidadEscopeta : MonoBehaviour
{
    //[SerializeField] private GameObject vfxPrefab;
    //[SerializeField] private Transform bocaDeArma;
    //[SerializeField] private int numVFX = 6;
    //[SerializeField] private float dispersion = 0.8f;
    //[SerializeField] private float velocidadVfx = 20f;
    //[SerializeField] private float vidaVfx = 1f;

    [SerializeField] private GameObject objetoDaño;
    [SerializeField] private ParticleSystem vfxSuperShootShotgun;

    public bool cantShoot;

    private PlayerController propietario;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void Start()
    {
        objetoDaño.SetActive(false);
    }

    public void ActivarHabilidad()
    {
        if(cantShoot && !propietario.muerto)
        {
            Fire();
        }
    }

    void Fire()
    {
        cantShoot = false;
        objetoDaño.SetActive(true);
        vfxSuperShootShotgun.Play();
        StartCoroutine(DesactivarObjetoDeDaño());
        //InstanciarVFX();
    }

    IEnumerator DesactivarObjetoDeDaño()
    {
        yield return new WaitForSeconds(1f);

        objetoDaño.SetActive(false);

        cantShoot = true;

        vfxSuperShootShotgun.Stop();
    }
    /*
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
    */
}
