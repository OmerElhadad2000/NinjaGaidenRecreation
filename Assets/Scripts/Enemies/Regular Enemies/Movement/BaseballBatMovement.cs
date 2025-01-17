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
        if (other.CompareTag("Player Attack"))
        {
            EnemyRigidbody2D.simulated = false;
            EnemyAnimator.SetTrigger(EnemyHit); 
        }
    }
    
    private void OnHitByPlayer()
    {
        BaseballBatPool.Instance.Return(this);
    }
}
