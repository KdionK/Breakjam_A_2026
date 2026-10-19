using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject SpawnEnemy(Vector3 position)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is not assigned!");
            return null;
        }

        return Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}