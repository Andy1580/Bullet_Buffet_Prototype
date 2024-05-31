using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{

    [SerializeField] private bool gMEncontrado = false;
    [SerializeField] private bool p1Listo = false;
    [SerializeField] private bool p2Listo = false;
    [SerializeField] private bool p3Listo = false;
    [SerializeField] private bool p4Listo = false;
    GameManager gM;

    [SerializeField] private GameObject personaje1;
    [SerializeField] private GameObject personaje2;
    [SerializeField] private GameObject personaje3;
    [SerializeField] private GameObject personaje4;

    private void Start()
    {
        if (gM == null && !gMEncontrado)
        {
            gM = FindFirstObjectByType<GameManager>();
        }
        else if (gM != null && !gMEncontrado)
        {
            gMEncontrado = true;
        }
    }

    private void Update()
    {
        if (!gMEncontrado)
        {
            gM = FindFirstObjectByType<GameManager>();
        }

        if(gM.oneVone && gM.modoHechizos && p1Listo && p2Listo)
        {
            Invoke("CargarModoHechizos", 0.25f);
        }
        else if(gM.oneVone && gM.modoHechizos && p1Listo && p2Listo && p3Listo && p4Listo)
        {
            Invoke("CargarModoHechizos", 0.25f);
        }
    }

    public void Personaje1()
    {
        gM.prefabPlayer1 = personaje1;
        p1Listo = true;
    }

    public void Personaje2()
    {
        gM.prefabPlayer2 = personaje2;
        p2Listo = true;
    }

    public void Personaje3()
    {
        gM.prefabPlayer1 = personaje1;
        p1Listo = true;
    }

    public void Personaje4()
    {
        gM.prefabPlayer4 = personaje4;
        p4Listo = true;
    }

    private void CargarModoHechizos()
    {
        Debug.Log("Si se invoco el metodo para cargra escena");
        SceneManager.LoadScene("ANDYCLASH");
    }
}
