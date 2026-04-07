using UnityEngine;

public class Enemy_Script : Character_Script
{
    protected Transform player;

    [Header("Shooting")]
    public float shootRange = 6f;


    protected override void Start()
    {
        base.Start();
        maxHealth = 50;
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        // If within range
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (timer <= 0f && distance < shootRange)
        {
            timer = fireRate;
            Vector2 direction = (player.position - transform.position).normalized;
            Shoot(direction); // inherited from Character_Script
        }
    }
}