using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelModos;
    [SerializeField] private GameObject panelVersus;
    [SerializeField] private GameObject panelSetings;
    [SerializeField] private GameObject bModes;
    [SerializeField] private GameObject bHechizos;
    

    [SerializeField] private GameObject eventSystem;
    //private EventSystem eventS;

    GameManager gM;

    private void Start()
    {
        if(gM == null)
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
        panelVersus.SetActive(false);


        //eventS.firstSelectedGameObject = bModes;
    }

    private void Update()
    {
        Update_CheckBooleans();
    }

    private void Update_CheckBooleans()
    {
        if(gM.modoHechizos && gM.oneVone)
        {
            Invoke("CargarModoHechizos", 0.25f);
        }
        else if(gM.modoHechizos && gM.twoVtwo)
        {
            Invoke("CargarModoHechizos", 0.25f);
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

        //eventS.firstSelectedGameObject = bModes;
    }

    public void GoToSettings()
    {
        panelSetings.SetActive(true);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void ModoHechizos()
    {
        gM.modoHechizos = true;
        panelVersus.SetActive(true);
        panelSetings.SetActive(false);
        panelModos.SetActive(false);
        panelInicio.SetActive(false);
    }

    public void OneVOne()
    {
        gM.oneVone = true;
    }

    public void TwoVTwo()
    {
        gM.twoVtwo = true;
    }

    private void CargarModoHechizos()
    {
        Debug.Log("Si se invoco el metodo para cargra escena");
        SceneManager.LoadScene("ANDYCLASH");
    }
}
