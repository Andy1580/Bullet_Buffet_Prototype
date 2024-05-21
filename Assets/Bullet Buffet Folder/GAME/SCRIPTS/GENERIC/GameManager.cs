using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //SceneManager.sceneLoaded += OnSceneLoaded;

    public static GameManager Instance;

    public GameObject _playerOne;
    public GameObject _playerTwo;


    private void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


}
