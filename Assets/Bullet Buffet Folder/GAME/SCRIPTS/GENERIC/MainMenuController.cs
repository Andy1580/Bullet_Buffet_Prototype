using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelSetings;

    [SerializeField] private GameObject bModes;
    [SerializeField] private GameObject bEnfrentamiento;

    //[SerializeField] private InputSystemUIInputModule inputModule;
    //[SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        //inputModule = GetComponent<InputSystemUIInputModule>();
        //eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        //if (eventSystem != null)
        //{
        //    eventSystem = GetComponent<EventSystem>();
        //}
        //else
        //{
        //    Debug.LogWarning("Nose encontro el Event System");
        //}
    }

    private void Start()
    {
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);

        //eventSystem.firstSelectedGameObject = bModes;
    }

    public void GoToModos()
    {
        panelModos.SetActive(true);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);

        //eventSystem.firstSelectedGameObject = bEnfrentamiento;
    }

    public void GoToInicio()
    {
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);

        //eventSystem.firstSelectedGameObject = bModes;
    }

    public void GoToSetings()
    {
        panelSetings.SetActive(true);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void ModoEnfrentamiento()
    {
        SceneManager.LoadScene("ANDYCLASH");
    }

    public void ModeCaptureFlag()
    {
        SceneManager.LoadScene("ANDYFLAG");
    }


}
