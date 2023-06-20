using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game...");
    }
}
