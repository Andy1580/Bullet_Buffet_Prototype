using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MsjController : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        Invoke("CargarEscena", 5.20f);
    }

    void CargarEscena()
    {
        SceneManager.LoadScene("ANDYMENUTEST");
    }
}
