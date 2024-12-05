using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject objetoDaño;

    [SerializeField] private ParticleSystem vfxShotun;

    public bool cantShoot;
    private PlayerController propietario;
    
    //[SerializeField] private Transform bocaArma;
    //[SerializeField] private float vidaVfx = 1f;
    //[SerializeField] private float dispersion = 0.8f;
    //[SerializeField] private int numVFX = 6;
    //[SerializeField] private GameObject vfxPrefab;
    //[SerializeField] private float velocidadVfx = 20f;

    private void Awake()
    {
        propietario = GetComponent<PlayerController>();
    }

    private void Start()
    {
        cantShoot = true;
        objetoDaño.SetActive(false);
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot && !propietario.muerto && !propietario.BloquearMovimiento && !propietario.escudo.gameObject.activeSelf)
        {
            Fire();
            cantShoot = false;
        }
    }

    void Fire()
    {
        objetoDaño.SetActive(true);
        vfxShotun.Play();

        AudioManager.instance.PlaySound("disparoKAI");

        StartCoroutine(DesactivarObjetoDeDaño());

        //FireVFX();
    }

    IEnumerator DesactivarObjetoDeDaño()
    {
        yield return new WaitForSeconds(1f);

        objetoDaño.SetActive(false);

        cantShoot = true;
    }
    /*
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
    */
}
