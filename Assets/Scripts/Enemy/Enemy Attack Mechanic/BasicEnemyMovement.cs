using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    [Header("For Patrolling")]
    [SerializeField] protected float moveSpeed;
    protected float moveDirection = 1;
    protected bool facingRight = true;
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected Transform wallCheckPoint;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float circleRadius;
    
    protected bool checkingGround;
    protected bool checkingWall;

    [Header("For SeeingPlayer")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Vector2 lineOfSite;
    [SerializeField] protected LayerMask playerLayer;
    private protected bool canSeePlayer;
    
    [Header("Other")]
    private protected Animator enemyAnim;
    private protected Rigidbody2D enemyRB;
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>(); 
        enemyAnim = GetComponent<Animator>();        
    }
    
    void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
        // AnimationController();
        if (!canSeePlayer)
        {
            Patrolling();
        }
       
        if (canSeePlayer)
        {
            OnPlayerSeen();
        }
    }

    void OnPlayerSeen()
    {
        Patrolling();
    }
    protected void Patrolling()
    {
        if (!checkingGround || checkingWall)
        {
            if (facingRight)
            {
                Flip();
            }
            else if (!facingRight)
            {
                Flip();
            }
        }
        enemyRB.linearVelocity = new Vector2(moveSpeed * moveDirection, enemyRB.linearVelocity.y);
    }

    protected void FlipTowardsPlayer()
    {
        
        float playerPosition = player.position.x - transform.position.x;
        
        if (playerPosition<0 && facingRight)
        {
            Flip();
        }
        else if (playerPosition>0 && !facingRight)
        {
            Flip();
        }
    }
    protected void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
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
}
