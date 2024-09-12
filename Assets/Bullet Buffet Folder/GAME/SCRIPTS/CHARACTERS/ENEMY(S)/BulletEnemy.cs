using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float velocidadBala = 10;
    [SerializeField] private float vidaBala = 3;

    private void Update()
    {
        transform.position += transform.forward * (velocidadBala * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) //Layer Player = 8
        {
            PlayerController pC = other.GetComponent<PlayerController>();
            if (!pC.isInvulnerable)
            {
                pC.Vida -= damage;
                //Debug.Log("vida restante: " + pC.Vida.ToString());
                Destroy(this.gameObject, vidaBala);
            }
        }
        else if(other.gameObject.layer == 0)
        {
            Destroy(this.gameObject);
        }
    }
}

