using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlayer1;
    [SerializeField] private GameObject prefabPlayer2;
    [SerializeField] private GameObject prefabPlayer3;
    [SerializeField] private GameObject prefabPlayer4;

    [SerializeField] private Transform respawnPoint1;
    [SerializeField] private Transform respawnPoint2;
    [SerializeField] private Transform respawnPoint3;
    [SerializeField] private Transform respawnPoint4;
    [SerializeField] private Transform respawnPoint5;
    [SerializeField] private Transform respawnPoint6;

    private void Start()
    {
        //Instantiate(prefabPlayer1, respawnPoint1.transform.position, respawnPoint1.transform.rotation);
        //Instantiate(prefabPlayer2, respawnPoint2.transform.position, respawnPoint2.transform.rotation);

        if (prefabPlayer1 == null)
        {
            prefabPlayer1 = GameObject.FindGameObjectWithTag("Player1");
        }

        
    }


    private void Update()
    {

        if (prefabPlayer2 == null)
        {
            prefabPlayer2 = GameObject.FindGameObjectWithTag("Player2");
        }


        if (prefabPlayer1.GetComponent<Vida>().salud <= 0)
        {
            //prefabPlayer1.GetComponent<Vida>().salud = 100;
            prefabPlayer1.transform.position = respawnPoint1.transform.position;

        }

        if (prefabPlayer2.GetComponent<Vida>().salud <= 0)
        {
            //prefabPlayer2.GetComponent<Vida>().salud = 100;
            prefabPlayer2.transform.position = respawnPoint2.transform.position;
        }
    }
}
