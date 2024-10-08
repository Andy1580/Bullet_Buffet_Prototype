using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class WeaponBase : MonoBehaviour
{
    [Header("Atributos de Arma")]
    public int daño = 10;       // Da�o por disparo
    public float cadencia = 0.5f;    // Cadencia (tiempo entre disparos)
    public float rango = 50f;        // Distancia m�xima del disparo
    public int cantidadDeDisparos = 1;        // Cantidad de raycasts por disparo (1 para pistola, varios para escopeta)
    public float dispersion = 0.1f;      // Dispersi�n (�ngulo de desviaci�n de los raycasts)
    public bool cantShoot;
    public Transform bocaArma;      // Punto de salida del disparo (boca del arma)
    //public GameObject vfxImpactoJugador;
    //public GameObject vfxImpactoObjeto;
    public GameObject projectilePrefab; // Prefab del proyectil visual
    public float velocidadProyectil = 50f; // Velocidad del proyectil visual
    //public float radioEsfera = 0.5f; // El radio de la esfera que simula el grosor del Raycast; // Velocidad del proyectil visual
    public float vidaBala = 2f;

    protected float siguienteDisparo = 0f; // Control del tiempo entre disparos

    private void Start()
    {
        cantShoot = true;
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot)
        {
            Disparar();
            //Fire();
        }
    }

    private void Disparar()
    {
        Ray ray = new Ray(bocaArma.position, bocaArma.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        //if (Time.time < siguienteDisparo) return;
        Vector3 spreadDirection = bocaArma.forward + new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), 0);

        //Imprimimos con que colisionamos
        print("Disparo: " + hit.collider.gameObject.name);

        //PLAYER
        if (hit.collider.gameObject.layer == 8)
        {
            PlayerController jugador = hit.collider.GetComponent<PlayerController>();
            jugador.Vida -= daño;
        }
        //ENEMIGO
        else if (hit.collider.gameObject.layer == 7)
        {
            EnemyAI_Flying eF = hit.collider.GetComponent<EnemyAI_Flying>();
            EnemyAI_Meele eM = hit.collider.GetComponent<EnemyAI_Meele>();

            if (eF != null)
            {
                eF.VidaEnemigo -= daño;
            }
            else if (eM != null)
            {
                eM.VidaEnemigo -= daño;
            }
        }
        //OTRA COSA
        else if (hit.collider.gameObject.layer == 0)
        {
            //Instancias el VFX donde colisiono el Raycast
            //Cambiar el VFX a cuando impacta con el Player o el Enemigo
            //Instantiate(vfxImpactoObjeto, hit.point, Quaternion.identity);                       //bocaArma.rotation
        }

        InstanciarProyectil(hit.point);

        //siguienteDisparo = Time.time + cadencia;

        //for (int i = 0; i < cantidadDeDisparos; i++)
        //{
            
        //}
        
    }

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
        GameObject proyectil = Instantiate(projectilePrefab, bocaArma.position, Quaternion.identity);

        // Calcula la direcci�n hacia el objetivo
        Vector3 direccion = (objetivo - bocaArma.position).normalized;
        proyectil.GetComponent<Rigidbody>().velocity = direccion * velocidadProyectil;

        Destroy(proyectil, vidaBala);
    }
}

