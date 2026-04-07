using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class Snowman_Script : Character_Script
{
    [Header("Movement")]
    public float speed = 5f;
    public float cooldown = 0.8f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        // Movement — new Input System
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            moveInput.x = (keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0);
            moveInput.y = (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0);
            moveInput = moveInput.normalized;
        }

        // Shoot on click — new Input System
        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame && timer <= 0f)
        {
            timer = cooldown;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            mouseWorld.z = 0f;
            Vector2 direction = (mouseWorld - transform.position).normalized;
            Shoot(direction);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    protected override void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        TotalScore.AddToTotal(ScoreCounter.instance.score);
        ScoreCounter.playerWon = false;
        StartCoroutine(DeathSequence());
    }

    System.Collections.IEnumerator DeathSequence()
    {
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Wait for sound to finish before loading
        float waitTime = deathSound != null ? deathSound.length : 1f;
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("Credits");
    }
}