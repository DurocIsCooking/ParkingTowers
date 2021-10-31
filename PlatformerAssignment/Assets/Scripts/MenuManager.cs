using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    public GameObject JumpIndicator;

    // Whether the game is paused
    public bool isPaused = false;

    // Singleton pattern
    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                MenuManager singleton = GameObject.FindObjectOfType<MenuManager>();
                if (singleton == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<MenuManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        OpenPauseMenu(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }    
        }
    }

    // Open / close pause menu
    public void PauseGame()
    {
        OpenPauseMenu(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        OpenPauseMenu(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void OpenPauseMenu(bool isActive)
    {
        // Since not all scenes contain a pause menu
        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(isActive);
        }

    }

    // Death menu
    public void OpenDeathMenu()
    {
        if(_deathMenu != null)
        {
            _deathMenu.SetActive(true);
        }
    }

    public void RespawnPlayer()
    {
        _deathMenu.SetActive(false);
        // Spawn player, attach the camera to them, and give them pointer to jump indicator
        GameObject newPlayer = Instantiate(_player, Player.RespawnPoint, Quaternion.identity);
        CameraMovement.Instance.AttachCameraToPlayer(newPlayer);
    }

    // Load scenes and quit game

    public void LoadLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void LoadMainMenu()
    {
        Player.MaxJumps = 1;
        SceneManager.LoadScene("MainMenu");
    }


    public void LoadGameEndMenu()
    {
        SceneManager.LoadScene("GameEndMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
