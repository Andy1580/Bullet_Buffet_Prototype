using UnityEngine;

public class InstanciarPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] objetosParaInstanciar;

    private EnemyAI_Flying eF;
    /*
    void Start()
    {
        eF = GetComponent<EnemyAI_Flying>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (eF != null && eF.VidaEnemigo == 0)
        {
            InstanciarObjetoAleatorio();
        }
    }

    void InstanciarObjetoAleatorio()
    {
        if (objetosParaInstanciar.Length > 0)
        {
            int randomIndex = Random.Range(0, objetosParaInstanciar.Length);
            GameObject objetoSeleccionado = objetosParaInstanciar[randomIndex];
            GameObject objetoInstanciado = Instantiate(objetoSeleccionado, transform.position, transform.rotation);
            objetoInstanciado.name = objetoSeleccionado.name.Clone().ToString();
        }
    }
    */
    void Start()
    {
        InstanciarObjetoAleatorio();
    }

    void InstanciarObjetoAleatorio()
    {
        if (objetosParaInstanciar.Length > 0)
        {
            int randomIndex = Random.Range(0, objetosParaInstanciar.Length);
            GameObject objetoSeleccionado = objetosParaInstanciar[randomIndex];

            // Instancia el objeto sin cambiar su nombre
            GameObject objetoInstanciado = Instantiate(objetoSeleccionado, transform.position, transform.rotation);

            objetoInstanciado.name = objetoSeleccionado.name.Replace("(Clone)", "").Replace("(UnityEngine.GameObject)", "").Trim();

            // Simplemente imprime el nombre
            Debug.Log("Objeto instanciado: " + objetoInstanciado.name);
        }
    }
}
