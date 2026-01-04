using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startMenuCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject victoryCanvas;
    public GameObject defeatCanvas;
    
    public enum GameState
    {
        Start,
        Playing,
        Paused,
        Victory,
        Defeat
    }

    public GameState currentState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetState(GameState.Start);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        Time.timeScale = 1f;

        if (startMenuCanvas) startMenuCanvas.SetActive(false);
        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(false);
        if (victoryCanvas) victoryCanvas.SetActive(false);
        if (defeatCanvas) defeatCanvas.SetActive(false);

        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 0f;
                if (startMenuCanvas) startMenuCanvas.SetActive(true);
                break;

            case GameState.Playing:
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                if (pauseMenuCanvas) pauseMenuCanvas.SetActive(true);
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                if (victoryCanvas) victoryCanvas.SetActive(true);
                break;

            case GameState.Defeat:
                Time.timeScale = 0f;
                if (defeatCanvas) defeatCanvas.SetActive(true);
                break;
        }
    }

    public bool IsPlaying()
    {
        return currentState == GameState.Playing;
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
            SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
            SetState(GameState.Playing);
    }

    public void TriggerVictory()
    {
        SetState(GameState.Victory);
    }

    public void TriggerDefeat()
    {
        SetState(GameState.Defeat);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
