using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingEnemyMovement : BasicEnemyMovement
{
    private static readonly int CanSeePlayer = Animator.StringToHash("CanSeePlayer");

    [Header("For JumpAttacking")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    private const float Cooldown = 1f; 
    private bool _isGrounded;
    private bool _canJump = true; 

    private void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        _isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        // AnimationController();
        if (!canSeePlayer && _isGrounded)
        {
            Patrolling();
        }
        if (!canSeePlayer || !_isGrounded || !_canJump) return;
        FlipTowardsPlayer();
        JumpAttack();
    }

    private void JumpAttack()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        if (_isGrounded)
        {
            enemyRB.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
            enemyAnim.SetTrigger(CanSeePlayer);
        }
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(Cooldown);
        _canJump = true;
    }
    
    // private void AnimationController()
    // {
    //     enemyAnim.SetBool(CanSeePlayer, canSeePlayer);
    // }

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
}
