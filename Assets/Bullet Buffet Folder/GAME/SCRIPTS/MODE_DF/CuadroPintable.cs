using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CuadroPintable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material material;
    private Color colorInicial;
    public string currentOwner;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        meshRenderer = GetComponent<MeshRenderer>();
        
        material = GetComponent<MeshRenderer>().material;

        colorInicial = material.color;
        

    }
    private void Start()
    {
        //currentOwner = "None"; // Inicialmente, el cuadro no pertenece a nadie
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
            currentOwner = "None";
            material.color = colorInicial;
        }
    }
    //En esta parte verificamos quin entro al trigger mediante el tag,
    //verificamos cual es el nombre del objeto para determinar el paso a seguir
    //y checamos si ya esta asignado a algun lista, sino esta lo asignamos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            //Obtebemos su nombre y veificamos que no se haya asignado al "curentOwner"
            string playerName = other.gameObject.name;
            if (playerName == "J1(Clone)" && currentOwner != "J1(Clone)")
            {
                if (currentOwner == "J2(Clone)")
                {
                    GameManager.Instance.RemoverCuadroPintado(this, "J2(Clone)");
                }
                CambiarPropietario(playerName);
            }
            else if (playerName == "J2(Clone)" && currentOwner != "J2(Clone)")
            {
                if (currentOwner == "J1(Clone)")
                {
                    GameManager.Instance.RemoverCuadroPintado(this, "J1(Clone)");
                }
                CambiarPropietario(playerName);
            }
        }
    }

    //Una vez verificado lo anterior pasamos a pintar el cuadro usando el meshRenderer para cambiarle el color de la textur
    //asignada al color especifico de cada jugador y posterior a eso registrar el cuadro correspondiente al jugador
    private void CambiarPropietario(string newOwner)
    {
        currentOwner = newOwner;
        if (newOwner == "J1(Clone)")
        {
            Debug.Log("Si se pinto cuadro de J1");
            meshRenderer.material.color = Color.blue;
        }
        else if (newOwner == "J2(Clone)")
        {
            Debug.Log("Si se pinto cuadro de J2");
            meshRenderer.material.color = Color.red;
        }
        else
        {
            Debug.LogError("MeshRender, no se encontro en " + gameObject.name);
        }

        GameManager.Instance.RegistrarCuadroPintado(this);
    }
}
