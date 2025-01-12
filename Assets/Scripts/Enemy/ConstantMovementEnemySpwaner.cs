using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ConstantMovementEnemySpwaner : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController enemyAnimator;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnDistance;
    private float _nextSpawnTime;
    [SerializeField] private Transform[] movementPoints;
    
    private void Update()
    {
        if (!(Time.time > _nextSpawnTime)) return;
        _nextSpawnTime = Time.time + spawnRate;
        SpawnEnemy();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnEnemy()
    {
        //use pool
        EnemyTemplate enemy = RegularMovementEnemyPool.Instance.Get();
        enemy.gameObject.GetComponent<ConstantMovementEnemy>().SetMovementPoints(movementPoints);
        enemy.transform.position = transform.position;
        enemy.SetEnemyAnimationController(enemyAnimator);
    }
}
