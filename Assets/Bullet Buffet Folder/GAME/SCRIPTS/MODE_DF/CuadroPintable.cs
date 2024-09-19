using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CuadroPintable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material material;
    private Color colorInicial;
    public int equipoActual = 0;
    public bool pintado = false;

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
            pintado = false;
            equipoActual = 0;
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
            PlayerController player = other.GetComponent<PlayerController>();

            //Si el que piso el cuadro es diferente del equipo del que ya estaba pintado ...
            if(player.equipo != equipoActual)
            {
                //Cambiar el equipo
                equipoActual = player.equipo;

                //Lo pintamos
                meshRenderer.material.color = player.equipo == 1 ? Color.red : Color.blue;

                //Enviamos los cambios al GameManager
                GameManager.CuadradoCambiado(this);

                //Ahora decimos que ya esta pintado
                pintado = true;
            }
        }
    }

}
