using UnityEngine;

public class InstanciarPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] objetosParaInstanciar;
    private bool isDeadOneTime = false;

    private void Start()
    {
        isDeadOneTime = false;
    }

    public void InstanciarObjetoAleatorio()
    {
        if (objetosParaInstanciar.Length > 0 && !isDeadOneTime)
        {
            isDeadOneTime = true;

            int randomIndex = Random.Range(0, objetosParaInstanciar.Length);
            GameObject objetoSeleccionado = objetosParaInstanciar[randomIndex];

            // Instancia el objeto sin cambiar su nombre
            GameObject objetoInstanciado = Instantiate(objetoSeleccionado, transform.position, transform.rotation);

            objetoInstanciado.name = objetoSeleccionado.name.Replace("(Clone)", "").Replace("(UnityEngine.GameObject)", "").Trim();

            //// Simplemente imprime el nombre
            //Debug.Log("Objeto instanciado: " + objetoInstanciado.name);

        }

        Destroy(gameObject);
    }
}
