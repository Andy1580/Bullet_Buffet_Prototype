using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Rigidbody body;
    public float velocidad;

    private void Start()
    {
        //body = GetComponent<Rigidbody>();
        //body.AddForce(transform.forward * velocidad);
    }
}
