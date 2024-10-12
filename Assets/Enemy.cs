using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // Enemy movement speed
    public Color enemyColor; // Enemy's color
    private Transform player;
    private PlayerController playerController;

    // Define possible colors
    private Color[] possibleColors = { Color.red, Color.green, Color.blue };

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        // Initialize enemy color
        ChangeColor();
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Move towards the player's position
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Check if enemy has reached the player
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < 0.5f)
            {
                // Trigger game over
                playerController.GameOver();
                Destroy(this.gameObject); // Optionally destroy the enemy
            }
        }
    }

    void ChangeColor()
    {
        // Select a random color from the possibleColors array
        enemyColor = possibleColors[Random.Range(0, possibleColors.Length)];
        GetComponent<SpriteRenderer>().color = enemyColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                if (bullet.bulletColor == enemyColor)
                {
                    // Same color: destroy both enemy and bullet
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    // Different color: destroy only bullet
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Optionally, handle any cleanup here
    }
}
