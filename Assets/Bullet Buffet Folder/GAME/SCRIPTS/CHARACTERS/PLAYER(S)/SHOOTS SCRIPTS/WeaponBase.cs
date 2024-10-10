using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class WeaponBase : MonoBehaviour
{
    [Header("Atributos de Arma")]
    public float cadencia = 0.5f;  
    public int cantidadDeDisparos = 1;        
    public float dispersion = 0.1f;     
    public bool cantShoot;
    public Transform bocaArma;      
    public GameObject proyectilPrefab; 
    public float velocidadProyectil = 50f; 
    public float vidaBala = 2f;

    protected float siguienteDisparo = 0f;

    private void Start()
    {
        cantShoot = true;
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot)
        {
            Disparar();
        }
    }

    private void Disparar()
    {


        if (Time.time < siguienteDisparo) return;

        Vector3 spreadDirection = bocaArma.forward + new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), 0);

        for (int i = 0; i < cantidadDeDisparos; i++)
        {
            Vector3 finalPoint = bocaArma.position + spreadDirection;
            InstanciarProyectil(finalPoint);
        }

        siguienteDisparo = Time.time + cadencia;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(bocaArma.position, bocaArma.position + bocaArma.forward * 100);
    }

    /*
    private void Fire()
    {
        // Verifica si puede disparar (basado en la cadencia)
        if (Time.time < siguienteDisparo) return;

        // Disparar varios raycasts (por ejemplo, para una escopeta)
        for (int i = 0; i < cantidadDeDisparos; i++)
        {
            // Calcular direcci�n con dispersi�n
            Vector3 spreadDirection = bocaArma.forward + new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), 0);

            RaycastHit hit;

            LayerMask layerMask = (1 << 8) | (1 << 7);  // Capa 8 es "Players" y Capa 7 es "Enemies"

            if (Physics.SphereCast(bocaArma.position, radioEsfera, bocaArma.forward, out hit, rango, layerMask, QueryTriggerInteraction.Collide))
            {
                Debug.Log("Impacto en: " + hit.collider.name);

                if (hit.collider.gameObject.layer == 7)
                {
                    EnemyAI_Meele eM = hit.collider.GetComponent<EnemyAI_Meele>();
                    EnemyAI_Flying eF = hit.collider.GetComponent<EnemyAI_Flying>();

                    if (eM != null)
                    {
                        Debug.Log("Le di a: " + eM.gameObject.name);
                        eM.VidaEnemigo -= daño;
                        eM.animator.SetTrigger("da�o");
                    }
                    else if (eF != null)
                    {
                        Debug.Log("Le di a: " + eF.gameObject.name);
                        eF.VidaEnemigo -= daño;
                        eF.animator.SetTrigger("da�o");
                    }
                }
                else if (hit.collider.gameObject.layer == 8)
                {
                    PlayerController player = hit.collider.GetComponent<PlayerController>();

                    player.Vida -= daño;
                }

                // Instanciar el proyectil visual y moverlo hacia el punto de impacto
                InstanciarProyectil(hit.point);
            }
            else
            {
                // Si no impacta, hacer que el proyectil vaya hacia la m�xima distancia
                Vector3 finalPoint = bocaArma.position + spreadDirection * rango;
                InstanciarProyectil(finalPoint);
            }
        }

        // Establecer el tiempo para el pr�ximo disparo
        siguienteDisparo = Time.time + cadencia;
    }
    */
    void InstanciarProyectil(Vector3 objetivo)
    {
        GameObject proyectil = Instantiate(proyectilPrefab, bocaArma.position, Quaternion.identity);

        Vector3 direccion = (objetivo - bocaArma.position).normalized;
        proyectil.GetComponent<Rigidbody>().velocity = direccion * velocidadProyectil;

        Destroy(proyectil, vidaBala);
    }
}

