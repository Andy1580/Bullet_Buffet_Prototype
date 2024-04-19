using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ChooseCharacterSystem cCS;

    

    public Transform _playerOne;
    public Transform _playerTwo;


    private void Awake()
    {
        MakeSingleton();

        SceneManager.sceneLoaded += OnSceneLoaded;
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene();
    }

    void CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "ANDY")
        {
            var playerGM = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerInputManager>();

            //playerGM.joinBehavior = _playerTwo.BroadcastMessage(name, _playerTwo, SendMessageOptions options);
        }
    }
}
