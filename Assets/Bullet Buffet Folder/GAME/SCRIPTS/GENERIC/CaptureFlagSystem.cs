using System.Collections;
using UnityEngine;

public class CaptureFlagSystem : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    private GameObject flag1;
    private GameObject flag2;
    [SerializeField] private GameObject leadFlag;

    private void Start()
    {
        CheckTags();
    }

    private void Awake()
    {

    }

    void CheckTags()
    {
        if (player1 != null && player2 != null)
        {
            Debug.LogWarning("Se encontraron los dos players");
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            Debug.LogWarning("Falta alguno de los players");
        }

        if (flag1 != null && flag2 != null)
        {
            Debug.LogWarning("Se encontraron las dos Flags");
            flag1 = GameObject.FindGameObjectWithTag("Flag1");
            flag2 = GameObject.FindGameObjectWithTag("Flag2");
        }
        else
        {
            Debug.LogWarning("Falta alguna de las Flags");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            leadFlag.SetActive(false);
            flag1.SetActive(true);
        }
        
        else if (other.gameObject.tag == "Player2")
        {
            leadFlag.SetActive(false);
            flag2.SetActive(true);
        }
    }

    public IEnumerator LeadFlagActivation()
    {
        yield return new WaitForSeconds(5f);
    }
}
