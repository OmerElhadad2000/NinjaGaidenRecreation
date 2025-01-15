using System;
using Mono_Pool;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour, IPoolableObject
{
    [Header("For Patrolling")]
    [SerializeField] protected float moveSpeed;
    protected float MoveDirection = 1;
    protected bool FacingRight = true;
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected Transform wallCheckPoint;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float circleRadius;
    
    protected bool CheckingGround;
    protected bool CheckingWall;

    [Header("For SeeingPlayer")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Vector2 lineOfSite;
    [SerializeField] protected LayerMask playerLayer;
    private protected bool PlayerInRange;
    
    [Header("Other")]
    private protected Animator EnemyAnimator;
    private protected Rigidbody2D EnemyRigidbody2D;
    void Start()
    {
        EnemyRigidbody2D = GetComponent<Rigidbody2D>(); 
        EnemyAnimator = GetComponent<Animator>();        
    }

    protected void Patrolling()
    {
        if (!CheckingGround || CheckingWall)
        {
            if (FacingRight)
            {
                Flip();
            }
            else if (!FacingRight)
            {
                Flip();
            }
        }
        EnemyRigidbody2D.linearVelocity = new Vector2(moveSpeed * MoveDirection, EnemyRigidbody2D.linearVelocity.y);
    }

    protected void FlipTowardsPlayer()
    {
        
        float playerPosition = player.position.x - transform.position.x;
        
        if (playerPosition<0 && FacingRight)
        {
            Flip();
        }
        else if (playerPosition>0 && !FacingRight)
        {
            Flip();
        }
    }
    protected void Flip()
    {
        MoveDirection *= -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
    
        Gizmos.color = Color.green;
    
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    
    }

    public void Reset()
    {
        MoveDirection = 1;
        FacingRight = true;
        EnemyRigidbody2D.linearVelocity = Vector2.zero;
        CheckingGround = false;
        CheckingWall = false;
        PlayerInRange = false;
        transform.rotation = Quaternion.identity;
    }
}
