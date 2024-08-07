using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private GameObject vfx;


    private void FixedUpdate()
    {
        transform.Rotate(0, 25f * Time.fixedDeltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null && player.hability == null)
            {
                player.SetHability(gameObject.name);
                Instantiate(vfx,this.transform);
                Destroy(this.gameObject, 0.25f);
            }
            else
            {
                Instantiate(vfx, this.transform);
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}