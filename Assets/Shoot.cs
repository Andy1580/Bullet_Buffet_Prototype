using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("Handgun stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private Transform bulletprefab;
    [SerializeField] private Transform bulletSpawn;

    private float cont = 0;
    
    private void Update()
    {
        cont -= Time.deltaTime;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        BulletShoot();
    }

    void BulletShoot()
    {
        if (cont <= 0)
        {
            Transform clon = Instantiate(bulletprefab, bulletSpawn.position, bulletSpawn.rotation);
            clon.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            Destroy(clon.gameObject, 3);
            cont = cooldown;
        }
    }
}
