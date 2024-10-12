using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Bullet speed
    public Color bulletColor { get; private set; }
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Move bullet upwards relative to its rotation
        rb.velocity = transform.up * speed;
    }

    public void SetColor(Color color)
    {
        bulletColor = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.enemyColor == bulletColor)
                {
                    // Same color: destroy both
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    // Different color: destroy only bullet
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        // Destroy bullet if it goes off-screen
        Destroy(this.gameObject);
    }
}
