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
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        if (snowballPrefab == null)
        {
            Debug.LogWarning(gameObject.name + " has no snowball assigned!");
            return;
        }

        GameObject sb = Instantiate(snowballPrefab, transform.position, Quaternion.identity);
        sb.GetComponent<Snowball_Script>().ThrowSnowball(transform.position + (Vector3)(direction * 0.5f), direction);
    }

    public virtual void TakeDamage(int amount)
    {
        AudioSource.PlayClipAtPoint(damagerSound, transform.position);
        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHealth);
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            if (ScoreCounter.instance != null)
            {
                if (gameObject.GetComponent<Hard_Enemy_Script>() != null)
                    ScoreCounter.instance.AddScore(20);
                else
                    ScoreCounter.instance.AddScore(10);

                Debug.Log("Score: " + ScoreCounter.instance.score);
            }
            AudioSource.PlayClipAtPoint(damagerSound, transform.position);
        }

        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
}