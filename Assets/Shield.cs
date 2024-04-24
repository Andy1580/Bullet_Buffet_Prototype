using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject lineRender;

    private Shoot shoot;

    private bool isShielding = false;

    [SerializeField] private float coolDown;

    private void Start()
    {
        shield.SetActive(false);
        lineRender.SetActive(true);

        shoot = GetComponent<Shoot>();
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isShielding)
            {
                ShieldHability();
            }
        }
    }

    void ShieldHability()
    {
        shoot.enabled = false;
        isShielding = true;
        shield.SetActive(true);
        lineRender.SetActive(false);
        StartCoroutine(TimeShield());
    }

    IEnumerator TimeShield()
    {
        yield return new WaitForSeconds(coolDown);
        isShielding = false;
        shield.SetActive(false);
        lineRender.SetActive(true);
        shoot.enabled = true;
        yield return new WaitForSeconds(coolDown);
    }
}
