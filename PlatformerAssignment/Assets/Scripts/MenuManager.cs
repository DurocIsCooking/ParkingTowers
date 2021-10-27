using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;

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
        OpenMainMenu(true);
        OpenPauseMenu(false);
        OpenGameOverMenu(false, "Game over!");
    }

    public void OpenMainMenu(bool isActive)
    {
        _mainMenu.SetActive(isActive);
    }

    public void OpenPauseMenu(bool isActive)
    {
        _pauseMenu.SetActive(isActive);
    }

    public void OpenGameOverMenu(bool isActive, string gameOverText)
    {
        _gameOverMenu.SetActive(isActive);
        _gameOverMenu.transform.GetChild(0).GetComponent<Text>().text = gameOverText;
    }

}
