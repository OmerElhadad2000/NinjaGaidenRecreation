using System;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask slideLayer;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    
    public static event Action WallJumpingEnabled;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall Slide Left"))
        {
            if (other.transform.position.x < player.transform.position.x &&
                Physics2D.OverlapCircle(wallCheck.position, 0.2f,slideLayer ))
            {
                WallJumpingEnabled?.Invoke();
                rb.simulated = false;
            }
        }
        if (other.CompareTag("Wall Slide Right"))
        {
            if (other.transform.position.x > player.transform.position.x &&
                Physics2D.OverlapCircle(wallCheck.position, 0.2f, slideLayer))
            {
                WallJumpingEnabled?.Invoke();
                rb.simulated = false;
            }
        }
    }
}
