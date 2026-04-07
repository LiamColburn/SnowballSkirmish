using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Script : MonoBehaviour
{
    public string sceneName = "Intro";
    private float timer = 0f;
    public float displayTime = 5f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= displayTime)
            ReturnIntro();
    }

    public void ReturnIntro()
    {
        if (ScoreCounter.instance != null)
            ScoreCounter.instance.ResetGame();

        TotalScore.ResetTotal();

        SceneManager.LoadScene(sceneName);
    }
}