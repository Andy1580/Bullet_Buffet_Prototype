using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeeleAttack : MonoBehaviour
{
    public bool cantShoot;
    public GameObject objetoDaño;
    private PlayerController propietario;
    private void Start()
    {
        cantShoot = true;
        objetoDaño.SetActive(false);
        propietario = GetComponent<PlayerController>();
    }

    public void Input_Disparo(InputAction.CallbackContext context)
    {
        if (cantShoot && !propietario.muerto && !propietario.BloquearMovimiento && !propietario.escudo.gameObject.activeSelf)
        {
            Attack();
            cantShoot = false;
        }
    }

    void Attack()
    {
        objetoDaño.SetActive(true);
        propietario.animator.SetTrigger("ataque");

        AudioManager.instance.PlaySound("disparoSKYIE");

        StartCoroutine(DesactivarObjetoDeDaño());
    }

    IEnumerator DesactivarObjetoDeDaño()
    {
        yield return new WaitForSeconds(0.5f);

        objetoDaño.SetActive(false);

        yield return new WaitForSeconds(1f);

        cantShoot = true;
    }
}
