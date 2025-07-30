using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject pauseMenu;

    public jump player;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
        StartPlayerMovement();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;  //pauses the game
        StopPlayerMovement(); //stops player from looking left right
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;  //unpauses
        StartPlayerMovement();
    }

    void StopPlayerMovement()
    {
        player.enabled = false;
    }

    void StartPlayerMovement()
    {
        player.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }


}
