using System;
using UnityEngine;

public class BaseballBatMovement : BasicEnemyMovement
{
    private static readonly int EnemyHit = Animator.StringToHash("EnemyHit");

    void FixedUpdate()
    {
        CheckingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        CheckingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        PlayerInRange = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        if (!PlayerInRange)
        {
            Patrolling();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player Attack")) return;
        SetEnemyScore(100);
        EnemyDead = true;
        EnemyRigidbody2D.simulated = false;
        EnemyAnimator.SetTrigger(EnemyHit);
    }
    
    private void OnEnemyGotHit()
    {
        EnemyReturned(EnemySpawnerId, true);
        BaseballBatPool.Instance.Return(this);
    }

    private void OnBecameInvisible()
    {
        if (EnemyDead) return;
        EnemyReturned(EnemySpawnerId, false);
        BaseballBatPool.Instance.Return(this);
    }
}
