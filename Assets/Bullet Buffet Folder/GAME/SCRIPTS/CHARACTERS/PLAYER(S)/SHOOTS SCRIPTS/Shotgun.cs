using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform gunMuzzle;
    public GameObject damageObject;

    public int numVFX = 6;
    public float spread = 0.8f;
    public float vfxSpeed = 20f;
    public float vfxLifetime = 1f;

    public bool cantShoot;

    private void Start()
    {
        cantShoot = true;
        damageObject.SetActive(false);
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot)
        {
            Fire();
            cantShoot = false;
        }
    }

    void Fire()
    {
        // Activar el objeto de daño
        damageObject.SetActive(true);
        StartCoroutine(DesactivarObjetoDeDaño());

        // Instanciar los VFX con dispersión
        FireVFX();
    }

    IEnumerator DesactivarObjetoDeDaño()
    {
        // Esperar 1 segundo antes de desactivar
        yield return new WaitForSeconds(1f);

        // Desactivar el objeto
        damageObject.SetActive(false);

        cantShoot = true;
    }

    void FireVFX()
    {
        for (int i = 0; i < numVFX; i++)
        {
            Vector3 spreadDirection = GetSpreadDirection(gunMuzzle.forward, spread);

            GameObject vfx = Instantiate(vfxPrefab, gunMuzzle.position, Quaternion.identity);
            Rigidbody vfxRb = vfx.GetComponent<Rigidbody>();
            if (vfxRb != null)
            {
                vfxRb.velocity = spreadDirection * vfxSpeed;
            }

            Destroy(vfx, vfxLifetime);
        }
    }

    private Vector3 GetSpreadDirection(Vector3 originalDirection, float spread)
    {
        return originalDirection + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)).normalized;
    }
}
