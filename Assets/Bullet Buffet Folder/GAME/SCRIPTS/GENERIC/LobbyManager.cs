using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager self;

    private PlayerInput[] inputs;
    private Dictionary<Gamepad, PlayerInput> dicControles = new Dictionary<Gamepad, PlayerInput>();

    private void Awake()
    {
        self = this;
        
        inputs = GetComponentsInChildren<PlayerInput>();
        Awake_DesactivarControles();



        Awake_AsignarControles();
        InputSystem.onDeviceChange += CambiosEnControl;
    }


    void Awake_DesactivarControles()
    {
        foreach(PlayerInput input in inputs)
            input.gameObject.SetActive(false);
    }

    void Awake_AsignarControles()
    {
        var gamepads = Gamepad.all;

        for(int i = 0; i < gamepads.Count; i++)
        {
            inputs[i].gameObject.SetActive(true);
            dicControles[gamepads[i]] = inputs[i];
        }
    }

    //Cada vez que haya un cambio de control
    private void CambiosEnControl(InputDevice device, InputDeviceChange cambio)
    {
        //Si no es un gamepad, no continua con el proceso
        if (!(device is Gamepad))
            return;

        //Polimorfismo: De InputDevice a Gamepad
        Gamepad gamepad = device as Gamepad;

        //Control Conectado
        if (cambio == InputDeviceChange.Added)
        {
            //Buscamos en los Player Inputs o Controles
            foreach(PlayerInput input in inputs)
            {
                //En el que este deshabilitado, tenga el espacio libre y no tenga un gamepad asignado
                if (!input.gameObject.activeSelf)
                {
                    //Se activa el control
                    input.gameObject.SetActive(true);

                    //Se agrega al diccionario
                    dicControles[gamepad] = input;
                    break;
                }
            }
        }

        //Control Desconectado
        else if (cambio == InputDeviceChange.Removed)
        {
            //Se desactiva el control
            dicControles[gamepad].gameObject.SetActive(false);

            //Se quita del diccionario
            dicControles.Remove(gamepad);
        }
    }

}
