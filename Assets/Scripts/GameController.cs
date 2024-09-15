using System.Collections;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] InteractableManager interactableManager;

    [SerializeField] private GameObject pauseUI;

    private bool isPaused = false; 
    private bool IsPaused 
    { 
        set 
        { 
            isPaused = value; 
            PauseGame();
        } 
    }

    public void GameStart() => gameManager.GameStart();
    public IEnumerator Start()
    {
        while(interactableManager.interactableGroups.Count == 0) yield return null;
        gameManager.LevelPrepare();
    }
    private void PauseGame()
    {
        pauseUI.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }
    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && gameManager.isGameGoing)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.Escape:
                    IsPaused = !isPaused;
                    break;
            }
        }
    }
}
