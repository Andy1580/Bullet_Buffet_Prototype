using System.Collections;
using UnityEngine;

public class HabilidadExplosion : MonoBehaviour
{
    [SerializeField] private Transform objetoExplosion;
    [SerializeField] private float escalaExplosion = 5f;
    [SerializeField] private float duracionExplosion = 3f;
    [SerializeField] private int dañoExplosion = 50;
    private PlayerController jugadorInvocador;
    private SphereCollider explosionCollider;
    private Vector3 escalaOriginal;

    private void Awake()
    {
        explosionCollider = objetoExplosion.GetComponent<SphereCollider>();
        explosionCollider.gameObject.SetActive(false);
    }

    void Start()
    {

        escalaOriginal = objetoExplosion.localScale;
        objetoExplosion.localScale = Vector3.zero;
    }

    public void ActivarHabilidad(PlayerController jugador)
    {
        jugadorInvocador = jugador;
        ActivarExplosion();
    }

    void ActivarExplosion()
    {
        explosionCollider.gameObject.SetActive(true);
        objetoExplosion.localScale = new Vector3(escalaExplosion, escalaExplosion, escalaExplosion);
        StartCoroutine(DesactivarExplosion());
    }

    IEnumerator DesactivarExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        objetoExplosion.localScale = Vector3.zero;
        explosionCollider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        EnemyAI_Flying eF = other.GetComponent<EnemyAI_Flying>();
        EnemyAI_Meele eM = other.GetComponent<EnemyAI_Meele>();

        if (other.gameObject == jugadorInvocador)
        {
            return;
        }
        if (other.gameObject.layer == 8 || other.gameObject.layer == 7) //8 jugadores / 7 enemigos
        {
            if (player != null)
            {
                player.Vida -= dañoExplosion;
            }

            if (eM != null)
            {
                eM.VidaEnemigo -= dañoExplosion;
            }

            if (eF != null)
            {
                eF.VidaEnemigo -= dañoExplosion;
            }
        }
    }
}
