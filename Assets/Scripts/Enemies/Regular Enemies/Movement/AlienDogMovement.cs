using System;
using UnityEngine;

public class AlienDogMovement : BasicEnemyMovement
{
    private static readonly int EnemyHit = Animator.StringToHash("EnemyHit");

    [Header("For JumpAttacking")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    private bool _isGrounded;
    
    
    private void FixedUpdate()
    {
        CheckingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        CheckingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        _isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        if (_isGrounded)
        {
            Patrolling();
        }
    }

    private new void Patrolling()
    {
        if (CheckingWall)
        {
            Flip();
        }
        EnemyRigidbody2D.linearVelocity = new Vector2(moveSpeed * MoveDirection, jumpHeight);
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
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Attack"))
        {
            EnemyRigidbody2D.simulated = false;
            EnemyAnimator.SetTrigger(EnemyHit);
        }
    }
    
    private void OnBecameInvisible()
    {
        EnemyReturned(EnemySpawnerId, false);
        AlienDogPool.Instance.Return(this);
    }
    
    private void OnEnemyGotHit()
    {
        EnemyReturned(EnemySpawnerId, true);
        AlienDogPool.Instance.Return(this);
    }
    
    
}
