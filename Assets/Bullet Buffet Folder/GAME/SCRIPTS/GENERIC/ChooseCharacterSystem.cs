using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterSystem : MonoBehaviour
{
    [SerializeField] private GameManager gM;

    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;

    private void Awake()
    {
        gM = gameObject.GetComponent<GameManager>();
    }

    public void Character1()
    {
        gM._playerOne = character1.transform;
    }

    public void Character2()
    {
        gM._playerTwo = character2.transform;
    }
}
