using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

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

    public void OpenPauseMenu(bool isActive)
    {
        // Since not all scenes contain a pause menu
        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(isActive);
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

    // Load scenes and quit game

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void LoadGameEndMenu(string gameOverText)
    {
        SceneManager.LoadScene("GameEndMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
