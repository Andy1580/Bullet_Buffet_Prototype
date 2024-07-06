using UnityEngine;

public class ModeKaiju : MonoBehaviour
{
    [SerializeField] private Transform targetObject1; // El primer objeto hacia el cual se moverá el GameObject
    [SerializeField] private Transform targetObject2; // El segundo objeto hacia el cual se moverá el GameObject
    [SerializeField] private GameObject objectToMove; // El GameObject que se moverá

    private GameObject playerPrefab1;
    private GameObject playerPrefab2;

    private void Start()
    {
        if (playerPrefab1 == null)
        {
            playerPrefab1 = GameObject.FindGameObjectWithTag("Player1");
        }
    }

    void Update()
    {
        if (playerPrefab2 == null)
        {
            playerPrefab2 = GameObject.FindGameObjectWithTag("Player2");
        }

        if (playerPrefab1.GetComponent<PlayerController>().Vida <= 0)
        {
            MoveToObjectPosition(targetObject1.position);
        }

        if (playerPrefab2.GetComponent<PlayerController>().Vida <= 0)
        {
            MoveToObjectPosition(targetObject2.position);
        }

    }

    void MoveToObjectPosition(Vector3 targetPosition)
    {
        // Mover el objeto a la posición del mago
        objectToMove.transform.position = targetPosition * Time.deltaTime;
    }

}
