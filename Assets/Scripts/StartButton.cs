using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public string sceneName;

    public void StartGame()
    {
        // Reset everything for a fresh game
        if (ScoreCounter.instance != null)
            ScoreCounter.instance.ResetGame();

        TotalScore.ResetTotal();

        SceneManager.LoadScene(sceneName);
    }
}