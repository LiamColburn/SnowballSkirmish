using Unity.VisualScripting;
using UnityEngine;

public class Character_Script : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Shooting")]
    public GameObject snowballPrefab;
    public float fireRate = 1f;
    protected float timer;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip damagerSound;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        timer = fireRate;
    }
    
    protected virtual void Update()
    {
        timer -= Time.deltaTime;
    }

    // Shared shoot method — everyone uses this
    protected void Shoot(Vector2 direction)
    {
        // Plays snowball shoot audio
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        if (snowballPrefab == null)
        {
            Debug.LogWarning(gameObject.name + " has no snowball assigned!");
            return;
        }

        // Initializes a gameobject, that is cast slightly in front of the snowman
        // to avoid self inflicted damage in the direction of the mouse at time on input
        GameObject sb = Instantiate(snowballPrefab, transform.position, Quaternion.identity);
        sb.GetComponent<Snowball_Script>().ThrowSnowball(transform.position + (Vector3)(direction * 0.5f), direction);
    }

    // Plays damage audio, and calls Die() is health <= 0
    public virtual void TakeDamage(int amount)
    {
        AudioSource.PlayClipAtPoint(damagerSound, transform.position);
        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHealth);
        if (currentHealth <= 0) Die();
    }


    // Only for enemies, overrided for player
    protected virtual void Die()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            if (ScoreCounter.instance != null)
            {   
                // Add to score based on easy/hard enemy
                if (gameObject.GetComponent<Hard_Enemy_Script>() != null)
                    ScoreCounter.instance.AddScore(20);
                else
                    ScoreCounter.instance.AddScore(10);

                Debug.Log("Score: " + ScoreCounter.instance.score);
            }

            // Play audio for snowman death
            AudioSource.PlayClipAtPoint(damagerSound, transform.position);
        }

        // Destroy the object, no need for it anymore!
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
}