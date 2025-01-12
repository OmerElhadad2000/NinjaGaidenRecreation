using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController enemyAnimator;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnDistance;
    private float _nextSpawnTime;
    private void Update()
    {
        if (!(Time.time > _nextSpawnTime)) return;
        _nextSpawnTime = Time.time + spawnRate;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        //use pool
        EnemyTemplate enemy = EnemyPool.Instance.Get();
        enemy.transform.position = transform.position;
        enemy.SetEnemyAnimationController(enemyAnimator);
    }
}
