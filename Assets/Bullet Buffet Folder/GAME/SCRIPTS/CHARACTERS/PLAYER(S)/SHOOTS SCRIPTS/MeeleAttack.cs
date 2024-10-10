using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeeleAttack : MonoBehaviour
{
    public bool cantShoot;
    public GameObject objetoDa�o;

    private void Start()
    {
        cantShoot = true;
        objetoDa�o.SetActive(false);
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot)
        {
            Attack();
            cantShoot = false;
        }
    }

    void Attack()
    {
        objetoDa�o.SetActive(true);
        StartCoroutine(DesactivarObjetoDeDa�o());
    }

    IEnumerator DesactivarObjetoDeDa�o()
    {
        yield return new WaitForSeconds(0.5f);

        objetoDa�o.SetActive(false);

        yield return new WaitForSeconds(1f);

        cantShoot = true;
    }
}
