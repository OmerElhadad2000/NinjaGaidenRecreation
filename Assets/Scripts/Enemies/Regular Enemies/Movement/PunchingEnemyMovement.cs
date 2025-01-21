using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingEnemyMovement : BasicEnemyMovement
{
    private static readonly int CanSeePlayer = Animator.StringToHash("CanSeePlayer");
    private static readonly int EnemyHit = Animator.StringToHash("EnemyHit");

    [Header("For Jump Attack")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    private const float Cooldown = 1f; 
    private bool _isGrounded;
    private bool _canJump = true; 
    
    private void FixedUpdate()
    {
        CheckingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        CheckingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        _isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        PlayerInRange = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        // AnimationController();
        if (!PlayerInRange && _isGrounded)
        {
            Patrolling();
        }
        if (!PlayerInRange || !_isGrounded || !_canJump) return;
        FlipTowardsPlayer();
        JumpAttack();
    }

    private void JumpAttack()
    {
        float distanceFromPlayer = Player.position.x - transform.position.x;

        if (_isGrounded)
        {
            EnemyRigidbody2D.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
            EnemyAnimator.SetTrigger(CanSeePlayer);
        }
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(Cooldown);
        _canJump = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    }
    
    override 
    public void Reset()
    {
        base.Reset();
        _isGrounded = true;
        _canJump = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Attack"))
        {
            EnemyDead = true;
            EnemyRigidbody2D.simulated = false;
            EnemyAnimator.SetTrigger(EnemyHit);
        }
    }
    
    private void OnEnemyGotHit()
    {
        EnemyReturned(EnemySpawnerId, true);
        BoxerPool.Instance.Return(this);
    }
    
    private void OnBecameInvisible()
    {
        if (EnemyDead) return;
        EnemyReturned(EnemySpawnerId, false);
        BoxerPool.Instance.Return(this);
    }
}
