using System;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : BasicEnemyMovement
{
    private static readonly int EnemyHit = Animator.StringToHash("EnemyHit");
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private BoxCollider2D attackCollider;
    private const int InitHealth = 16;
    private int _health;
    private bool _isAttacking;
    private float _attackCooldownTimer = 5;

    public static event Action BossDied;

    private void OnEnable()
    {
        _health = InitHealth;
        CanvasManager.Instance.UpdateEnemyHealth(_health);
        EnemyAnimator = GetComponent<Animator>();
        EnemyRigidbody2D = GetComponent<Rigidbody2D>();
        attackCollider.enabled = false;
    }
    

    private void FixedUpdate()
    {  
        if (EnemyFrozen || _isAttacking)
        {
            return;
        }
        
        _attackCooldownTimer -= Time.deltaTime;
        print(_attackCooldownTimer);
        
        CheckingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        CheckingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        PlayerInRange = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);

        if (!PlayerInRange || _attackCooldownTimer > 0)
        {
            Patrolling();
            return;
        }

        FlipTowardsPlayer();
        SlashAttack();
    }

    private void SlashAttack()
    {
        EnemyRigidbody2D.simulated = false;
        _isAttacking = true;
        attackCollider.enabled = true;
        EnemyAnimator.SetTrigger(EnemyHit);
    }

    private void OnSlashAttackEnded()
    {
        print("slash attack ended");
        EnemyRigidbody2D.simulated = true;
        _isAttacking = false;
        attackCollider.enabled = false;
        _attackCooldownTimer = 5f; // Set cooldown duration
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player Attack")) return;
        TakeDamage();
    }

    private void TakeDamage()
    {
        _health--;
        CanvasManager.Instance.UpdateEnemyHealth(_health);
        if (_health > 0) return;
        EnemyFrozen = true;
        // EnemyAnimator.SetTrigger("EnemyDeath");
        BossDied?.Invoke();
    }

    public override void Reset()
    {
        base.Reset();
        _health = InitHealth;
        CanvasManager.Instance.UpdateEnemyHealth(_health);
    }
}