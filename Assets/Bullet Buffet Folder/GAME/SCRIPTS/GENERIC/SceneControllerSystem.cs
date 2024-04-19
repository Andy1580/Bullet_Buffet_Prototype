using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerSystem : MonoBehaviour
{
    [SerializeField] private GameObject panelVictoria;

    public void GoToMenu()
    {
        Debug.Log("Click menu");
        panelVictoria.SetActive(false);
        SceneManager.LoadScene("ANDYMENUTEST");
    }

    public void RestartGame()
    {
        Debug.Log("Click restart");
        panelVictoria.SetActive(false);
        SceneManager.LoadScene("ANDY");
    }
}
