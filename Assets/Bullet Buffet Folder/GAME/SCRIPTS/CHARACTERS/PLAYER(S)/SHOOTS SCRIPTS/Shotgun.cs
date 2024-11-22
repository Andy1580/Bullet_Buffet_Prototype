using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform bocaArma;
    public GameObject objetoDaño;

    public int numVFX = 6;
    public float dispersion = 0.8f;
    public float velocidadVfx = 20f;
    public float vidaVfx = 1f;

    public bool cantShoot;
    private PlayerController propietario;

    private void Awake()
    {
        propietario = transform.parent.GetComponent<PlayerController>();
    }

    private void Start()
    {
        cantShoot = true;
        objetoDaño.SetActive(false);
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot && !propietario.muerto)
        {
            Fire();
            cantShoot = false;
        }
    }

    void Fire()
    {
        objetoDaño.SetActive(true);
        StartCoroutine(DesactivarObjetoDeDaño());

        FireVFX();
    }

    IEnumerator DesactivarObjetoDeDaño()
    {
        yield return new WaitForSeconds(0.5f);

        objetoDaño.SetActive(false);

        cantShoot = true;
    }

    void FireVFX()
    {
        for (int i = 0; i < numVFX; i++)
        {
            Vector3 direccionDeDispersion = ObtenerDispersion(bocaArma.forward, dispersion);

            GameObject vfx = Instantiate(vfxPrefab, bocaArma.position, Quaternion.identity);
            Rigidbody vfxRb = vfx.GetComponent<Rigidbody>();
            if (vfxRb != null)
            {
                vfxRb.velocity = direccionDeDispersion * velocidadVfx;
            }

            Destroy(vfx, vidaVfx);
        }
    }

    private Vector3 ObtenerDispersion(Vector3 originalDirection, float spread)
    {
        return originalDirection + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)).normalized;
    }
}
