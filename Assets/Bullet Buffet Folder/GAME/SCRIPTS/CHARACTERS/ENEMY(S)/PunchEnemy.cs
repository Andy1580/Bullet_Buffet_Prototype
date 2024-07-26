using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchEnemy : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pC = other.GetComponent<PlayerController>();
            if (pC != null && !pC.isInvulnerable)
            {
                pC.Vida -= damage;
                //Debug.Log("vida restante: " + pC.Vida.ToString());
            }
        }
        else if(other.CompareTag("Shield"))
        {
            return;
        }
    }
}
