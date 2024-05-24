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

    [SerializeField] private bool gMEncontrado = false;

    GameManager gM;

    private void Start()
    {
        //Instantiate(prefabPlayer1, respawnPoint1.transform.position, respawnPoint1.transform.rotation);
        //Instantiate(prefabPlayer2, respawnPoint2.transform.position, respawnPoint2.transform.rotation);


        if (gM == null && !gMEncontrado)
        {
            gM = FindFirstObjectByType<GameManager>();
        }
        else if (gM != null && !gMEncontrado)
        {
            gMEncontrado = true;
        }

        if (gM.oneVone == true)
        {
            Instantiate(prefabPlayer1, respawnPoint1.localPosition, Quaternion.identity);
            Instantiate(prefabPlayer2, respawnPoint2.localPosition, Quaternion.identity);
        }
        else if (gM.twoVtwo == true)
        {
            Instantiate(prefabPlayer1, respawnPoint3.localPosition, Quaternion.identity);
            Instantiate(prefabPlayer2, respawnPoint4.localPosition, Quaternion.identity);
            Instantiate(prefabPlayer3, respawnPoint5.localPosition, Quaternion.identity);
            Instantiate(prefabPlayer4, respawnPoint6.localPosition, Quaternion.identity);
        }
    }

    private void Update()
    {
        if(!gMEncontrado)
        {
            gM = FindFirstObjectByType<GameManager>();
        }
    }
}
