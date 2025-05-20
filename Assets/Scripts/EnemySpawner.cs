using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
  [Header("Spawn Settings")]
  public GameObject[] enemyPrefabs;
  public Transform[] spawnPoints;
  public float initialSpawnRate = 2.0f;
  public float minimumSpawnRate = 0.5f;
  public float difficultyRamp = 0.1f;

  [Header("Wave Settings")]
  public int enemiesPerWave = 5;
  public float timeBetweenWaves = 5.0f;
  public int currentWave = 0;

  private float _currentSpawnRate;
  private int _enemiesSpawned;
  private int _enemiesRemainingInWave;


  private void Start() {
    _currentSpawnRate = initialSpawnRate;
    _enemiesSpawned = 0;

    StartCoroutine(SpawnRoutine());
  }

  IEnumerator SpawnRoutine() {
    while (true) {
      if (_enemiesRemainingInWave > 0) {
        SpawnEnemy();
        _enemiesRemainingInWave--;
        _enemiesSpawned++;
      }

      yield return new WaitForSeconds(_currentSpawnRate);
    }
  }

  void SpawnEnemy() {
    if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0) {
      Debug.LogWarning("No spawn points or enemy prefabs assigned!");
      return;
    }

    // Choose a random spawn point
    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

    // Choose a random enemy prefab
    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

    // Spawn the enemy
    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
  }

  void StartNextWave() {
    currentWave++;

    // Increase difficulty
    _currentSpawnRate = Mathf.Max(minimumSpawnRate, initialSpawnRate - (difficultyRamp * currentWave));

    // Set enemies for this wave (could scale with wave number)
    _enemiesRemainingInWave = enemiesPerWave + (currentWave * 2);

    // Optional: Announce new wave
    Debug.Log("Wave " + currentWave + " started!");

    // When all enemies in this wave are spawned, wait and start the next wave
    StartCoroutine(WaitForNextWave());
  }
  
  IEnumerator WaitForNextWave()
  {
    // Wait until all enemies in the wave are spawned
    while (_enemiesRemainingInWave > 0)
    {
      yield return null;
    }
        
    // Wait the designated time between waves
    yield return new WaitForSeconds(timeBetweenWaves);
        
    // Start the next wave
    StartNextWave();
  }

}