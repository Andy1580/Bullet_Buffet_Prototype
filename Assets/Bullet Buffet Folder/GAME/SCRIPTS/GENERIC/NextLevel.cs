using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private GameObject playerReference1;
    [SerializeField] private GameObject playerReference2;
    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject panelVictoria;

    public float tiempoInicial = 4f;
    public float tiempoActual;
    [SerializeField] private float multiplicador;

    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    //[SerializeField] private Transform text1ReferenceObject;
    //[SerializeField] private Transform text2ReferenceObject;

    [SerializeField] private GameObject panelPantallaDeCarga;

    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;

    CapsuleCollider col1;
    PlayerController plaC1;

    CapsuleCollider col2;
    PlayerController plaC2;

    SphereCollider portalCol;

    Vector3 spawnPlayer1;

    [SerializeField] private bool player1InArea = false;
    [SerializeField] private bool player2InArea = false;

    private void Start()
    {
        playerReference1 = GameObject.FindGameObjectWithTag("Player1");
        
        map2.SetActive(false);

        spawnPlayer1 = spawn1.position - playerReference1.transform.position;
        
        portalCol = portal.GetComponent<SphereCollider>();

        panelVictoria.SetActive(false);

        //panelPantallaDeCarga.SetActive(false);

        ////text1ReferenceObject.Find("Time1");
        ////text2ReferenceObject.Find("Time2");

        //col1 = playerReference1.GetComponent<CapsuleCollider>();
        //plaC1 = playerReference1.GetComponent<PlayerController>();

        //col2 = playerReference2.GetComponent<CapsuleCollider>();
        //plaC2 = playerReference2.GetComponent<PlayerController>();

        ////text1.text = playerReference1.gameObject.transform.GetChild(0).GetChild(6).name = "Time1";
        ////text2.text = playerReference2.gameObject.transform.GetChild(0).GetChild(6).name = "Time2";



    }

    private void Awake()
    {
        tiempoActual = tiempoInicial;

    }

    private void Update()
    {
        playerReference2 = GameObject.FindGameObjectWithTag("Player2");

        if (playerReference2 != null)
        {
            Debug.Log("Se encontro player 2");
        }
        else
        {
            Debug.Log("No se encontro player 2");
        }

        //if(plaC1 != null)
        //{
        //    Debug.LogWarning("Encontrado player Script");
        //}

        //if (playerReference1 != null)
        //{
        //    Debug.LogWarning("Object player1 encontrado: ");

        //}

        //if (col1 != null)
        //{
        //    Debug.LogWarning("Encontrado colider player");
        //}

        //CheckArea();
        //CheckTime();
        //FindAnyObject();
    }

    void FindAnyObject()
    {
        if (text1 == null || text2 == null)
        {
            //text1.text = playerReference1.gameObject.transform.GetChild(0).GetChild(6).name = "Time1";
            //text2.text = playerReference2.gameObject.transform.GetChild(0).GetChild(6).name = "Time2";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerReference1.tag == "Player1")
        {
            if (!player2InArea)
            {
                player1InArea = true;
                StartCoroutine(IrAlSiguienteNivel());
            }
        }

        else if (playerReference2.tag == "Player2")
        {
            if (!player1InArea)
            {
                player2InArea = true;
                StartCoroutine(IrAlSiguienteNivel());
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (playerReference1.tag == "Player1")
        {
            if (player1InArea)
            {
                player1InArea = false;
                tiempoActual = tiempoInicial;
                //text1.enabled = false;
                StopCoroutine(IrAlSiguienteNivel());
            }
        }

        else if (playerReference2.tag == "Player2")
        {
            if (player2InArea)
            {
                player2InArea = false;
                tiempoActual = tiempoInicial;
                //text2.enabled = false;
                StopCoroutine(IrAlSiguienteNivel());
            }
        }

    }

    void CheckArea()
    {
        if (player1InArea && !player2InArea)
        {
            Debug.LogWarning("Si entra al CheckArea");
            ReducirTiempo();
        }

        else if (!player1InArea && player2InArea)
        {
            ReducirTiempo();
        }
        else
        {
            return;
        }
    }

    void CheckTime()
    {
        if (tiempoActual <= 0)
        {
            Debug.LogWarning("Entro a la Corutina");
            StartCoroutine(nextLevelCoroutine());
        }
    }

    void ReducirTiempo()
    {
        if (player1InArea)
        {

            text1.enabled = true;
            text1.text = tiempoActual.ToString();
            tiempoActual -= Time.deltaTime * multiplicador;
        }

        else if (player2InArea) //Testiar haber si no ocaciona problemas con el prendido y apagado de la vaiable text1 y text2, si si los ocaciona, cambiar el "else if" por un "if" y debajo del mismo, agregarle el else con su respectiva linea de codigo para apagar las variables text1 y 2.
        {
            text2.enabled = true;
            text2.text = tiempoActual.ToString();
            tiempoActual -= Time.deltaTime * multiplicador;
        }
        else
        {
            text1.enabled = false;
            text2.enabled = false;
        }
    }

    void TrasladarPlayer()
    {
        //Instantiate(playerReference1, spawn1.transform.position, Quaternion.identity);
        //Instantiate(playerReference2, spawn2.transform.position, Quaternion.identity);

        //playerReference1.transform.Translate(spawn1.transform.position);
        //playerReference2.transform.Translate(spawn2.transform.position);

        //playerReference1.transform.Translate(spawnPlayer1);
        Debug.Log("Se traslado el player");
        playerReference1.transform.Translate(spawnPlayer1.normalized * Time.deltaTime);
    }

    IEnumerator nextLevelCoroutine()
    {
        Debug.LogWarning("Se esta ejecutando la Corutina");
        //panelPantallaDeCarga.SetActive(true);
        //playerReference1.SetActive(false);
        col1.enabled = false;
        col2.enabled = false;
        plaC1.enabled = false;
        plaC2.enabled = false;
        map1.SetActive(false);
        map2.SetActive(true);
        //TrasladarPlayer();
        yield return new WaitForSeconds(5f);
        //panelPantallaDeCarga.SetActive(false);
        //playerReference1.SetActive(true);
        tiempoActual = tiempoInicial;
        col1.enabled = true; col2.enabled = true;
        yield return new WaitForSeconds(2f);
        plaC1.enabled = true; plaC2.enabled = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator IrAlSiguienteNivel()
    {
        map1.SetActive(false);
        playerReference1.GetComponent<PlayerController>().enabled = false;
        playerReference2.GetComponent<PlayerController>().enabled = false;
        portalCol.enabled = false;
        //TrasladarPlayer();
        yield return new WaitForSeconds(0.5f);
        map2.SetActive(true);
        panelVictoria.SetActive(true);
        yield return new WaitForSeconds(2f);
        playerReference1.GetComponent<PlayerController>().enabled = true;
        playerReference2.GetComponent<PlayerController>().enabled = true;
        yield return new WaitForSeconds(10f);
        portalCol.enabled = true;
    }


}
