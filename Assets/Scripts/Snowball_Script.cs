using UnityEngine;

public class Snowball_Script : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    private Vector2 direction;
    private bool hasHit = false;

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Move while no collision
    void Update()
    {
        if (!hasHit)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    // Self-contained — creates AND launches the snowball
    public void ThrowSnowball(Vector3 startPosition, Vector2 throwDirection)
    {
        
        // Set Direction and position
        transform.position = startPosition;
        direction = throwDirection.normalized;
    }

    // Determines what object was in the collision, only take damage if an enemy or player. Always break
    void OnCollisionEnter2D(Collision2D coll)
    {

        GameObject collidedWith = coll.gameObject;

        if (hasHit) return;

        if (collidedWith.CompareTag("Obstacle"))
        {
            HitSomething();
        }
        else if (collidedWith.CompareTag("Snowball"))
        {
            HitSomething();
        }
        else if (collidedWith.CompareTag("Enemy") || collidedWith.CompareTag("Player"))
        {
            collidedWith.GetComponent<Character_Script>()?.TakeDamage(25);
            HitSomething();
        }
    }

    // Destroy snowball when it hits
    void HitSomething()
    {
        hasHit = true;
        Destroy(gameObject);
    }
}