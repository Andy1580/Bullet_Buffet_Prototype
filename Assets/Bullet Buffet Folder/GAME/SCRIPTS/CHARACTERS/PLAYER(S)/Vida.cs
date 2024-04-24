using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    //public int Daño;

    public int salud;
    public Slider BarraSalud;
    public bool isInvulnerable = false;

    

    private void Update()
    {
        BarraSalud.GetComponent<Slider>().value = salud;

        if (salud <= 0)
        {
            Debug.Log("Muerto" + this.gameObject.name);
            //Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }
        
    }

}
