using System.Collections;
using TMPro;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    //public BoxCollider colliderZone;

    //public float timeToClose;

    public float velocidadReduccion = 0.5f; // Velocidad de reducción del collider
    private BoxCollider boxCollider; // Referencia al BoxCollider

    public float tiempoInicial = 60f; // Tiempo inicial en segundos
    private float tiempoRestante; // Tiempo restante en segundos
    public TextMeshProUGUI textoTiempo; // Referencia al objeto Text donde se mostrará el tiempo

    public bool playerInZone;

    private PlayerHealth pH;

    private GameObject playerRef;


    private void Start()
    {
        //colliderZone = GetComponent<BoxCollider>();
        //colliderZone.size = new Vector3(colliderZone.size.x, colliderZone.size.y, colliderZone.size.z);

        // Obtener la referencia al BoxCollider
        boxCollider = GetComponent<BoxCollider>();
        // Inicializar el tiempo restante
        tiempoRestante = tiempoInicial;

        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");

            if (playerRef != null)
            {

                pH = playerRef.GetComponent<PlayerHealth>();

                if (pH != null)
                {
                    Debug.LogWarning("Se encontro el componente en dicho objeto");
                }
                else
                {
                    Debug.LogWarning("No se encontro el componente en dicho objeto");
                }

            }
        }
        else
        {
            Debug.LogWarning("No se encontro dicho objeto");
        }

    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        // Restar el tiempo
        tiempoRestante -= Time.deltaTime;

        // Actualizar el texto del tiempo
        ActualizarTextoTiempo();

        // Verificar si el tiempo ha llegado a cero
        if (tiempoRestante <= 0f)
        {
            // Aquí puedes realizar acciones adicionales cuando el tiempo se agote, como detener el juego o mostrar un mensaje de fin de juego.
            Debug.Log("Tiempo agotado");
            // Puedes detener el tiempo en lugar de dejar que sea negativo
            tiempoRestante = 0f;
            StartCoroutine(CorutinaDeTiempo());
        }

    }


    void ActualizarTextoTiempo()
    {
        // Convertir el tiempo restante a minutos y segundos
        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        // Formatear el tiempo como cadena de texto (mm:ss)
        string tiempoTexto = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Actualizar el texto en el objeto Text
        textoTiempo.text = "Tiempo: " + tiempoTexto;
    }

    void ReducirZonaSegura()
    {
        // Reducir gradualmente el tamaño del BoxCollider en función del tiempo transcurrido
        float reduccion = velocidadReduccion * Time.deltaTime;
        Vector3 nuevaEscala = boxCollider.size - new Vector3(reduccion, reduccion, reduccion);
        boxCollider.size = nuevaEscala;

        // Detener la reducción si el collider ha alcanzado un tamaño mínimo
        if (boxCollider.size.x <= 15 || boxCollider.size.y <= 15 || boxCollider.size.z <= 15)
        {
            // Detener la reducción estableciendo la escala a cero o cualquier otro valor mínimo deseado
            boxCollider.size = Vector3.Normalize(nuevaEscala);
        }

        //Debug.Log("Current BoxCollider Size : " + boxCollider.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInZone = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;

        if (other.tag == "Player" && !playerInZone)
        {
            pH.ReduccionVidaPlayer();
        }
    }

    IEnumerator CorutinaDeTiempo()
    {
        ReducirZonaSegura();
        yield return new WaitForSeconds(8f);
        StopCoroutine(CorutinaDeTiempo());
        tiempoRestante = 8f;
        yield return new WaitForSeconds(1f);
    }

    //Make sure there is a BoxCollider component attached to your GameObject
    //BoxCollider m_Collider;
    //float m_ScaleX, m_ScaleY, m_ScaleZ;


    //void Start()
    //{
    //    //Fetch the Collider from the GameObject
    //    m_Collider = GetComponent<BoxCollider>();
    //    //These are the starting sizes for the Collider component
    //    m_ScaleX = 36.57625f;
    //    m_ScaleY = 9.471907f;
    //    m_ScaleZ = 35.58574f;

    //}

    //void Update()
    //{
    //    m_ScaleX = m_Collider.size.x;
    //    m_ScaleY = m_Collider.size.y;
    //    m_ScaleZ = m_Collider.size.z;

    //    m_Collider.size = new Vector3(m_ScaleX, m_ScaleY, m_ScaleZ);
    //    Debug.Log("Current BoxCollider Size : " + m_Collider.size);
    //}



    //void Update()
    //{
    //    // Reducir gradualmente el tamaño del BoxCollider en función del tiempo transcurrido
    //    float reduccion = velocidadReduccion * Time.deltaTime;
    //    Vector3 nuevaEscala = boxCollider.size - new Vector3(reduccion, reduccion, reduccion);
    //    boxCollider.size = nuevaEscala;

    //    // Detener la reducción si el collider ha alcanzado un tamaño mínimo
    //    if (boxCollider.size.x <= 15 || boxCollider.size.y <= 15 || boxCollider.size.z <= 15)
    //    {
    //        // Detener la reducción estableciendo la escala a cero o cualquier otro valor mínimo deseado
    //        boxCollider.size = Vector3.Normalize(nuevaEscala);
    //    }

    //    Debug.Log("Current BoxCollider Size : " + boxCollider.size);
    //}
}
