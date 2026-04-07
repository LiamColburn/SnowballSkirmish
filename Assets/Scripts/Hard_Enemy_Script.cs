using UnityEngine;

public class Hard_Enemy_Script : Enemy_Script
{
    [Header("Tracking")]
    public float moveSpeed = 2f;
    public float chaseRange = 6f;

    [Header("Avoidance")]
    public float avoidanceDistance = 1f;
    public LayerMask obstacleLayer;

    protected override void Start()
    {
        base.Start();
        maxHealth += 50;
        currentHealth = maxHealth;
        fireRate /= 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < chaseRange)
        {
            Vector2 moveDirection = GetBestDirection();
            transform.position = Vector2.MoveTowards(
                transform.position,
                (Vector2)transform.position + moveDirection,
                moveSpeed * Time.deltaTime
            );
        }
    }

    Vector2 GetBestDirection()
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;

        int rayCount = 16;
        float bestScore = float.MinValue;
        Vector2 bestDirection = toPlayer;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = (360f / rayCount) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                dir,
                avoidanceDistance,
                obstacleLayer
            );

            if (hit.collider == null)
            {
                float score = Vector2.Dot(dir, toPlayer);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = dir;
                }
            }
        }

        return bestDirection;
    }
}