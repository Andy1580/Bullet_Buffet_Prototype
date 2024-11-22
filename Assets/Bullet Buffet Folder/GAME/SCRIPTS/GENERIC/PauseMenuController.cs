using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Gamepad activeGamepad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        gameObject.SetActive(false); // Mantener el menú de pausa inactivo hasta que sea necesario
    }

    public void EnablePauseMenu(Gamepad gamepad)
    {
        activeGamepad = gamepad;
        playerInput.SwitchCurrentControlScheme("Gamepad", gamepad); // Cambiar esquema de control
        playerInput.ActivateInput(); // Habilitar el input del menú de pausa
        gameObject.SetActive(true);
    }

    public void DisablePauseMenu()
    {
        playerInput.DeactivateInput();
        gameObject.SetActive(false);
    }

    public void OnSubmit()
    {
        // Lógica de Submit en el menú de pausa
        Debug.Log("Submit action in Pause Menu");
    }

    public void OnCancel()
    {
        //// Lógica para Cancelar o salir del menú de pausa
        //if(GameManager.Instance.panelPausa.activeSelf)
        //{
        //    GameManager.Instance.Resumir(); // Esto reanuda el juego
        //}
        
    }
}
