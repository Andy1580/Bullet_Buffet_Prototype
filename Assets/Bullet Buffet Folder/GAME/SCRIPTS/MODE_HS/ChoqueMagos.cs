using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoqueMagos : MonoBehaviour
{
    public float Masa = 1.0f;
    public int Fuerza = 10;
    public int FuerzaEnemy = 10;
    public bool Gana = false;
    public bool Pierde = false;
    public bool Empate = false;
    public bool Normal = true;
    public GameObject Enemy;
    public List<GameObject> Powers;
    public GameObject Mago1;
    public GameObject Mago2;
    public GameObject Destruido;

    private void Start()
    {
        GameObject[] more = GameObject.FindGameObjectsWithTag("murito");
        if (more != null)
        {
            foreach(GameObject go in more)
            {
                Powers.Add(go);
            }
        }
    }

    void update()
    {
        if (Fuerza == FuerzaEnemy)
        {
            Gana = false;
            Empate = true;
            Normal = false;
            Pierde = false;
        }

        if (Fuerza < FuerzaEnemy)
        {
            Gana = false;
            Empate = false;
            Normal = false;
            Pierde = true;
        }

        if (Fuerza > FuerzaEnemy)
        {
            Gana = true;
            Empate = false;
            Normal = false;
            Pierde = false;
        }

        if(Normal == true && Gana == false && Pierde == false && Empate == false)
        {
            Masa = -1 * Time.deltaTime;
            transform.Translate(0, 0, Masa);
        }

        else if (Normal == false && Gana == true && Pierde == false && Empate == false)
        {
            Masa = -1 * Time.deltaTime;
            transform.Translate(0, 0, Masa);
        }

        else if (Normal == false && Gana == false && Pierde == true && Empate == false)
        {
            Masa = 1 * Time.deltaTime;
            transform.Translate(0, 0, Masa);
        }

        else if (Normal == false && Gana == true && Pierde == false && Empate == true)
        {

        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        foreach(ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        if (collisionInfo.gameObject.name == "CubeEnd")
        {
            Debug.Log("collision con el CubeEnd");
            foreach(GameObject target in Powers)
            {
                if (target != null)
                {
                    ChoqueMagos2 eh = (ChoqueMagos2)target.GetComponent("ChoqueMagos2");
                    if (eh != null)
                    {
                        eh.AddjustCurrent(Fuerza);
                    }
                }


            }
        }

        if(collisionInfo.gameObject.name == "CubeStart02")
        {
            Debug.Log("collisoncon el CubeStar02");
            Destroy(Mago1);
            Destroy(Mago2);

            for (int i = 0; i < 1; i++)
            {
                GameObject clone = (GameObject)Instantiate(Destruido, Enemy.transform.position, Enemy.transform.rotation);
            }
        }
    }

    public void AddjustCurrent(int adji)
    {
        FuerzaEnemy = adji;
    }


}
