using System;
using UnityEngine;

public class EnemySpawnTimer : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timer;

    private void Start()
    {
        
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            Vector3 position = spawnPoint != null
                ? spawnPoint.position
                : transform.position;

            // EnemySpawner.Instance.SpawnEnemy(position);
            
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
}