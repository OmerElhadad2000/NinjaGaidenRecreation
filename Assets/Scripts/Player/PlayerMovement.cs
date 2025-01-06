using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    [SerializeField]
    private Transform playerGroundPosition;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float moveSpeed;
    
    [SerializeField] private LayerMask wallLayer;
    
    [SerializeField] private Transform wallCheck;
    
    

    private bool _isJumping;
    
    private bool _isInAir;

    private bool _isCrouching;

    private float _lastXDirection;

    private bool _isWallSliding;
    
    private float _wallSlidingSpeed = 2f;
    
    private float xInput;
    
    

    [SerializeField] private Rigidbody2D playerRb;

    private void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        
        _isJumping = IsOnGround() && Input.GetKey(KeyCode.UpArrow);
        
        _isCrouching = Input.GetKey(KeyCode.DownArrow) && !_isInAir;
        
        WallSlide();
    }

    private void FixedUpdate()
    {
        
        
        
        
        // Move the player horizontally only if not crouching
        playerRb.linearVelocity = new Vector2(xInput * moveSpeed, playerRb.linearVelocity.y);

        if (xInput != 0)
        {
            _lastXDirection = xInput;
        }
        
        SetPlayerDirection();
        
        if (_isJumping)
        {
            JumpNoSword();
        }
        
        else if (_isCrouching)
        {
            Crouch();
        }
    }

    private void JumpNoSword()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
        playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    
    private void Crouch()
    {
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
    }

    private bool IsOnGround()
    {
        var hit = Physics2D.Raycast(playerGroundPosition.position, Vector2.down, 0.2f, groundLayer);
        return hit.collider != null;
    }
    
    private void SetPlayerDirection()
    {
        if (_lastXDirection == 0) {return;}
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(_lastXDirection) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        
    }
    
    private void WallSlide()
    {
        if (IsWalled() && !IsOnGround() && xInput != 0f)
        {
            Debug.Log("Wall sliding");
            _isWallSliding = true;
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, Mathf.Clamp(playerRb.linearVelocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }
    
    
}