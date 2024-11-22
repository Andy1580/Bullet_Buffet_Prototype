using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private DefaultInputActions inputActions;
    private Gamepad exclusiveGamepad;

    public static event Action OnSubmitAction;
    public static event Action OnCancelAction;

    private void Awake()
    {
        inputActions = new DefaultInputActions();
        Instance = this;
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Submit.performed += HandleSubmit;
        inputActions.UI.Cancel.performed += HandleCancel;
    }

    private void OnDisable()
    {
        inputActions.UI.Submit.performed -= HandleSubmit;
        inputActions.UI.Cancel.performed -= HandleCancel;
        inputActions.UI.Disable();
    }

    private void HandleSubmit(InputAction.CallbackContext context)
    {
        if (exclusiveGamepad == null || context.control.device == exclusiveGamepad)
        {
            //Debug.Log("HandleSubmit triggered by: " + context.control.device);
            OnSubmitAction?.Invoke();
        }
        else
        {
            //Debug.Log("HandleSubmit ignored by: " + context.control.device);
        }
    }

    private void HandleCancel(InputAction.CallbackContext context)
    {
        if (exclusiveGamepad == null || context.control.device == exclusiveGamepad)
        {
            //Debug.Log("HandleCancel triggered by: " + context.control.device);
            OnCancelAction?.Invoke();
        }
        else
        {
            //Debug.Log("HandleCancel ignored by: " + context.control.device);
        }
    }

    // Asignar un Gamepad exclusivo para la navegación
    public void SetExclusiveGamepad(Gamepad gamepad)
    {
        exclusiveGamepad = gamepad;
        //Debug.Log("Exclusive Gamepad set to: " + gamepad);
    }

    // Limpiar el Gamepad exclusivo para permitir el control compartido nuevamente
    public void ClearExclusiveGamepad()
    {
        //Debug.Log("Clearing exclusive Gamepad: " + exclusiveGamepad);
        exclusiveGamepad = null;
    }

    // Configurar los callbacks de panel activo
    public void SetActivePanel(Action submitCallback, Action cancelCallback)
    {
        // Desconectar anteriores
        OnSubmitAction = null;
        OnCancelAction = null;

        // Conectar nuevos callbacks
        if (submitCallback != null) OnSubmitAction += submitCallback;
        if (cancelCallback != null) OnCancelAction += cancelCallback;

        //Debug.Log("SetActivePanel configured. Submit: " + submitCallback + ", Cancel: " + cancelCallback);
    }
}
