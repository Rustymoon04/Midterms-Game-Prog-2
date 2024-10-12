using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab
    public float spawnInterval = 2f; // Time between spawns
    public float spawnRadius = 10f; // Distance from player to spawn

    void Start()
    {
        // Start spawning enemies
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Determine a random position around the player within spawnRadius
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * spawnRadius;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
