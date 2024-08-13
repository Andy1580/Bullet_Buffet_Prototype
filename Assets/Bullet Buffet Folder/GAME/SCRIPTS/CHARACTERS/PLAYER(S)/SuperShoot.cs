using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShoot : MonoBehaviour
{
    [SerializeField] private float vidaBala = 3;
    [SerializeField] private int daño;
    public float spreadSpeed = 300f;

    [SerializeField] private float velocidadBala = 300f;

    private void Start()
    {
        Destroy(this.gameObject, vidaBala);
        StartCoroutine(SpreadBullet());
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * (velocidadBala * Time.deltaTime);
    }

    private IEnumerator SpreadBullet()
    {
        while (true)
        {
            Vector3 direction = transform.forward;
            transform.position += direction * spreadSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Vida = daño;
                Destroy(this.gameObject);
            }

        }

        else if (other.CompareTag("Enemy"))
        {
            EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();

            if (eM != null)
            {
                eM.VidaEnemigo = daño;
                Destroy(this.gameObject);
            }

            if(eF != null)
            {
                eF.VidaEnemigo = daño;
                Destroy(this.gameObject);
            }
        }

    }
}
