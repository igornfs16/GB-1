using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas _menu;
    [SerializeField] private Canvas _options;

    private void Start()
    {
        _menu.enabled = true;
        _options.enabled = false;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Options()
    {
        _menu.enabled = false;
        _options.enabled = true;
    }
    public void Back()
    {
        _menu.enabled = true;
        _options.enabled = false;
    }
}
