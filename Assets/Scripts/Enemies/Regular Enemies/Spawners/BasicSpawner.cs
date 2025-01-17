using System;
using Mono_Pool;
using UnityEngine;
using UnityEngine.Pool;

public class BasicSpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolableObject
{
    [SerializeField] private int maxEnemiesInSpawner;
    [SerializeField] private int currentEnemiesInSpawner;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRateTimer;
    [SerializeField] protected Transform player;
    [SerializeField] private MonoPool<T> enemyPool;

    private void Update()
    {
        if (currentEnemiesInSpawner >= maxEnemiesInSpawner) return;
        spawnRateTimer += Time.deltaTime;
        if (!(spawnRateTimer >= spawnRate)) return;
        SpawnEnemy();
        spawnRateTimer = 0;
        currentEnemiesInSpawner++;
    }

    protected virtual void SpawnEnemy()
    {
        var enemy = enemyPool.Get();
        enemy.SetPlayerTransform(player);
        enemy.transform.position = transform.position;
    }
}