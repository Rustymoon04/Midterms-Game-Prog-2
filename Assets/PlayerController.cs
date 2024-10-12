using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Position to spawn bullets
    public float fireRate = 1f; // Bullets per second
    public float rotationSpeed = 5f; // Speed of rotation towards enemy
    public float rangeRadius = 5f; // Detection range for enemies

    // Private Variables
    private float nextFireTime = 0f;
    private Color playerColor;

    // UI
    public GameObject gameOverUI;

    // Define possible colors
    private Color[] possibleColors = { Color.red, Color.green, Color.blue };

    void Start()
    {
        // Initialize player color
        ChangeColor(); // Sets a random color from the possibleColors array
    }

    void Update()
    {
        // Shooting mechanism
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Detect and rotate towards nearest enemy
        RotateTowardsNearestEnemy();

        // Detect player click to change color
        DetectPlayerClick();
    }

    void Shoot()
    {
        // Instantiate bullet at firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Set bullet color to player's color
        bullet.GetComponent<SpriteRenderer>().color = playerColor;
        // Set bullet's color property
        bullet.GetComponent<Bullet>().SetColor(playerColor);
    }

    void DetectPlayerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to detect if player was clicked
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                ChangeColor();
            }
        }
    }

    void ChangeColor()
    {
        // Select a random color from the possibleColors array
        playerColor = possibleColors[Random.Range(0, possibleColors.Length)];
        GetComponent<SpriteRenderer>().color = playerColor;
    }

    void RotateTowardsNearestEnemy()
    {
        GameObject nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != null)
        {
            // Calculate direction to enemy
            Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
            // Calculate the angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Smoothly rotate towards the enemy
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjusting for sprite orientation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    GameObject GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue; // Skip if enemy is destroyed
            float dist = Vector2.Distance(currentPos, enemy.transform.position);
            if (dist < minDist && dist <= rangeRadius)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    private void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }

    // Method to handle game over
    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }
}
