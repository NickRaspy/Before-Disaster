using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherUIController : MonoBehaviour
{
    public void OpenScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void Unpause(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Exit() => Application.Quit();
}
