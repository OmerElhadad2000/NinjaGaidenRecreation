using System;
using Mono_Pool;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class BasicSpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolableObject
{
    
    [Header("Spawner Settings")]
    [SerializeField] private int maxEnemiesInSpawner;
    [SerializeField] private float spawnRate; 
    [SerializeField] protected Transform player;
    [SerializeField] private MonoPool<T> enemyPool;
    [SerializeField] private int spawnerId;
    
    private int _currentEnemiesInSpawner;
    private float _spawnRateTimer;

    private void OnEnable()
    {
        BasicEnemyMovement.EnemyReturnedToPool += OnEnemyReturnedToPool;
    }

    private void Update()
    {
        if (_currentEnemiesInSpawner >= maxEnemiesInSpawner) return;
        _spawnRateTimer += Time.deltaTime;
        if (!(_spawnRateTimer >= spawnRate)) return;
        SpawnEnemy();
        _spawnRateTimer = 0;
        _currentEnemiesInSpawner++;
        print(_currentEnemiesInSpawner);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected virtual void SpawnEnemy()
    {
        var enemy = enemyPool.Get();
        enemy.SetPlayerTransform(player);
        enemy.transform.position = transform.position;
        enemy.GetComponent<BasicEnemyMovement>().SetEnemySpawnerId(spawnerId);
    }
    
    private void OnEnemyReturnedToPool(int enemySpawnerId)
    {
        if (enemySpawnerId != spawnerId) return;
        _currentEnemiesInSpawner--;
        print(_currentEnemiesInSpawner);
    }
    
    private void OnDisable()
    {
        BasicEnemyMovement.EnemyReturnedToPool -= OnEnemyReturnedToPool;
    }
}