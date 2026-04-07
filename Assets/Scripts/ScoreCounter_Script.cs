using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;

    public int score = 0;

    [Header("Audio")]
    public AudioClip victorySound;


    [Header("Text Objects")]
    public Text scoreText;
    public Text healthText;
    public Text roundText;

    [Header("Backgrounds")]
    public GameObject scoreBg;
    public GameObject healthBg;
    public GameObject roundBg;

    private bool roundOver = false;
    public static bool playerWon = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (roundBg != null) roundBg.SetActive(false);
    }

    void Update()
    {
        // Update UI by finding text objects in the current scene
        Text scoreText = GameObject.Find("Scoretext")?.GetComponent<Text>();
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        Text healthText = GameObject.Find("HealthText")?.GetComponent<Text>();
        if (healthText != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Character_Script cs = player.GetComponent<Character_Script>()
                                   ?? player.GetComponentInChildren<Character_Script>();
                if (cs != null)
                    healthText.text = "HP: " + cs.currentHealth;
            }
            else
            {
                healthText.text = "HP: 0";
            }
        }

        if (!roundOver && 
            GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position);
            roundOver = true;
            if (roundBg != null) roundBg.SetActive(true);
            StartCoroutine(LoadNextScene());
        }
    }

    System.Collections.IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);

        roundOver = false;

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentScene == "Map1") {
            TotalScore.AddToTotal(score);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Map2");
        }
        else if (currentScene == "Map2")
        {
            TotalScore.AddToTotal(score);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Map3");
        }
        else if (currentScene == "Map3")
        {
            TotalScore.AddToTotal(score);
            playerWon = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        if (score % 40 == 0 && score > 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Character_Script cs = player.GetComponent<Character_Script>()
                                   ?? player.GetComponentInChildren<Character_Script>();
                if (cs != null)
                {
                    cs.currentHealth = cs.maxHealth;
                    Debug.Log("Health restored!");
                }
            }
        }
    }

    public void ResetGame()
    {
        score = 0;
        playerWon = false;
        roundOver = false;
    }
}