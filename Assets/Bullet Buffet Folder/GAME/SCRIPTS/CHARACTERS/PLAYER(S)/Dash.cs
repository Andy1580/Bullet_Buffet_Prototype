using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Dash : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 1f; // Tiempo de espera entre Dashes

    private bool isDashing = false;
    private bool canDash = true;

    private PlayerController pC;

    private Shoot shoot;

    private void Start()
    {
        pC = GetComponent<PlayerController>();
        shoot = GetComponent<Shoot>();
    }

    void Update()
    {
        //if (!isDashing)
        //{
        //    float horizontalInput = Input.GetAxis("Horizontal");
        //    float verticalInput = Input.GetAxis("Vertical");

        //    Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        //    transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        //}
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        dash();
    }

    void dash()
    {
        // Detectar el input para el Input_Dash
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        if (canDash)
        {
            shoot.enabled = false;
            canDash = false;
            isDashing = true;

            pC.enabled = false; // Desactivar el control del pfJugador durante el Input_Dash

            Vector3 dashDirection = transform.position.normalized; // Direcci�n del Input_Dash (hacia adelante)
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
                yield return null;
            }

            isDashing = false;
            shoot.enabled = true;
            pC.enabled = true; // Reactivar el control del pfJugador despu�s del Input_Dash

            yield return new WaitForSeconds(dashCooldown);
            canDash = true; // Permitir otro Input_Dash despu�s del tiempo de espera
        }
    }
}
