using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxhealth = 100;
    public float currenthealth;

    private void Start()
    {
        currenthealth = maxhealth;
    }

    private void Update()
    {
        if (currenthealth <= 0)
        {
            Debug.Log("Murio el Jugador");
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);    
        }
    }

    public void ReduccionVidaPlayer()
    {
        currenthealth -= 100;
    }
}
