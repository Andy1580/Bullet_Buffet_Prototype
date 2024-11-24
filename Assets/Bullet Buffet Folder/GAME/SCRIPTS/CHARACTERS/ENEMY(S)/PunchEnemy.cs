using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchEnemy : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if(!player.isInvulnerable || !player.muerto)
                {
                    player.Vida -= damage;
                }
            }
        }
        else if(other.gameObject.layer == 10)
        {
            return;
        }
    }
}
