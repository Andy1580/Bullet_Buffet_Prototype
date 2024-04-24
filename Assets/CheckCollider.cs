using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mago2")
        {
            Debug.LogWarning("si es mago 2");
            
        }
        
        else if(other.tag == "Mago1")
        {
            Debug.LogWarning("si es mago 1");
        }
    }
}
