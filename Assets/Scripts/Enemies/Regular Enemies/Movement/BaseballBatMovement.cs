using UnityEngine;

public class BaseballBatMovement : BasicEnemyMovement
{
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
}
