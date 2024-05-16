using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelSetings;

    [SerializeField] private GameObject bModes;
    [SerializeField] private GameObject bHechizos;

    [SerializeField] private GameObject eventSystem;
    private EventSystem eventS;

    private void Start()
    {

        if (eventSystem == null)
        {
            Debug.LogWarning("Nose encontro el Event System");
            eventSystem = GameObject.Find("EventSystem");
        }
        else
        {
            Debug.LogWarning("Se encontro el Event System");
        }

        eventS = eventSystem.GetComponent<EventSystem>();

        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);

        eventS.firstSelectedGameObject = bModes;
    }

    public void GoToModos()
    {
        panelModos.SetActive(true);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);

        eventS.firstSelectedGameObject = bHechizos;
    }

    public void GoToInicio()
    {
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);

        eventS.firstSelectedGameObject = bModes;
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
