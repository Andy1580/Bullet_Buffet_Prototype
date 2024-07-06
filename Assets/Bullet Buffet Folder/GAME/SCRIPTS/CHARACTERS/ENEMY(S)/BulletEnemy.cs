using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float velocidadBala = 10;

    private void Update()
    {
        transform.position += transform.forward * (velocidadBala * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pC = other.GetComponent<PlayerController>();
            if (pC != null && !pC.isInvulnerable)
            {
                pC.Vida -= damage;
                Debug.Log("Le hize daño a: " + pC.gameObject.name);
            }
        }
    }
}

