using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelMapas;
    [SerializeField] private GameObject panelSetings;
    [SerializeField] private GameObject bModes;
    [SerializeField] private GameObject bHechizos;


    [SerializeField] private GameObject eventSystem;
    //private EventSystem eventS;

    GameManager gM;

    private void Start()
    {
        if (gM == null)
        {
            gM = FindFirstObjectByType<GameManager>();
        }



        //if (eventSystem == null)
        //{
        //    Debug.LogWarning("Nose encontro el Event System");
        //    eventSystem = GameObject.Find("EventSystem");
        //}
        //else
        //{
        //    Debug.LogWarning("Se encontro el Event System");
        //}

        //eventS = eventSystem.GetComponent<EventSystem>();

        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelMapas.SetActive(false);


        //eventS.firstSelectedGameObject = bModes;
    }

    private void Update()
    {
        Update_CheckBooleans();
    }

    private void Update_CheckBooleans()
    {
        if (SceneManager.GetActiveScene().name != "ANDYMENUTEST")
            return;

        if (GameManager.modoHS && GameManager.boolMapaStreet)
        {
            Invoke("CargarJuego", 0.25f);
        }
        else if (GameManager.modoDS && GameManager.boolMapaStreet)
        {
            Invoke("CargarJuego", 0.25f);
        }
    }

    public void GoToModos()
    {
        panelModos.SetActive(true);
        panelSetings.SetActive(false);
        panelInicio.SetActive(false);

        //eventS.firstSelectedGameObject = bHechizos;
    }

    public void Back()
    {
        panelInicio.SetActive(true);
        panelModos.SetActive(false);
        panelSetings.SetActive(false);
        panelMapas.SetActive(false);

        //eventS.firstSelectedGameObject = bModes;
    }

    public void GoToSettings()
    {
        panelSetings.SetActive(true);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void ModoHechizosSazonados()
    {
        GameManager.modoHS = true;
        panelMapas.SetActive(true);
        panelSetings.SetActive(false);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void ModoDueloDeSalsas()
    {
        GameManager.modoDS = true;
        panelMapas.SetActive(true);
        panelSetings.SetActive(false);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void MapaStreet()
    {
        GameManager.boolMapaStreet = true;
    }

    private void CargarJuego()
    {
        SceneManager.LoadScene("ANDYINGAME");
    }
}
