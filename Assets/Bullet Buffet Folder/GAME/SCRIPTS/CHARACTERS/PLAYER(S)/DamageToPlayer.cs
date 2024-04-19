using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    private GameObject player;

    CharacterDisplay chD;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        chD = GetComponent<CharacterDisplay>();
        //characterData = player.GetComponent<CharacterData>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(player.tag == "Player")
    //    {
    //        chD.healthPlayer -= 50;
    //    }
    //}
}
