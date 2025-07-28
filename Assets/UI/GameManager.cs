using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
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


}
