using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Lobby : MonoBehaviour
{
    public static Lobby InstanceLobby;

    public GameObject playerUIPrefab;
    public List<GameObject> playerUIs = new List<GameObject>();

    private void Awake()
    {
        if(InstanceLobby != null)
        {
            Destroy(this);
        }
        else
        {
            InstanceLobby = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        InputSystem.onDeviceChange += CambioEnControl;
    }

    [SerializeField] private List<SlotJugador> slots;
    private void CambioEnControl(InputDevice device, InputDeviceChange cambio)
    {
        //if(!(device is Gamepad || !(device is Keyboard)) || !(device is Mouse))
        //    return;

        if (!(device is Gamepad))
            return;

        if (cambio == InputDeviceChange.Added)
        {
            foreach (SlotJugador slot in slots)
            {
                if (slot.Control == null)
                    slot.Control = device;
                AddPlayer();
                //GameManager.RegistrarJugadores();
            }
        }

        else if (cambio == InputDeviceChange.Removed)
        {
            foreach (SlotJugador slot in slots)
            {
                if (slot.Control == device)
                    slot.Control = null;
            }
        }


    }

    private void AddPlayer()
    {
        //var playerUI = Instantiate(playerUIPrefab, transform);
        //var playerInput = playerUI.GetComponent<PlayerInput>();

        //if (playerInput != null)
        //{
        //    InputUser.PerformPairingWithDevice(device, playerInput.user);
        //}

        //playerUIs.Add(playerUI);

        foreach(SlotJugador slot in slots)
        {
            Instantiate(slot.pfJugador);
        }
    }
}
