using UnityEngine;

public class ModeKaiju : MonoBehaviour
{
    public Transform targetObject1; // El primer objeto hacia el cual se moverá el GameObject
    public Transform targetObject2; // El segundo objeto hacia el cual se moverá el GameObject
    public GameObject objectToMove; // El GameObject que se moverá

    private GameObject playerPrefab1;
    private GameObject playerPrefab2;

    private void Start()
    {
        if(playerPrefab1 == null)
        {
            playerPrefab1 = GameObject.FindGameObjectWithTag("Player1");
        }
    }

    void Update()
    {
        if(playerPrefab2 == null)
        {
            playerPrefab2 = GameObject.FindGameObjectWithTag("Player2");
        }

        if(playerPrefab1.GetComponent<Vida>().salud <= 0)
        {
            MoveToObjectPosition(targetObject1.position);
        }

        if(playerPrefab2.GetComponent<Vida>().salud <= 0)
        {
            MoveToObjectPosition(targetObject2.position);
        }

        //// Comprobar si se pulsa la tecla A
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    MoveToObjectPosition(targetObject1.position);
        //}

        //// Comprobar si se pulsa la tecla D
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    MoveToObjectPosition(targetObject2.position);
        //}
    }

    void MoveToObjectPosition(Vector3 targetPosition)
    {
        // Mover instantáneamente el objeto a la posición del objetivo
        objectToMove.transform.position = targetPosition;
    }

}
