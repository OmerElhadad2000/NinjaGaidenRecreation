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
    [SerializeField] private bool faceRight;

    private int _currentEnemiesInSpawner;
    private float _spawnRateTimer;
    private bool _isVisible;
    private int _enemiesKilled;

    private void OnEnable()
    {
        BasicEnemyMovement.EnemyReturnedToPool += OnEnemyReturnedToPool;
    }

    private void Update()
    {
        if (_enemiesKilled == maxEnemiesInSpawner || !_isVisible || _currentEnemiesInSpawner >= maxEnemiesInSpawner) return;
        _spawnRateTimer += Time.deltaTime;
        if (!(_spawnRateTimer >= spawnRate)) return;
        SpawnEnemy();
        _spawnRateTimer = 0;
        _currentEnemiesInSpawner++;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected virtual void SpawnEnemy()
    {
        var enemy = enemyPool.Get();
        enemy.transform.position = transform.position;
        //preform a flip if we want the spawner to face right using Flip of enemy
        enemy.GetComponent<BasicEnemyMovement>().SetEnemySpawnerId(spawnerId);
        enemy.SetPlayerTransform(player);
        if (!faceRight)
        {
            enemy.GetComponent<BasicEnemyMovement>().Flip();
        }
    }

    private void OnEnemyReturnedToPool(int enemySpawnerId, bool returnedByHit)
    {
        if (enemySpawnerId != spawnerId) return;
        if (returnedByHit)
        {
            _enemiesKilled++;
        }
        _currentEnemiesInSpawner--;
    }
    
    protected virtual void OnBecameInvisible()
    {
        _isVisible = false;
    }
    
    protected virtual void OnBecameVisible()
    {
        _enemiesKilled = 0;
        _spawnRateTimer = spawnRate;
        _isVisible = true;
    }
    
    private void OnDisable()
    {
        BasicEnemyMovement.EnemyReturnedToPool -= OnEnemyReturnedToPool;
    }

}