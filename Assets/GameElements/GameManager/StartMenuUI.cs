using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.SetState(GameManager.GameState.Playing);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
