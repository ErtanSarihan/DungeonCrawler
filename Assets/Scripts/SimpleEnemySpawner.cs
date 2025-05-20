using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
  public GameObject enemyPrefab;
  public Transform[] spawnPoints;
  public float spawnInterval = 2.0f;
    
  private float _timer = 0f;
    
  void Update()
  {
    _timer += Time.deltaTime;
        
    if (_timer >= spawnInterval)
    {
      SpawnEnemy();
      _timer = 0f;
    }
  }
    
  void SpawnEnemy()
  {
    if (spawnPoints.Length == 0 || enemyPrefab == null)
    {
      Debug.LogWarning("Missing spawn points or enemy prefab!");
      return;
    }
        
    // Choose a random spawn point
    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
    // Spawn the enemy
    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
    // Set the enemy's scale
        
  }
}