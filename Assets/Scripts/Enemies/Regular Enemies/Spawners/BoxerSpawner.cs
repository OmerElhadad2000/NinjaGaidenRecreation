using UnityEngine;

public class BoxerSpawner : BasicSpawner<PunchingEnemyMovement>
{
    protected override void SpawnEnemy()
    {
        var enemy = BoxerPool.Instance.Get();
        enemy.SetPlayerTransform(player);
        enemy.transform.position = transform.position;
    }
}