using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CuadroPintable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material material;
    private Color colorInicial;
    public int currentTeam;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        meshRenderer = GetComponent<MeshRenderer>();
        
        material = GetComponent<MeshRenderer>().material;

        colorInicial = material.color;
        

    }
    private void Start()
    {
        //currentTeam = "None"; // Inicialmente, el cuadro no pertenece a nadie
        //material.color = colorInicial;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetearCuadro();
    }

    void ResetearCuadro()
    {
        if(SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            currentTeam = 0;
            material.color = colorInicial;
        }
    }
    //En esta parte verificamos quin entro al trigger mediante el tag,
    //verificamos cual es el nombre del objeto para determinar el paso a seguir
    //y checamos si ya esta asignado a algun lista, sino esta lo asignamos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Obtebemos su nombre y veificamos que no se haya asignado al "curentOwner"
            PlayerController playerTeam = other.GetComponent<PlayerController>();

            if (playerTeam.equipo == 1 && currentTeam != 1)
            {
                if (currentTeam == 2)
                {
                    GameManager.Instance.RemoverCuadroPintado(this, currentTeam);
                }
                CambiarPropietario(playerTeam.equipo);
            }
            else if (playerTeam.equipo == 2 && currentTeam != 2)
            {
                if (currentTeam == 1)
                {
                    GameManager.Instance.RemoverCuadroPintado(this, currentTeam);
                }
                CambiarPropietario(playerTeam.equipo);
            }
        }
    }

    //Una vez verificado lo anterior pasamos a pintar el cuadro usando el meshRenderer para cambiarle el color de la textur
    //asignada al color especifico de cada jugador y posterior a eso registrar el cuadro correspondiente al jugador
    private void CambiarPropietario(int newOwner)
    {
        currentTeam = newOwner;
        if (newOwner == 1)
        {
            meshRenderer.material.color = Color.red;
        }
        else if (newOwner == 2)
        {
            meshRenderer.material.color = Color.blue;
        }
        else
        {
            Debug.LogError("MeshRender, no se encontro en " + gameObject.name);
        }

        GameManager.Instance.RegistrarCuadroPintado(this);
    }
}
