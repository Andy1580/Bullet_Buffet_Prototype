using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSystemKaiju : MonoBehaviour
{
    public GameObject panelWin1;
    public GameObject panelWin2;
    
    public GameObject panelWinOptions;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Mago2")
        {
            Debug.LogWarning("si col con mago2");
            panelWin1.SetActive(true);
            panelWinOptions.SetActive(true);
            Time.timeScale = 0f;
        }

        if(collision.collider.gameObject.tag == "Mago1")
        {
            panelWin2.SetActive(true);
            panelWinOptions.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        panelWin1.SetActive(false);
        panelWin2.SetActive(false);
        panelWinOptions.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene("ANDYCLASH");
    }

    public void GoToMenu()
    {
        panelWin1.SetActive(false);
        panelWin2.SetActive(false);
        panelWinOptions.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene("ANDYMENUTEST");
    }
}
