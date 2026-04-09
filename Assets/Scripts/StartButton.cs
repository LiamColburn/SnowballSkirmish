using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public string sceneName;

    // In opening scene, this function is tied to "On Click"
    // Goes to specified scene (Map1)
    public void StartGame()
    {
        // Reset everything for a fresh game
        if (ScoreCounter.instance != null)
            ScoreCounter.instance.ResetGame();

        TotalScore.ResetTotal();

        SceneManager.LoadScene(sceneName);
    }
}