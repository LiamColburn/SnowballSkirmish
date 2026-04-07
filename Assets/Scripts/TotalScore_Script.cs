using UnityEngine;

public class TotalScore : MonoBehaviour
{
    public static TotalScore instance;
    public static int totalScore = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved total from disk
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
    }

    public static void AddToTotal(int amount)
    {
        totalScore += amount;
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
    }

    public static void ResetTotal()
    {
        totalScore = 0;
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.Save();
    }
}